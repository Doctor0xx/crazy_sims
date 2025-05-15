using Npgsql;
using System;

namespace SIMS
{
    public class DBBase
    {
        public string ConnectionString { get; set; } =
           Environment.GetEnvironmentVariable("postgresdb");
    }
}
