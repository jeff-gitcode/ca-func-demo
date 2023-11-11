namespace Application.Abstraction;

public abstract class BaseHandler
{
    public static Task<TResult> Response<TResult>(TResult result) => Task.FromResult(result);
}