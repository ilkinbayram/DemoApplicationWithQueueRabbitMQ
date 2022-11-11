using Core.Resources.Model;
using Core.Resources.Results;

namespace Operation.Abstract
{
    public interface IFunction
    {
        void DoExecute();
        Task DoExecuteAsync();
    }

    public interface IFunction<TResult> where TResult : class, IResponse, new()
    {
        TResult DoExecute();
        Task<TResult> DoExecuteAsync();
        Task<TResult> DoExecuteAsync(string uri, DataMover dataMover);

        Task<TResult> DoExecuteAsync(IOperationModel model);
    }
}
