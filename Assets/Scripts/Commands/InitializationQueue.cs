using System.Collections.Generic;
using System.Threading.Tasks;

namespace Commands
{
    public class InitializationQueue<T, TU> where T : ICommand<TU> where TU : ICommandContext
    {
        private readonly Queue<T> _queue = new();
        private TU _context;

        public void Initialize(TU context)
        {
            _context = context;
        }

        public void Enqueue(T action)
        {
            _queue.Enqueue(action);
        }

        public async Task ProcessAllAsync()
        {
            while (_queue.Count > 0)
            {
                var action = _queue.Dequeue();
                await action.Execute(_context);
            }
        }
    }
}