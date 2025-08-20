namespace ServiceDefaults.Middleware;

public interface IRequestContextAccessor
{
    RequestContext Current { get; }

    void SetCurrent(RequestContext context);
}


public class RequestContextAccessor : IRequestContextAccessor
{
    public RequestContext Current { get; private set; }
    
    public void SetCurrent(RequestContext context) => Current = context;
}

