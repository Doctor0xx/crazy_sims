using StackExchange.Redis;

namespace SIMSAPI
{
    public class RedisDB
    {
        private readonly ConfigurationOptions config;

        public RedisDB()
        {
            config = new ConfigurationOptions
            {
                EndPoints = { "redisdb1-axk2jy.serverless.apne1.cache.amazonaws.com:6379" },
                Ssl = true,
                AbortOnConnectFail = false
            };
        }

        public void StoreToken(string username, string token)
        {
            try
            {
                using var redis = ConnectionMultiplexer.Connect(config);
                IDatabase db = redis.GetDatabase();
                db.StringSet(username, token);
            }
            catch
            {
                throw;
            }
        }

        public bool CheckToken(string username, string token)
        {
            try
            {
                using var redis = ConnectionMultiplexer.Connect(config);
                IDatabase db = redis.GetDatabase();
                return db.StringGet(username) == token;
            }
            catch
            {
                throw;
            }
        }
    }
}
