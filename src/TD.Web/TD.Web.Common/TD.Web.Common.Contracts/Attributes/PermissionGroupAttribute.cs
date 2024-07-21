namespace TD.Web.Common.Contracts.Attributes;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
public class PermissionGroupAttribute (string name) : Attribute
{
    public string Name { get; } = name;
}
