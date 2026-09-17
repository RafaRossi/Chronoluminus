using System.Threading;
using System.Threading.Tasks;

namespace DaySystem
{
    public class StartDayGameAction : GameActions
    {
        public override Task Execute(CancellationToken token)
        {
            DiscreteHourController.Instance.StartDay();
            return Task.CompletedTask;
        }
    }
}