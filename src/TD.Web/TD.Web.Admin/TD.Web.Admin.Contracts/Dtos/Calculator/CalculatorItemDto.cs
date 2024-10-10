using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Dtos.Calculator;

public class CalculatorItemDto
{
    public long Id { get; set; }
    public string ProductName { get; set; }
    public string Unit { get; set; }
    public CalculatorType CalculatorType { get; set; }
    public int Order { get; set; }
    public decimal Quantity { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsHobi { get; set; }
    public bool IsStandard { get; set; }
    public bool IsProfi { get; set; }
}
