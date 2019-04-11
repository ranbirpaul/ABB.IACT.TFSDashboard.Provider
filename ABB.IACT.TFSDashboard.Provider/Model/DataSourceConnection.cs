using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABB.IACT.TFSDashboard.Provider.Model
{
    public class DataSourceConnection
    {
        public string Id { get; set; }

        public string DataSourceName { get; set; }

        public string ServerName { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string DataSourceType { get; set; }

        public string Description { get; set; }
    }

    public class Result
    {
        public IEnumerable<DataSourceConnection> DataSourceConnections { get; set; }
    }
}
