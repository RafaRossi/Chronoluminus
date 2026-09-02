using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueCarousel : MonoBehaviour
{
    [SerializeField] private GameObject optionPrefab;
    [SerializeField] private Transform container;

    [SerializeField] private float itemSpacingY = 60f;
    [SerializeField] private float lerpSpeed = 12f;

    [SerializeField] private AnimationCurve xOffsetCurve = AnimationCurve.EaseInOut(0, 50, 2, 0);

    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.Linear(0, 1.3f, 2, 0.7f);

    [SerializeField] private AnimationCurve alphaCurve = AnimationCurve.Linear(0, 1.0f, 2, 0.3f);

    private readonly List<RectTransform> _optionItems = new ();
    private readonly List<TMP_Text> _optionTexts = new ();
    private int _selectedIndex = 0;

    private void Update()
    {
        if (_optionItems.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            ChangeSelection(1);
        }

        UpdateCarouselVisuals();
    }

    public void SetOptions(List<string> options)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        _optionItems.Clear();
        _optionTexts.Clear();

        for (int i = 0; i < options.Count; i++)
        {
            GameObject obj = Instantiate(optionPrefab, container);
            RectTransform rect = obj.GetComponent<RectTransform>();
            TMP_Text tmp = obj.GetComponent<TMP_Text>();

            tmp.text = options[i];
            
            _optionItems.Add(rect);
            _optionTexts.Add(tmp);
        }

        _selectedIndex = 0;
    }

    private void ChangeSelection(int direction)
    {
        _selectedIndex = Mathf.Clamp(_selectedIndex + direction, 0, _optionItems.Count - 1);
    }

    private void UpdateCarouselVisuals()
    {
        for (int i = 0; i < _optionItems.Count; i++)
        {
            float distanceFromSelected = i - _selectedIndex;
            float absDistance = Mathf.Abs(distanceFromSelected);

            float targetY = -distanceFromSelected * itemSpacingY;

            float targetX = xOffsetCurve.Evaluate(absDistance);

            float targetScale = scaleCurve.Evaluate(absDistance);
            float targetAlpha = alphaCurve.Evaluate(absDistance);

            RectTransform rect = _optionItems[i];
            Vector3 targetPos = new Vector3(targetX, targetY, 0f);
            rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, targetPos, Time.deltaTime * lerpSpeed);
            rect.localScale = Vector3.Lerp(rect.localScale, Vector3.one * targetScale, Time.deltaTime * lerpSpeed);

            TMP_Text tmp = _optionTexts[i];
            Color c = tmp.color;
            c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * lerpSpeed);
            tmp.color = c;
        }
    }
}