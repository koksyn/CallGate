using System;
using CallGate.DependencyInjection;
using RethinkDb.Driver.Ast;

namespace CallGate.Data
{
    public interface IRethinkDbDelegateBus : IScopedDependency
    {
        void AddDelegateToRun(Func<ReqlExpr> commandDelegate);
        
        void AddDelegateToRunResult(Func<ReqlExpr> commandDelegate);
        
        bool HasDelegates();
        
        void Commit();
    }
}