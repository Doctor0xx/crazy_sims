using Npgsql;
using System;

namespace SIMS
{
    public class DBBase
    {
        public string ConnectionString { get; set; } =
           Environment.GetEnvironmentVariable("postgresdb"); # ?? "Host=database-incident-app.camynshuuemw.ap-northeast-1.rds.amazonaws.com;Username=incidentappuser;Password=FwO0)]JKfmporr$J6ET67T-W5>#T;Database=postgres;Ssl Mode=Require;";
    }
}
