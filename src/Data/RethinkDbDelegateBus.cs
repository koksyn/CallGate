using System;
using System.Collections.Generic;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;

namespace CallGate.Data
{
    internal enum ReqlExecutionType
    {
        Run,
        RunResult
    }

    public class RethinkDbDelegateBus : IRethinkDbDelegateBus
    {
        private readonly IList<KeyValuePair<ReqlExecutionType, Func<ReqlExpr>>> _delegateWrappers;
        private readonly Connection _connection;

        public RethinkDbDelegateBus(IRethinkDbConnectionFactory connectionFactory)
        {
            _delegateWrappers = new List<KeyValuePair<ReqlExecutionType, Func<ReqlExpr>>>();
            _connection = connectionFactory.CreateConnection();
            _connection.CheckOpen();
        }

        public void AddDelegateToRun(Func<ReqlExpr> commandDelegate)
        {
            var delegateWrapper = new KeyValuePair<ReqlExecutionType, Func<ReqlExpr>>(
                ReqlExecutionType.Run,
                commandDelegate
            );
            
            _delegateWrappers.Add(delegateWrapper);
        }

        public void AddDelegateToRunResult(Func<ReqlExpr> commandDelegate)
        {
            var delegateWrapper = new KeyValuePair<ReqlExecutionType, Func<ReqlExpr>>(
                ReqlExecutionType.RunResult,
                commandDelegate
            );
            
            _delegateWrappers.Add(delegateWrapper);
        }

        public bool HasDelegates()
        {
            return _delegateWrappers.Count > 0;
        }

        public void Commit()
        {
            if (HasDelegates())
            {
                foreach (var delegateWrapper in _delegateWrappers)
                {
                    ExecuteReql(delegateWrapper.Key, delegateWrapper.Value);
                }
                
                _delegateWrappers.Clear();
            }
        }

        private void ExecuteReql(ReqlExecutionType executionType, Func<ReqlExpr> commandDelegate)
        {
            var reqlExpr = commandDelegate();
            
            if (executionType == ReqlExecutionType.Run)
            {
                reqlExpr.Run(_connection);
            }
            else if (executionType == ReqlExecutionType.RunResult)
            {
                reqlExpr.RunResult(_connection);
            }
        }
    }
}