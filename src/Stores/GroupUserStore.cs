using System;
using System.Linq;
using CallGate.Data;
using CallGate.Documents;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;

namespace CallGate.Stores
{
    public class GroupUserStore : Store<GroupUser>, IGroupUserStore
    {
        public GroupUserStore(
            IRethinkDbConnectionFactory connectionFactory,
            IRethinkDbDelegateBus rethinkDbDelegateBus
        ) : base(connectionFactory, rethinkDbDelegateBus) {}
        
        public GroupUser GetByUserIdAndGroupId(Guid userId, Guid groupId)
        {
            Cursor<GroupUser> all = R.Db(DbName)
                .Table(TableName)
                .Filter(a => a["UserId"].Eq(userId) && a["GroupId"].Eq(groupId))
                .Run<GroupUser>(Connection);
            
            var groupUsers = all.ToList();

            return groupUsers.FirstOrDefault();
        }

        public void RemoveByUserIdAndGroupId(Guid userId, Guid groupId)
        {
            ReqlExpr CommandDelegate() => R.Db(DbName)
                .Table(TableName)
                .Filter(a => a["UserId"].Eq(userId) && a["GroupId"].Eq(groupId))
                .Delete();

            _rethinkDbDelegateBus.AddDelegateToRun(CommandDelegate);
        }
    }
}