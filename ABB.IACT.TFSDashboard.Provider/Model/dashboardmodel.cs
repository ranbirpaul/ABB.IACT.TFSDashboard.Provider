using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABB.IACT.TFSDashboard.Provider.Model
{
	public class dashboardmodel
	{
		public string roomid { get; set; }
		public string roomname { get; set; }
		public string datacentername { get; set; }
		public double temperature { get; set; }
		public double humidity { get; set; }
	}
}
