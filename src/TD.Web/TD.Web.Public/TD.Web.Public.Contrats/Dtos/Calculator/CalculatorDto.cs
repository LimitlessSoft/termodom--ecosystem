namespace TD.Web.Public.Contracts.Dtos.Calculator;

public class CalculatorDto
{
    public decimal HobiValueWithVAT { get => StandardValueWithVAT * 0.89m; }
    public decimal StandardValueWithVAT { get; set; }
    public decimal ProfiValueWithVAT { get => StandardValueWithVAT * 1.1m; }
}
