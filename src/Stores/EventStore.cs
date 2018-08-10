using System;
using System.Collections.Generic;
using System.Linq;
using CallGate.Data;
using CallGate.Documents;
using RethinkDb.Driver.Ast;
using RethinkDb.Driver.Net;

namespace CallGate.Stores
{
    public class EventStore : Store<Event>, IEventStore
    {
        private readonly Table _eventTable;
        private readonly Table _groupUserTable;
        private readonly Table _channelUserTable;
        private readonly Table _chatUserTable;

        public EventStore(
            IRethinkDbConnectionFactory connectionFactory,
            IRethinkDbDelegateBus rethinkDbDelegateBus
        ) : base(connectionFactory, rethinkDbDelegateBus)
        {
            _eventTable = R.Db(DbName).Table(TableName);
            _groupUserTable = R.Db(DbName).Table(nameof(GroupUser));
            _channelUserTable = R.Db(DbName).Table(nameof(ChannelUser));
            _chatUserTable = R.Db(DbName).Table(nameof(ChatUser));
        }
        
        public IEnumerable<Event> GetAllForUserId(Guid userId)
        {
            return _eventTable
                .OrderBy().OptArg("index", R.Desc("Created"))
                .Filter(@event => UserIdRelatedCriteria(userId, @event))
                .Limit(10)
                .RunCursor<Event>(Connection)
                .ToList();
        }
        
        public Cursor<EventChangeResult> GetChangeFeedCursorForUserId(Guid userId)
        {
            return _eventTable
                .Changes()
                .Filter(eventChange => UserIdRelatedCriteria(userId, eventChange["new_val"]))
                .Filter(IsNewEvent)
                .RunCursor<EventChangeResult>(Connection);
        }

        private And UserIdRelatedCriteria(Guid userId, ReqlExpr eventExpr)
        {
            return R.And(
                R.Or(
                    R.And(
                        R.Or(
                            GroupNotCreated(eventExpr),
                            UserNotRemovedFromGroup(userId, eventExpr)
                        ),
                        UserIsInsideGroup(userId, eventExpr)
                    ),
                    UserIsGroupCreator(userId, eventExpr),
                    UserRemovedFromGroup(userId, eventExpr)
                ),
                R.Or(
                    ChatNotAffected(eventExpr),
                    UserRemovedFromChat(userId, eventExpr),
                    UserIsInsideAffectedChat(userId, eventExpr)
                ),
                R.Or(
                    ChannelNotAffected(eventExpr),
                    UserRemovedFromChannel(userId, eventExpr),
                    UserMembersInChannelNotChanged(eventExpr),
                    UserIsInsideAffectedChannel(userId, eventExpr)
                )
            );
        }

        private static Ne GroupNotCreated(ReqlExpr eventExpr)
        {
            return eventExpr["Type"].Ne(EventType.GroupCreated);
        }
        
        private static And UserNotRemovedFromGroup(Guid userId, ReqlExpr eventExpr)
        {
            return R.And(
                UserAttached(userId, eventExpr),
                eventExpr["Type"].Ne(EventType.UserRemovedFromGroup)
            );
        }
        
        private Gt UserIsInsideGroup(Guid userId, ReqlExpr eventExpr)
        {
            return _groupUserTable
                .Filter(
                    groupUser => R.And(
                        groupUser["GroupId"].Eq(eventExpr["GroupId"]),
                        groupUser["UserId"].Eq(userId)
                    )
                )
                .Count()
                .Gt(0);
        }
        
        private static And UserIsGroupCreator(Guid userId, ReqlExpr eventExpr)
        {
            return R.And(
                eventExpr["Type"].Eq(EventType.GroupCreated),
                UserIsAuthor(userId, eventExpr)
            );
        }
        
        private static And UserRemovedFromGroup(Guid userId, ReqlExpr eventExpr)
        {
            return R.And(
                UserAttached(userId, eventExpr),
                eventExpr["Type"].Eq(EventType.UserRemovedFromGroup)
            );
        }
        
        private static Eq ChatNotAffected(ReqlExpr eventExpr)
        {
            return eventExpr["ChatId"].Eq(null);
        }
        
        private static And UserRemovedFromChat(Guid userId, ReqlExpr eventExpr)
        {
            return R.And(
                UserAttached(userId, eventExpr),
                eventExpr["Type"].Eq(EventType.UserRemovedFromChat)
            );
        }
        
        private Gt UserIsInsideAffectedChat(Guid userId, ReqlExpr eventExpr)
        {
            return _chatUserTable
                .Filter(
                    chatUser => R.And(
                        chatUser["ChatId"].Eq(eventExpr["ChatId"]),
                        chatUser["UserId"].Eq(userId)
                    )
                )
                .Count()
                .Gt(0);
        }
        
        private static Eq ChannelNotAffected(ReqlExpr eventExpr)
        {
            return eventExpr["ChannelId"].Eq(null);
        }
        
        private static And UserRemovedFromChannel(Guid userId, ReqlExpr eventExpr)
        {
            return R.And(
                UserAttached(userId, eventExpr),
                eventExpr["Type"].Eq(EventType.UserRemovedFromChannel)
            );
        }
        
        private static And UserMembersInChannelNotChanged(ReqlExpr eventExpr)
        {
            return R.And(
                eventExpr["Type"].Ne(EventType.UserAddedToChannel),
                eventExpr["Type"].Ne(EventType.UserRemovedFromChannel)
            );
        }
        
        private Gt UserIsInsideAffectedChannel(Guid userId, ReqlExpr eventExpr)
        {
            return _channelUserTable
                .Filter(
                    channelUser => R.And(
                        channelUser["ChannelId"].Eq(eventExpr["ChannelId"]),
                        channelUser["UserId"].Eq(userId)
                    )
                )
                .Count()
                .Gt(0);
        }
        
        private static Eq UserIsAuthor(Guid userId, ReqlExpr eventExpr)
        {
            return eventExpr["AuthorUserId"].Eq(userId);
        }

        private static Eq UserAttached(Guid userId, ReqlExpr eventExpr)
        {
            return eventExpr["UserId"].Eq(userId);
        }
        
        private static Eq IsNewEvent(ReqlExpr eventChangeExpr)
        {
            return eventChangeExpr["old_val"].Eq(null);
        }
    }
}