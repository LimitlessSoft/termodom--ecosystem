using System.ComponentModel;

namespace TD.Common.Environments;

public enum Environment
{
	[Description("develop")]
	Development,

	[Description("stage")]
	Stage,

	[Description("production")]
	Production,

	[Description("automation")]
	Automation
}
