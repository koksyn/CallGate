namespace CallGate.Exceptions
{
    public interface IApiException
    {
        string GetMessage();
        int GetHttpStatusCode();
    }
}