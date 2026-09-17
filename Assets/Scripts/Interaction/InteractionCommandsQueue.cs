using Commands;

namespace Interaction
{
    public class InteractionCommandsQueue : CommandsQueue<IInteractionCommand, InteractionCommandsContext>
    {
    
    }

    public interface IInteractionCommand : ICommand<ICommandContext> { }

    public abstract class InteractionCommandsContext : ICommandContext
    {
        public readonly PlayerInteractionComponent playerInteractionComponent;

        protected InteractionCommandsContext(PlayerInteractionComponent playerInteractionComponent)
        {
            this.playerInteractionComponent = playerInteractionComponent;
        }
    }
}