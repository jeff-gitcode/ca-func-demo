namespace Application.Abstraction;

public abstract class BaseHandler
{
    public static Task<TResult> Success<TResult>(TResult result) => Task.FromResult(result);
}