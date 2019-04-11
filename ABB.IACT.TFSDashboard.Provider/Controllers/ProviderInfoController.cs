
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Abb.Mom.InfoModels.Common;
using Abb.Mom.InfoModels.Common.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NJsonSchema;

namespace ABB.IACT.TFSDashboard.Provider.Controllers
{
    /// <summary>
    /// To get Data provider info
    /// </summary>
    [EnableCors("AllowAllOrigins")]
    [Produces("application/json")]
    [Route("dcm/providerinfo")]
    [ApiController]
    public class ProviderInfoController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetProviderInfo()
        {
            try
            {
                var result = new List<ProviderInfo>();
                var providerItems = await ProviderInformation().ConfigureAwait(false);
                string outputString = JsonConvert.SerializeObject(providerItems);

                var jArr = JArray.Parse(outputString);
                return this.Ok(jArr);

            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// To get Provider name.
        /// </summary>
        /// <returns>List</returns>
        private static async Task<List<ProviderInfo>> ProviderInformation()
        {
            var entities = await Entities().ConfigureAwait(false);
            return new List<ProviderInfo>()
            {
                new ProviderInfo { ProviderType = "DCADashboard", Description="DCM Data Source", Entities = entities},
            };
        }

        /// <summary>
        /// To get list of Entities
        /// </summary>
        /// <returns>List</returns>
        private static async Task<List<EntityInfo>> Entities()
        {
            var buildSchemaModel = await ProviderInfoController.BuildSchemaModel().ConfigureAwait(false);
            var buildSchemaModel1 = await ProviderInfoController.BuildSchemaModel1().ConfigureAwait(false);
            return new List<EntityInfo>()
            {
                new EntityInfo {Entity = "Tables", Description = "DCM Variables Entity", Tags = {"DCM Data" }, EntitySchemas = buildSchemaModel, IsSearchable = true},
                new EntityInfo {Entity = "Projects", Description = "DCM Projects Entity", Tags = {"DCM Data" }, EntitySchemas = buildSchemaModel1, IsSearchable = true}

            };
        }

        /// <summary>
        /// To get Build Schema Model.
        /// </summary>
        /// <returns>list</returns>
        private static async Task<List<EntitySchema>> BuildSchemaModel()
        {
            var variableFilterDto = await SearchFilter().ConfigureAwait(false);
            JObject entityJObject = JObject.Parse(variableFilterDto);
            return new List<EntitySchema>()
            {
                new EntitySchema { SchemaType = SchemaTypeEnum.Info, Filters = new List<object> { entityJObject }, Response = "DataTable", EndPoint = "dataAccess" },
                new EntitySchema { SchemaType = SchemaTypeEnum.Data, Filters = new List<object> { entityJObject }, Response = "DataTable", EndPoint = "dataAccess" }
            };
        }
        private static async Task<List<EntitySchema>> BuildSchemaModel1()
        {
            var variableFilterDto = await SearchFilter().ConfigureAwait(false);
            JObject entityJObject = JObject.Parse(variableFilterDto);

            return new List<EntitySchema>()
            {
                new EntitySchema { SchemaType = SchemaTypeEnum.Info, Filters = new List<object> { entityJObject }, Response = "DataTable", EndPoint = "dataAccess1" },
                new EntitySchema { SchemaType = SchemaTypeEnum.Data, Filters = new List<object> { entityJObject }, Response = "DataTable", EndPoint = "dataAccess1" }
            };
        }


        /// <summary>
        /// To get table info
        /// </summary>
        /// <returns>TableInfo</returns>
        private static async Task<String> SearchFilter()
        {
            var schema = await JsonSchema4.FromTypeAsync<Model.SearchFilter>().ConfigureAwait(false);
            var schemaData = schema.ToJson();
            return schemaData;
        }
    }
}