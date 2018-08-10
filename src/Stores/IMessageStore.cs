using System;
using System.Collections.Generic;
using CallGate.DependencyInjection;
using CallGate.Documents;
using RethinkDb.Driver.Net;

namespace CallGate.Stores
{
    public interface IMessageStore : IStore<Message>, IScopedDependency
    {
        IEnumerable<ChatMessage> GetAllChatMessagesByChatId(Guid chatId);
        
        IEnumerable<ChannelMessage> GetAllChannelMessagesByChannelId(Guid channelId);

        Cursor<MessageChangeResult> GetChangeFeedCursorForUserId(Guid userId);
    }
}