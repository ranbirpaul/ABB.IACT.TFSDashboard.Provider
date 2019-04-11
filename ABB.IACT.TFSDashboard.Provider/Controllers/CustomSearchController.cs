using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ABB.IACT.TFSDashboard.Provider.DA;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ABB.IACT.TFSDashboard.Provider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomSearchController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            StructureProvider provider = new StructureProvider();

            var obj = provider.GenerateData();
            System.IO.File.WriteAllText("data.json", JsonConvert.SerializeObject(obj)) ;

            return Ok(new[] { "" });
        }
    }
}