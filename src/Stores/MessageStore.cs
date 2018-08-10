using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Documents;
using RethinkDb.Driver.Net;

namespace CallGate.Stores
{
    public class MessageStore : Store<Message>, IMessageStore
    {
        public MessageStore(
            IRethinkDbConnectionFactory connectionFactory,
            IRethinkDbDelegateBus rethinkDbDelegateBus
        ) : base(connectionFactory, rethinkDbDelegateBus) {}
        
        public IEnumerable<ChatMessage> GetAllChatMessagesByChatId(Guid chatId)
        {
            Cursor<ChatMessage> all = R.Db(DbName)
                .Table(TableName)
                .RunCursor<ChatMessage>(Connection);

            return all.OrderByDescending(m => m.Created)
                .Where(m => m.ChatId == chatId)
                .ToList();
        }

        public IEnumerable<ChannelMessage> GetAllChannelMessagesByChannelId(Guid channelId)
        {
            Cursor<ChannelMessage> all = R.Db(DbName)
                .Table(TableName)
                .RunCursor<ChannelMessage>(Connection);

            return all.OrderByDescending(m => m.Created)
                .Where(m => m.ChannelId == channelId)
                .ToList();
        }

        public Cursor<MessageChangeResult> GetChangeFeedCursorForUserId(Guid userId)
        {
            var channelUserTable = R.Db(DbName).Table(nameof(ChannelUser));
            var chatUserTable = R.Db(DbName).Table(nameof(ChatUser));
            
            Cursor<MessageChangeResult> infiniteCursor = R.Db(DbName)
                .Table(TableName)
                .Changes()
                .Filter(row => R.Or(
                        channelUserTable
                            .Filter(
                                channelUser => R.And(
                                    channelUser["ChannelId"].Eq(row["new_val"]["ChannelId"]),
                                    channelUser["UserId"].Eq(userId)
                                )
                            )
                            .Count().Gt(0), 
                        chatUserTable
                            .Filter(
                                chatUser => R.And(
                                    chatUser["ChatId"].Eq(row["new_val"]["ChatId"]),
                                    chatUser["UserId"].Eq(userId)
                                )
                            )
                            .Count().Gt(0)
                    )
                )
                .Filter(row => row["old_val"].Eq(null)) // get only new messages
                .RunCursor<MessageChangeResult>(Connection);
            
            return infiniteCursor;
        }
    }
}