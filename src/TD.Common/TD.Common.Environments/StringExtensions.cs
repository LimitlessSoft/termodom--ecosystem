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
			return descriptionAttribute!.Description switch
			{
				"production" => Environment.Production,
				"stage" => Environment.Stage,
				"develop" => Environment.Development,
				"automation" => Environment.Automation,
				_ => throw new ArgumentException("Invalid environment")
			};
		}
		throw new ArgumentException("Invalid environment");
	}
}
