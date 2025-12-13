namespace Home_5.API.Responses.Base;

public class ResponseBase<T>
{
    public ResponseBase(T? response, long elapsed)
    {
        Items = response;
        Elapsed = elapsed;
    }
    public T? Items { get; set; }
    public long Elapsed { get; set; }
}