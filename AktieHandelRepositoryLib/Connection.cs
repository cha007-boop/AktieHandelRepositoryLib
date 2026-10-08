using System;
using System.Collections.Generic;
using System.Text;

namespace AktieHandelRepositoryLib
{
    public static class Connection
    {
        private static string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=AktieHandelDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30";
        public static string ConnectionString
        {
            get { return _connectionString; }
        }
    }
}
