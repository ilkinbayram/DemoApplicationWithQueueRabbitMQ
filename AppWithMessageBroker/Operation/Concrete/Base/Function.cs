using Core.Resources.Model;
using Core.Resources.Results;
using Operation.Abstract;

namespace Operation.Concrete.Base
{
    public class Function : IFunction
    {
        public virtual void DoExecute()
        {
        }

        public virtual Task DoExecuteAsync() => Task.CompletedTask;
    }


    public class Function<TOut> : IFunction<TOut>
        where TOut : class, IResponse, new()
    {
        public virtual TOut DoExecute() => null;

        public virtual Task<TOut> DoExecuteAsync() => null;
        public virtual Task<TOut> DoExecuteAsync(string uri, DataMover dataMover) => null;

        public virtual Task<TOut> DoExecuteAsync(IOperationModel model) => null;
    }
}
