namespace TD.Komercijalno.Contracts.Requests.Promene;

public class PromenaGetMultipleRequest
{
    public string? KontoStartsWith { get; set; }
    public int[]? PPID { get; set; }
}
