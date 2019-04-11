using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABB.IACT.TFSDashboard.Provider.DA
{
    public class StructureProvider
    {
        public List<structureObject> locationStructure;

        static StructureProvider provider;
        public StructureProvider()
        {
            var json = System.IO.File.ReadAllText("data.json");
           locationStructure= JsonConvert.DeserializeObject<List<structureObject>>(json);

        }
        public static StructureProvider Getprovider()
        {
            if(provider==null)
            {
                provider = new StructureProvider();
            }
            return provider;
        }

        #region not used
        public List<structureObject> GenerateData()
        {
            List<structureObject> objects = new List<structureObject>();
            var singapore_R = new structureObject() { Name = "Singapore", Id = Guid.NewGuid().ToString(), Children = null, Type = "Location" };
            var singapore_S = new structureObject() { Name = "Singapore", Id =  Guid.NewGuid().ToString(), Children = null, Type = "Location" };
            var DC = new structureObject() { Name = "Data Ceneter 1", Id =  Guid.NewGuid().ToString(), Children = null, Type = "DC" };
            var Rack1 = new structureObject() { Name = "Rack 1", Id =  Guid.NewGuid().ToString(), Children = null, Type = "Rack" };
            var Rack2 = new structureObject() { Name = "Rack 2", Id =  Guid.NewGuid().ToString(), Children = null, Type = "Rack" };
            var Floor = new structureObject() { Name = "Floor 1", Id =  Guid.NewGuid().ToString(), Children = null, Type = "Location" };
            var bcm1 = new structureObject() { Name = "BCM 1", Id =  Guid.NewGuid().ToString(), Children = null, Type = "BCM" };
            var bcm2 = new structureObject() { Name = "BCM 2", Id =  Guid.NewGuid().ToString(), Children = null, Type = "BCM" };
            var bcm3 = new structureObject() { Name = "BCM 3", Id =  Guid.NewGuid().ToString(), Children = null, Type = "BCM" };
            var bcm4 = new structureObject() { Name = "BCM 4", Id =  Guid.NewGuid().ToString(), Children = null, Type = "BCM" };

            objects.Add(new structureObject()
            {
                Id =  Guid.NewGuid().ToString(),
                Name = "Regions",
                Type = "Location",
                Children = new List<structureObject>()
                {
                    singapore_R
                }
            });
            objects.Add(new structureObject()
            {
                Id = singapore_R.Id,
                Name = "Singapore",
                Type = "Location",
                Children = new List<structureObject>()
                {
                    singapore_S
                    
                }
            });
            objects.Add(new structureObject()
            {
                Id = singapore_S.Id,
                Name = "Singapore",
                Type = "Location",
                Children = new List<structureObject>()
                {
                    DC

                }
            });
            objects.Add(new structureObject()
            {
                Id = DC.Id,
                Name = "Data Center 1",
                Type = "DC",
                Children = new List<structureObject>()
                {
                    Floor

                }
            });
            objects.Add(new structureObject()
            {
                Id = Floor.Id,
                Name = "Floor 1",
                Type = "Location",
                Children = new List<structureObject>()
                {
                    Rack1,Rack2

                }
            });
            objects.Add(new structureObject()
            {
                Id = Rack1.Id,
                Name = "Rack 1",
                Type = "Location",
                Children = new List<structureObject>()
                {
                    bcm1,bcm2

                }
            });
            objects.Add(new structureObject()
            {
                Id = Rack2.Id,
                Name = "Rack 2",
                Type = "Location",
                Children = new List<structureObject>()
                {
                    bcm3,bcm4

                }
            });
            return objects;
        }
        #endregion

        public structureObject Get(string Id)
        {
           var structure= locationStructure.Where(x => x.Id == Id).FirstOrDefault();
            return structure;
        }

        public structureObject GetParent()
        {
            var structure = locationStructure.Where(x => x.Name == "Regions").FirstOrDefault();
            return structure;
        }

        public structureObject Execute(string query)
        {
            if(query=="Location")
            {
                return GetParent();
            }
            if(query=="xxxx/Location")
            {
                return new structureObject() { Children=new List<structureObject>() { Get("5f37378d-430f-48dd-a316-e2b884fe1ca3")},Id="",Name="",Type="" };
            }
            var keys = query.Split('/');
            var key = keys[keys.Length-1];
           return Get(key);
            
        }
    }

    public class structureObject
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public List<structureObject> Children { get; set; }
        public string Type { get; set; }
    }


}
