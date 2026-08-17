using System.Threading.Tasks;

public interface ICommand<in T>
{
    Task Execute(T context);
}