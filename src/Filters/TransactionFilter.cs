using CallGate.Data;
using CallGate.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CallGate.Filters
{
    public class TransactionFilter : IActionFilterDependency
    {
        private readonly IRethinkDbDelegateBus _rethinkDbDelegateBus;
        private readonly IDatabaseManager _databaseManager;

        public TransactionFilter(IRethinkDbDelegateBus rethinkDbDelegateBus, IDatabaseManager databaseManager)
        {
            _rethinkDbDelegateBus = rethinkDbDelegateBus;
            _databaseManager = databaseManager;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception == null)
            {
                _databaseManager.Commit();
                _rethinkDbDelegateBus.Commit();
            }
        }
    }
}
