using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ABB.IACT.TFSDashboard.Provider.Model
{
    public class CustomDataProvider
    {
        [DataMember(Name = "Entity", Order = 1)]
        public string Entity { get; set; }

        [DataMember(Name = "Truncated", Order = 2)]
        public bool Truncated { get; set; }

        [DataMember(Name = "SearchItems", Order = 3)]
        public List<JObject> SearchItems { get; set; }
    }
}
