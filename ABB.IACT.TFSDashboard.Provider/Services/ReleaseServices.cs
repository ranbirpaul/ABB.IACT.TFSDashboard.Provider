using System;
using System.Collections.Generic;
using ABB.IACT.TFSDashboard.Provider.Model;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using System.Linq;
using Newtonsoft.Json;

namespace ABB.IACT.TFSDashboard.Provider
{
    /// <summary>
    /// TFS Release Functions.
    /// </summary>
    public class ReleaseServices
    {
        private static readonly string DatabaseId = "outDatabase";
        private static readonly string CollectionId = "MyCollection";
        private static readonly string endpoint = "https://abbcidashboard.documents.azure.com:443/";
        private static readonly string authKey = "m3j6FnImQO5mt2uTqtN0H4NEmnZAxgTMUA9PAMmX22VDRqNf8ltyvfJ0uSXNDFVXizH0jywOeDvXOY5x0Zb5lQ==";
        private static DocumentClient client;

        /// <summary>
        /// Function to fetch release results from Cosmosdb.
        /// </summary>
        /// <returns>dynamic</returns>
        public async Task<dynamic> GetReleaseResults(string project)
        {
            using (client = new DocumentClient(new Uri(endpoint), authKey))
            {
                var link = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
                var query = client.CreateDocumentQuery<dynamic>(
                    link,
                    new SqlQuerySpec
                    {
                        QueryText = @"SELECT * FROM root f WHERE f.data.eventType=""ms.vss-release.deployment-completed-event"" AND f.data.resource.project.name = @project",
                        Parameters = new SqlParameterCollection()
                        {
                            new SqlParameter("@project",project)
                        }
                    }).AsDocumentQuery();
                var results = new List<dynamic>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<dynamic>());
                }
                var ReleaseResults = from result in results
                                     select new
                                     {
                                         BuildNumber = result.data.resource.deployment.release.artifacts[0].definitionReference.version.name,
                                         ReleaseDefinition = result.data.resource.environment.releaseDefinition.name,
                                         ReleaseEnvironment = result.data.resource.environment.name,
                                         ReleaseStatus = result.data.resource.environment.status
                                     };
                return ReleaseResults;

            }
        }

    }
}