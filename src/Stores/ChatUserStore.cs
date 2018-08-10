using System;
using System.Linq;
using CallGate.Data;
using CallGate.Documents;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;

namespace CallGate.Stores
{
    public class ChatUserStore : Store<ChatUser>, IChatUserStore
    {
        public ChatUserStore(
            IRethinkDbConnectionFactory connectionFactory,
            IRethinkDbDelegateBus rethinkDbDelegateBus
        ) : base(connectionFactory, rethinkDbDelegateBus) {}
        
        public ChatUser GetOneByUserIdAndChatId(Guid userId, Guid chatId)
        {
            Cursor<ChatUser> all = R.Db(DbName)
                .Table(TableName)
                .Filter(a => a["UserId"].Eq(userId) && a["ChatId"].Eq(chatId))
                .Run<ChatUser>(Connection);
            
            var chatUsers = all.ToList();

            return chatUsers.FirstOrDefault();
        }

        public void RemoveByUserIdAndChatId(Guid userId, Guid chatId)
        {
            ReqlExpr CommandDelegate() => R.Db(DbName)
                .Table(TableName)
                .Filter(a => a["UserId"].Eq(userId) && a["ChatId"].Eq(chatId))
                .Delete();

            _rethinkDbDelegateBus.AddDelegateToRun(CommandDelegate);
        }

        public void RemoveAllByChatId(Guid chatId)
        {
            ReqlExpr CommandDelegate() => R.Db(DbName)
                .Table(TableName)
                .Filter(a => a["ChatId"].Eq(chatId))
                .Delete();

            _rethinkDbDelegateBus.AddDelegateToRun(CommandDelegate);
        }
    }
}