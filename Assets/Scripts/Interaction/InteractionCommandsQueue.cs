using Commands;

namespace Interaction
{
    public class InteractionCommandsQueue : CommandsQueue<IInteractionCommand, InteractionCommandsContext>
    {
    
    }

    public interface IInteractionCommand : ICommand<ICommandContext> { }

    public class InteractionCommandsContext : ICommandContext
    {
        public readonly PlayerInteractionComponent playerInteractionComponent;

        public InteractionCommandsContext(PlayerInteractionComponent playerInteractionComponent)
        {
            this.playerInteractionComponent = playerInteractionComponent;
        }
    }
}