using System;
using System.Linq;
using CallGate.Data;
using CallGate.Documents;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;

namespace CallGate.Stores
{
    public class ChannelUserStore : Store<ChannelUser>, IChannelUserStore
    {
        public ChannelUserStore(
            IRethinkDbConnectionFactory connectionFactory,
            IRethinkDbDelegateBus rethinkDbDelegateBus
        ) : base(connectionFactory, rethinkDbDelegateBus) {}
        
        public ChannelUser GetOneByUserIdAndChannelId(Guid userId, Guid channelId)
        {
            Cursor<ChannelUser> all = R.Db(DbName)
                .Table(TableName)
                .Filter(a => a["UserId"].Eq(userId) && a["ChannelId"].Eq(channelId))
                .Run<ChannelUser>(Connection);
            
            var channelUsers = all.ToList();

            return channelUsers.FirstOrDefault();
        }

        public void RemoveByUserIdAndChannelId(Guid userId, Guid channelId)
        {
            ReqlExpr CommandDelegate() => R.Db(DbName)
                .Table(TableName)
                .Filter(a => a["UserId"].Eq(userId) && a["ChannelId"].Eq(channelId))
                .Delete();

            _rethinkDbDelegateBus.AddDelegateToRun(CommandDelegate);
        }
    }
}