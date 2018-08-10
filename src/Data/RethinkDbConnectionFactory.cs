using CallGate.Services;
using Microsoft.Extensions.Options;
using RethinkDb.Driver;
using RethinkDb.Driver.Net;

namespace CallGate.Data
{
    public class RethinkDbConnectionFactory : IRethinkDbConnectionFactory
    {
        private static readonly RethinkDB R = RethinkDB.R;
        private readonly RethinkDbOptions _options;
        private Connection _connection;

        public RethinkDbConnectionFactory(IOptions<RethinkDbOptions> options)
        {
            _options = options.Value;
        }

        public Connection CreateConnection()
        {
            if (_connection == null)
            {
                _connection = R.Connection()
                    .Hostname(_options.Host)
                    .Port(_options.Port)
                    .Timeout(_options.Timeout)
                    .Connect();
            }

            if(!_connection.Open)
            {
                _connection.Reconnect();
            }

            return _connection;
        }

        public void CloseConnection()
        {
            if (_connection != null && _connection.Open)
            {
                _connection.Close(false);
            }
        }

        public RethinkDbOptions GetOptions()
        {
            return _options;
        }
    }
}