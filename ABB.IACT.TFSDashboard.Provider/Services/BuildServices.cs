using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;
using ABB.IACT.TFSDashboard.Provider.Model;
using Microsoft.Azure.Documents;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace ABB.IACT.TFSDashboard.Provider
{
    /// <summary>
    /// Build Services Class
    /// </summary>
    public class BuildServices
    {
        

        /// <summary>
        /// Function to fetch Build completed count from Azure CosmosDB.
        /// </summary>
        /// <returns>integer</returns>
        //public async Task<int> GetBuildCounts(string project, string buildDefinition)
        //{
        //    using (client = new DocumentClient(new Uri(endpoint), authKey))
        //    {
        //        var link = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
        //        var query = client.CreateDocumentQuery<dynamic>(
        //            link,
        //            new SqlQuerySpec
        //            {
        //                QueryText = @"SELECT * FROM f WHERE f.data.eventType=""build.complete"" AND f.data.resource.project.name=@project AND f.data.resource.definition.name=@buildDefinition",
        //                Parameters = new SqlParameterCollection()
        //                {
        //                    new SqlParameter("@project",project),
        //                    new SqlParameter("@buildDefinition",buildDefinition)
        //                }
        //            },
        //            new FeedOptions()
        //            {
        //                EnableCrossPartitionQuery = true
        //            }).AsDocumentQuery();

        //        var results = new List<dynamic>();
        //        while (query.HasMoreResults)
        //        {
        //            results.AddRange(await query.ExecuteNextAsync<dynamic>());
        //        }
        //        return results.ToArray().Length;
        //    }
        //}

        /// <summary>
        /// Function to fetch async Build Results from Azure CosmosDB.
        /// </summary>
        /// <returns>dynamic</returns>
        //public async Task<dynamic> Results(string project)
        //{
        //    using (client = new DocumentClient(new Uri(endpoint), authKey))
        //    {
        //        var link = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
        //        var query = client.CreateDocumentQuery<dynamic>(
        //            link,
        //            new SqlQuerySpec
        //            {
        //                QueryText = @"SELECT f.data.resource.project,f.data.resource.definition.name, f.data.resource.buildNumber, f.data.resource.status, f.data.resource.result FROM root f WHERE f.data.eventType=""build.complete"" AND f.data.resource.project.name=@project",
        //                Parameters = new SqlParameterCollection()
        //                {
        //                    new SqlParameter("@project",project)
        //                }
        //            }).AsDocumentQuery();
                   
        //        var results = new List<dynamic>();
        //        while (query.HasMoreResults)
        //        {
        //            results.AddRange(await query.ExecuteNextAsync<dynamic>());
        //        }
        //        return results.ToArray();

        //    }
        //}



        
    }
}