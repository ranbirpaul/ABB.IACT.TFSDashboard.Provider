using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ABB.IACT.TFSDashboard.Provider.Model
{
    public class SearchFilter
    {
        [DataMember(Name = "ProviderName", Order = 1)]
        public string ProviderName { get; set; }

        [DataMember(Name =  "Entity", Order = 2)]
        public string Entity { get; set; }

        [DataMember(Name = "filters", Order = 3)]
        public List<String> Filters { get; set; }

        [DataMember(Name = "ShowOnlyFavourites", Order = 4)]
        public bool ShowOnlyFavorites { get; set; }

        [DataMember(Name = "Query", Order = 5)]
        public string Query { get; set; }
    }




}
