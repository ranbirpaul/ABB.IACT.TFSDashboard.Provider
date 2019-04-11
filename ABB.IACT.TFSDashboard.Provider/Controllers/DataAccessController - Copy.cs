using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.IACT.TFSDashboard.Provider.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ABB.IACT.TFSDashboard.Provider.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Produces("application/json")]
    [Route("dcm/dataaccess1")]
    public class DataAccessVController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly IHostingEnvironment _hostingEnvironment;
        public DataAccessVController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public  IActionResult Post(string sourceid,string filter)
        {
            
                string outputString = JsonConvert.SerializeObject(GetRoomData());

                var jArr = JArray.Parse(outputString);
                jArr.Capitalize();

                return Ok(jArr);
            
            //else
            //    return Ok("");

        }

        private List<JObject> GetBcmData()
        {
            var rand = new Random();
            List<JObject> items = new List<JObject>();
            for (int i = 1; i <= 15; i++)
            {
                var item = new JObject();

                item.Add("Timestamp", DateTime.Now.AddMinutes(-(i*20)));
                item.Add("Power", rand.Next(220));
                items.Add(item);
            }
            return items;
        }

        private List<JObject> GetRoomData()
        {
            var rand = new Random();
            List<JObject> items = new List<JObject>();
            var item = new JObject();
            item.Add("Variable", "Total Power");
            item.Add("value", rand.Next(500));
            item.Add("Timestamp", "January 14,2019");
            item.Add("Status", "Good");
            item.Add("Unit", "");
            items.Add(item);
            return items;
        }
    }

   
}