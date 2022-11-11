using Core.Resources.Model;
using Core.Resources.Results;
using Operation.Concrete.Base;

namespace web.Operation.Concrete
{
    public class HttpGetFunction<TResult> : Function<TResult>
        where TResult : class, IResponse, new()
    {
        public override async Task<TResult> DoExecuteAsync()
        {
            return null;
        }
    }
}
