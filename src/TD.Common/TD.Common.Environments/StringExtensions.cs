using System.ComponentModel;
using System.Reflection;

namespace TD.Common.Environments;

public static class StringExtensions
{
	public static Environment ResolveDeployVariable(this string deployVariable) =>
		typeof(Environment)
			.GetMember(deployVariable)[0]
			.GetCustomAttribute<DescriptionAttribute>()
			?.Description switch
		{
			"develop" => Environment.Development,
			"stage" => Environment.Stage,
			"production" => Environment.Production,
			"automation" => Environment.Automation,
			_ => throw new ArgumentException("Invalid environment"),
		};
}
