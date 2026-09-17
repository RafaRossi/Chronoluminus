using System.Threading.Tasks;
using Commands;

public interface ICommand<in T> : ICommand
{
    Task Execute(T context);
    Task ICommand.Execute() => Execute(default);
}

public interface ICommand
{
    Task Execute();
}