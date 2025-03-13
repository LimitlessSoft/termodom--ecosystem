using System.ComponentModel;
using System.Reflection;

namespace TD.Common.Environments;

public static class StringExtensions
{
	public static Environment ResolveDeployVariable(this string deployVariable)
	{
		foreach (var env in Enum.GetValues<Environment>())
		{
			var memberInfo = typeof(Environment).GetMember(env.ToString());
			var descriptionAttribute =
				memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false).First()
				as DescriptionAttribute;
			
			if(descriptionAttribute!.Description == deployVariable)
				return env;
		}
		throw new ArgumentException("Invalid environment");
	}
}
