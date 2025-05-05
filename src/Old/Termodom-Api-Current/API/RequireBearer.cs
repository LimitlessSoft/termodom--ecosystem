using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
	/// <summary>
	/// Indicates that endpoint require valid bearer token
	/// </summary>
	public class RequireBearer : Attribute { }
}
