/*using Npgsql;
using System;

namespace SIMS
{
    public class DBBase
    {
        public string ConnectionString { get; set; } =
           Environment.GetEnvironmentVariable("postgresdb");

    }
}*/
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.Runtime;
using Amazon.Extensions.NETCore.Setup;
using Npgsql;

namespace SIMS
{
    public class DBBase
    {
        public string ConnectionString { get; private set; }

        public DBBase()
        {
            // Call async method and wait synchronously (or make this constructor async if you're in a modern async context)
            ConnectionString = GetConnectionStringFromSecretsManager().GetAwaiter().GetResult();
        }

        private async Task<string> GetConnectionStringFromSecretsManager()
        {
            var dbSecretName = Environment.GetEnvironmentVariable("postgresdb")
                ?? throw new InvalidOperationException("postgresdb environment variable is not set.");

            var client = new AmazonSecretsManagerClient();

            var response = await client.GetSecretValueAsync(new GetSecretValueRequest
            {
                SecretId = dbSecretName
            });

            if (string.IsNullOrWhiteSpace(response.SecretString))
            {
                throw new InvalidOperationException("Secret value is empty.");
            }

            // Parse the secret string JSON
            var json = JsonDocument.Parse(response.SecretString);
            var username = json.RootElement.GetProperty("username").GetString();
            var password = json.RootElement.GetProperty("password").GetString();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException("Username or password not found in secret.");
            }

            return $"Host=database-incident-app.camynshuuemw.ap-northeast-1.rds.amazonaws.com;Username={username};Password={password};Database=postgres;Ssl Mode=Require";
        }
    }
}
