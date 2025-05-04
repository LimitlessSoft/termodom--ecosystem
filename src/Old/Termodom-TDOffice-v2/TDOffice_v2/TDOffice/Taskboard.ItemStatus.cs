using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
	public partial class Taskboard
	{
		public enum ItemStatus
		{
			Requested = 0,
			InQueue = 1,
			InProgress = 2,
			Finished = 3
		}
	}
}
