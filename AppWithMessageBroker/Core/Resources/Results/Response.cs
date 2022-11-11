namespace Core.Resources.Results
{
    public class Response : IResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
    }
}
