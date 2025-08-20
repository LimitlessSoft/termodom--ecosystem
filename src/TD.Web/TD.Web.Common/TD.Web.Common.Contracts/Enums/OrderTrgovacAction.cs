namespace TD.Web.Common.Contracts.Enums;

public enum OrderTrgovacAction {
    None,
    ClientContactedNoResponse,
    ClientContactedWillCall,
    Confirmed,
    Delivered,
    Cancelled,
    ForwardToLocalTrgovac,
    ProfakturaSent
}