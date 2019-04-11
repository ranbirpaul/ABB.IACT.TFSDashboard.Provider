using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    [Route("dcm/dataaccess")]
    public class DataAccessController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _hostingEnvironment;
        public DataAccessController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public  IActionResult Post([FromBody] Rootobject filter)
        {
			if (filter.Item == "Room Temperature")
			{
				Task<List<JObject>> taskroom = GetRoomTemperatureData();
				taskroom.Wait();
				var jsonoutputString = taskroom.Result;
				string outputString = JsonConvert.SerializeObject(jsonoutputString);
				var jArr = JArray.Parse(outputString);
				jArr.Capitalize();
				return Ok(jArr);
			}

			if (filter.Item == "Room")
            {
                string outputString = JsonConvert.SerializeObject(GetRoomData());
                var jArr = JArray.Parse(outputString);
                jArr.Capitalize();
                return Ok(jArr);
            }
            else if (filter.Item== "Rack 2")
            {
                string outputString = JsonConvert.SerializeObject(GetRackData());

                var jArr = JArray.Parse(outputString);
                jArr.Capitalize();
                return Ok(jArr);
            }
            else //if (filter.Item.Contains("BCM"))
            {
                string outputString = JsonConvert.SerializeObject(GetBcmData());
                var jArr = JArray.Parse(outputString);
                jArr.Capitalize();
                return Ok(jArr);
            }
            //else
            //    return Ok("");
        }

		private async Task<List<JObject>> GetRoomTemperatureData()
		{
			try
			{
				List<JObject> items = new List<JObject>();
				// Call API to get the data
				// Get Room Temperature and Humidity
				List<dashboardmodel> roomresult = new List<dashboardmodel>();
				// Step 1 Get All Rooms and Data Center from https://dcaapi3.azurewebsites.net/api/v1/Query/SendQuery
				using (var client = new HttpClient())
				{
						client.BaseAddress = new Uri("https://dcaapi3.azurewebsites.net/api/v1/Query/");
						// HTTP POST
						string data = ".dcaType('Room')";
						var responseTask = await client.PostAsJsonAsync("SendQuery", data);
						if (responseTask.IsSuccessStatusCode)
						{
							var roomdata = await responseTask.Content.ReadAsAsync<dynamic>();
							int i = 1;
							foreach (var dt in roomdata)
							{
								if (i == 3) // Contains room data
								{
								    var rand = new Random();
								    var rdatacollection = dt.Value;
									// Convert roomdata value to dashboardmodel
									foreach (var dcroomdata in rdatacollection)
									{
										dashboardmodel objdashboardmodel = new dashboardmodel();
										objdashboardmodel.roomid = dcroomdata.properties.RoomId.value;
										objdashboardmodel.roomname = dcroomdata.name;
										objdashboardmodel.datacentername = dcroomdata.properties.DataCenterName.value;
									    int simulatedtemperaure = rand.Next(22,25);
									    objdashboardmodel.temperature = simulatedtemperaure; // Get some random temperature
										int simulatedhumidity = rand.Next(60, 65);
										objdashboardmodel.humidity = simulatedhumidity; // Get some random humidity

										// Convert to JSON Object
										string jsonSTRINGResult = JsonConvert.SerializeObject(objdashboardmodel);
									    JObject jObjItem = JsonConvert.DeserializeObject<JObject>(jsonSTRINGResult);
									    items.Add(jObjItem);
									} //foreach (var dcroomdata in rdatacollection)
								} // if (i == 3) // Contains room data

								i++;
							} // foreach (var dt in roomdata)
						}
				} // using (var client = new HttpClient())

				return items;
			}
			catch (Exception ex)
			{
				throw ex;
			}
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


        private List<JObject> GetRackData()
        {
            var rand = new Random();
            List<JObject> items = new List<JObject>();
            for (int i = 1; i <= 10; i++)
            {
                var date = DateTime.Now.AddMinutes(i).ToString("MM-dd-yyyy HH:mm:ss");
               for(int j=1;j<4;j++)
                {
                    var obj = new JObject();
                    obj.Add("TimeStamp", date);
                    obj.Add("BCM", "BCM"+j);
                    obj.Add("Power", rand.Next(220,225));
                    items.Add(obj);
                }
            }
            return items;
        }

        private List<JObject> GetRoomData()
        {
            var rand = new Random();
            List<JObject> items = new List<JObject>();
            for(int i=1;i<=31;i++)
            {
                var item = new JObject();
                item.Add("Date", "Dec " + i);
                for (int j=1;j<=10;j++)
                {
                    item.Add("Item" + j, rand.Next(50));

                }
                items.Add(item);
            }
            return items;
        }
    }

    public class Rootobject
    {
        public string ProviderName { get; set; }
        public string Source { get; set; }
        public string Item { get; set; }
        //public string Entity { get; set; }
        public string[] Columns { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}