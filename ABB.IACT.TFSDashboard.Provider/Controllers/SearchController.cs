using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ABB.IACT.TFSDashboard.Provider.Model;
using ABB.IACT.TFSDashboard.Provider.Services;
using ABB.IACT.TFSDashboard.Provider.DA;

namespace ABB.IACT.TFSDashboard.Provider.Controllers
{
    [EnableCors("AllowAllOrigins")]
    [Produces("application/json")]
    [Route("dcm/search")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private BuildServices buildServices = new BuildServices();
        //private ReleaseServices releaseServices = new ReleaseServices();
        //private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;

        public SearchController(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            //_hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuery([FromQuery] Model.SearchFilter searchFilter)
        {
            return await GetQuery(searchFilter.Entity ?? "LocationStructure", searchFilter.Query).ConfigureAwait(false);
        }

        [HttpGet("entities")]
        public Task<IActionResult> GetEntities()
        {
            var entities = "[{\"Entity\": \"Tables\",\"Description\": \"Location structure\",\"Tags\": [\"Location Data\"]}]";
            var parsedEntities = JArray.Parse(entities);
            return Task.FromResult(Ok(parsedEntities) as IActionResult);
        }

        [HttpGet("schema")]
        public Task<IActionResult> GetSchema()
        {
            var schema = "[{\"Entity\": \"Tables\",\"Schema\": {\"name\": \"string\"},\"EndPoints\": {\"execution\": \"dataAccess\"}}]";

            var parsedSchema = JArray.Parse(schema);
            return Task.FromResult(Ok(parsedSchema) as IActionResult);
        }

        private async Task<IActionResult> GetQuery1(string entity="Projects", string query = null)
        {
            var tfsDataProvider = TFSServices.GetTFSServices(_configuration);
            var result = await tfsDataProvider.ExecuteQuery(query).ConfigureAwait(false);
            CustomDataProvider customDataProvider = new CustomDataProvider
            {
                Entity = entity,
                Truncated = false,
                SearchItems = result
            };

            string outputString = JsonConvert.SerializeObject(customDataProvider);
            var jArr = JObject.Parse(outputString);
            jArr.Capitalize();
            return Ok(jArr);
        }

        private  List<JObject> GenerateJson(structureObject obj)
        {
            List<JObject> objects = new List<JObject>();
            dynamic DS = new JObject();
            DS.DataSource = "History?sourceid=a1007019-8209-45b1-9aea-ec7d72f3a33e/Build&variable=QSU.integration.Nightly";
            foreach (var item in obj.Children)
            {
                dynamic Projects = new JObject();
                Projects.Item = item.Name;
                Projects.DisplayName = item.Name;
                Projects.Id = item.Id;
                Projects.IsFavorite = false;
                Projects.ProviderName = "DCM";
                Projects.Source = item.Id + "/" + item.Type;
                Projects.IsClickable = true;
                // Projects.History = "History?sourceid=a1007019-8209-45b1-9aea-ec7d72f3a33e/Build&variable=QSU.integration.Nightly";
                Projects.History = DS;
                if (item.Type == "BCM")
                {
                    Projects.Type = "GRID";
                    Projects.IsLeaf = true;
                    Projects.IsDraggable = true;
                }
                else if(item.Name=="Rack 2")
                {
                    Projects.Type = "GRID";
                    Projects.IsLeaf = true;
                    Projects.IsDraggable = true;
                }
                else
                {
                    Projects.Type = string.Empty;
                    Projects.IsLeaf = false;
                    Projects.IsDraggable = false;

                }

                objects.Add(Projects);
            }

            return objects;
        }

        private async Task<IActionResult> GetQuery(string entity = "Tables", string query = null)
        {
            List<JObject> objects;
            if(query==null)
            {
                dynamic Projects = new JObject();
                Projects.Item = "Location Structure";
                Projects.DisplayName = "Location Structure";
                Projects.Id = "Location";
                Projects.IsFavorite = false;
                Projects.ProviderName = "DCM";
                Projects.Source = "xxxx"; ;
                Projects.IsDraggable = false;
                Projects.IsClickable = true;
                Projects.IsLeaf = false;
                Projects.Type = string.Empty;
                objects = new List<JObject>();
                objects.Add(Projects);

				dynamic p2 = new JObject();
                p2.Item = "Room";
                p2.DisplayName = "Room";
                p2.Id = "Room";
                p2.IsFavorite = false;
                p2.ProviderName = "DCM";
                p2.Source = "Room"; ;
                p2.IsDraggable = true;
                p2.IsClickable = true;
                p2.IsLeaf = true;
                p2.Type ="GRID";
                dynamic DS = new JObject();
                DS.DataSource = "History?sourceid=room";
                p2.History = DS;
                if (entity !="Projects")
                   objects.Add(p2);

				dynamic p3 = new JObject();
				p3.Item = "Room Temperature";
				p3.DisplayName = "Room Temperature";
				p3.Id = "RoomTemperature";
				p3.IsFavorite = false;
				p3.ProviderName = "DCM";
				p3.Source = "RoomTemperature";
				p3.IsDraggable = true;
				p3.IsClickable = true;
				p3.IsLeaf = true;
				p3.Type = "GRID";
				dynamic DS1 = new JObject();
				DS1.DataSource = "History?sourceid=RoomTemperature";
				p3.History = DS1;
				if (entity != "Projects")
				  objects.Add(p3);
			}
            else if (query.Split('/').Last()=="Variables")
            {
                dynamic p2 = new JObject();
                p2.Item = "Total Power";
                p2.DisplayName = "Total Power";
                p2.Id = "Total Power";
                p2.IsFavorite = false;
                p2.ProviderName = "Properties";
                p2.Source = "Total Power";
                p2.IsDraggable = true;
                p2.IsClickable = true;
                p2.IsLeaf = true;
                p2.Type = "LIST_VALUE";
                objects = new List<JObject>();
                dynamic DS = new JObject();
                DS.DataSource = "History?sourceid=room";
                p2.History = DS;
                objects.Add(p2);
            }
            else
            {
              objects= GenerateJson( StructureProvider.Getprovider().Execute(query));
                //if(entity=="Variables" && objects[0].Type.ToString()=="Rack")
                //{
                    
                dynamic p2 = new JObject();
                p2.Item = "Variables";
                p2.DisplayName = "Variables";
                p2.Id = "Variables";
                p2.IsFavorite = false;
                p2.ProviderName = "Variables";
                p2.Source = "Variables"; ;
                p2.IsDraggable = false;
                p2.IsClickable = true;
                p2.IsLeaf = false;
                p2.Type = "";
                //}
                if(entity!="Tables")
                objects.Add(p2);
            }
            

            CustomDataProvider customDataProvider = new CustomDataProvider
            {
                Entity = entity,
                Truncated = false,
                SearchItems = objects
            };

            string outputString = JsonConvert.SerializeObject(customDataProvider);

            var jArr = JObject.Parse(outputString);
            jArr.Capitalize();

            return Ok(jArr);
        }


        //[HttpGet("build/results/{project}")]
        //public async Task<String> GetResults(string project)
        //{
        //    return JsonConvert.SerializeObject(await this.buildServices.Results(project));
        //}

        /// <summary>
        /// Get number of Builds Completed. The project name and build definition name are case sensitive.
        /// </summary>
        /// <returns>int</returns>
        //[HttpGet("build/count/{project}/{buildDefinition}")]
        //public async Task<int> GetCount(string project, string buildDefinition)
        //{
        //    return (await this.buildServices.GetBuildCounts(project, buildDefinition));
        //}

        ///// <summary>
        ///// Get release results.
        ///// </summary>
        ///// <returns>dynamic</returns>
        //[HttpGet("release/results/{project}")]
        //public async Task<dynamic> GetReleaseResults(string project)
        //{
        //    return JsonConvert.SerializeObject(await this.releaseServices.GetReleaseResults(project));
        //}

    }
}