namespace Core.Resources.Results
{
    public interface IResponse
    {
        bool IsSuccess { get; set; }
        object Result { get; set; }
        string Message { get; set; }
    }
}
