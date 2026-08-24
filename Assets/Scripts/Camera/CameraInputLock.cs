using System.Collections.Generic;

public static class CameraInputLock
{
    private static readonly HashSet<string> ActiveLocks = new HashSet<string>();

    public static bool IsLocked => ActiveLocks.Count > 0;

    public static void Lock(string reason)
    {
        ActiveLocks.Add(reason);
    }

    public static void Unlock(string reason)
    {
        ActiveLocks.Remove(reason);
    }

    public static IReadOnlyCollection<string> ActiveLockReasons => ActiveLocks;
}
