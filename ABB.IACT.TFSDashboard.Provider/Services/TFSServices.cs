using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace ABB.IACT.TFSDashboard.Provider.Services
{
    public class TFSServices : ISearchServices
    {
        public static TFSServices GetTFSServices(IConfiguration configuration)
        {
            lock (sync)
            {
                return _tfsServices ?? (_tfsServices = new TFSServices(configuration));
            }
        }

        private TFSServices(IConfiguration configuration)
        {

        }
        public async Task<List<JObject>> ExecuteQuery(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return await GetTFSProjects().ConfigureAwait(false);
            }

            string[] queryParts = query.Split("/");
            switch (queryParts.Length)
            {
                case 1: return GetGroupofTFSProjects(queryParts[0]);
            }

            throw new NotSupportedException();

        }

        public async Task<dynamic> GetTFSProjects()
        {
            var projectsNameToObjectIdPair = new List<JObject>();
            using (client = new DocumentClient(new Uri(endpoint), authKey))
            {
                var link = UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId);
                var query = client.CreateDocumentQuery<dynamic>(
                    link,
                    new SqlQuerySpec
                    {
                        QueryText = @"SELECT DISTINCT f.data.resource.project FROM root f WHERE f.data.eventType=""build.complete"" OR f.data.eventType=""ms.vss-release.deployment-completed-event"""
                    }).AsDocumentQuery();

                var results = new List<dynamic>();
                while (query.HasMoreResults)
                {
                    results.AddRange(await query.ExecuteNextAsync<dynamic>());
                }

                var array = JArray.Parse(JsonConvert.SerializeObject(results));

                foreach (var item in array)
                {

                    if (item.HasValues)
                    {
                        dynamic Projects = new JObject();
                        var projectsName = item["project"]["name"].ToString();
                        var projectObjectId = item["project"]["id"].ToString();
                        Projects.Item = projectsName;
                        Projects.DisplayName = projectsName;
                        Projects.Id = projectObjectId;
                        Projects.IsFavorite = false;
                        Projects.ProviderName = "TFS";
                        Projects.Source = string.Empty;
                        Projects.IsDraggable = false;
                        Projects.IsClickable = true;
                        Projects.IsLeaf =false;
                        Projects.Type = string.Empty;
                        projectsNameToObjectIdPair.Add(Projects);
                    }

                }
                return projectsNameToObjectIdPair;
            }
        }

        public static List<JObject> GetGroupofTFSProjects(string id)
        {
            var builds = GetBuildGroup(id);
            var releases = GetReleaseGroup(id);
            return new List<JObject> { builds, releases };
        }

        private static JObject GetBuildGroup(string id)
        {
            dynamic variablesGroup = new JObject();
            variablesGroup.Item = BuildDefinitionName;
            variablesGroup.DisplayName = BuildDefinitionName;
            variablesGroup.Id = BuildDefinitionName;
            variablesGroup.IsFavorite = false;
            variablesGroup.ProviderName = ProviderName;
            variablesGroup.Source = id;
            variablesGroup.IsDraggable = false;
            variablesGroup.IsClickable = true;
            variablesGroup.IsLeaf = false;
            variablesGroup.Type = string.Empty;
            return variablesGroup;
        }

        private static JObject GetReleaseGroup(string id)
        {
            dynamic variablesGroup = new JObject();
            variablesGroup.Item = ReleaseDefinitionName;
            variablesGroup.DisplayName = ReleaseDefinitionName;
            variablesGroup.Id = ReleaseDefinitionName;
            variablesGroup.IsFavorite = false;
            variablesGroup.ProviderName = ProviderName;
            variablesGroup.Source = id;
            variablesGroup.IsDraggable = false;
            variablesGroup.IsClickable = true;
            variablesGroup.IsLeaf = false;
            variablesGroup.Type = string.Empty;
            return variablesGroup;
        }

        private static readonly string DatabaseId = "outDatabase";
        private static readonly string CollectionId = "MyCollection";
        private static readonly string endpoint = "https://abbcidashboard.documents.azure.com:443/";
        private static readonly string authKey = "m3j6FnImQO5mt2uTqtN0H4NEmnZAxgTMUA9PAMmX22VDRqNf8ltyvfJ0uSXNDFVXizH0jywOeDvXOY5x0Zb5lQ==";
        private static DocumentClient client;
        private static TFSServices _tfsServices;
        private static readonly object sync = new object();
        private const string ProviderName = "TFS";
        private const string BuildDefinitionName = "Build Definitions";
        private const string ReleaseDefinitionName = "Release Definitions";
    }
}
