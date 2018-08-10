using CallGate.Documents;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

namespace CallGate.Data
{
    public class RethinkDbManager : IRethinkDbManager
    {
        private static readonly RethinkDB R = RethinkDB.R;
        private readonly Connection _connection;
        private readonly string _dbName;

        public RethinkDbManager(IRethinkDbConnectionFactory connectionFactory)
        {
            _connection = connectionFactory.CreateConnection();
            _connection.CheckOpen();
            
            _dbName = connectionFactory.GetOptions().Database;
        }
    
        public void EnsureDatabaseCreated()
        {
            // database
            CreateDb();
    
            // tables
            CreateTable(nameof(Message));
            CreateTable(nameof(GroupUser));
            CreateTable(nameof(ChatUser));
            CreateTable(nameof(ChannelUser));
            CreateTable(nameof(Event));
    
            // indexes
            CreateIndex(nameof(Message), nameof(Message.ChatId));
            CreateIndex(nameof(Message), nameof(Message.ChannelId));
            CreateIndex(nameof(Message), nameof(Message.UserId));
            CreateIndex(nameof(Message), nameof(Message.Username));
            CreateIndex(nameof(GroupUser), nameof(GroupUser.UserId));
            CreateIndex(nameof(GroupUser), nameof(GroupUser.GroupId));
            CreateIndex(nameof(ChatUser), nameof(ChatUser.UserId));
            CreateIndex(nameof(ChatUser), nameof(ChatUser.ChatId));
            CreateIndex(nameof(ChannelUser), nameof(ChannelUser.UserId));
            CreateIndex(nameof(ChannelUser), nameof(ChannelUser.ChannelId));
            CreateIndex(nameof(Event), nameof(Event.AuthorUserId));
            CreateIndex(nameof(Event), nameof(Event.GroupId));
            CreateIndex(nameof(Event), nameof(Event.ChannelId));
            CreateIndex(nameof(Event), nameof(Event.ChatId));
            CreateIndex(nameof(Event), nameof(Event.UserId));
            CreateIndex(nameof(Event), nameof(Event.Created));
        }

        private void CreateDb()
        {
            var exists = R.DbList().Contains(db => db == _dbName).Run(_connection);
    
            if (!exists)
            {
                R.DbCreate(_dbName).Run(_connection);
                R.Db(_dbName).Wait_().Run(_connection);
            }
        }

        private void CreateTable(string tableName)
        {
            var exists = R.Db(_dbName).TableList().Contains(t => t == tableName).Run(_connection);
            
            if (!exists)
            {
                R.Db(_dbName).TableCreate(tableName).Run(_connection);
                R.Db(_dbName).Table(tableName).Wait_().Run(_connection);
            }
        }

        private void CreateIndex(string tableName, string indexName)
        {
            var exists =  R.Db(_dbName).Table(tableName).IndexList().Contains(t => t == indexName).Run(_connection);
            
            if (!exists)
            {
                R.Db(_dbName).Table(tableName).IndexCreate(indexName).Run(_connection);
                R.Db(_dbName).Table(tableName).IndexWait(indexName).Run(_connection);
            }
        }
    
        public void Reconfigure(int shards, int replicas)
        {
            var tables = R.Db(_dbName).TableList().Run(_connection);
            
            foreach (string table in tables)
            {
                R.Db(_dbName).Table(table).Reconfigure().OptArg("shards", shards).OptArg("replicas", replicas).Run(_connection);
                R.Db(_dbName).Table(table).Wait_().Run(_connection);
            }
        }
    }
}