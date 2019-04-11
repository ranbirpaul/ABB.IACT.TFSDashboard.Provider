using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ABB.IACT.TFSDashboard.Provider.Model;
using Microsoft.AspNetCore.Cors;

namespace ABB.IACT.TFSDashboard.Provider.Controllers
{
    [Route("dcm/connections")]
    [EnableCors("AllowAllOrigins")]
    public class ConnectionsController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetConnections()
        {
            await Task.Delay(1).ConfigureAwait(false);

            List<DataSourceConnection> dataConnections = new List<DataSourceConnection>();

            var dataSourceConnection = new DataSourceConnection()
            {
                Id = "ede8bde0-42ba-4c5e-bf6a-3255cbf06840",
                DataSourceName = "DCM",
                ServerName = string.Empty,
                UserName = string.Empty,
                Password = string.Empty,
                DataSourceType = "DCMData",
                Description = string.Empty
            };

            dataConnections.Add(dataSourceConnection);

            return Ok(dataConnections);

        }
    }
}