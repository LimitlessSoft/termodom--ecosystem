using System.ComponentModel;

namespace TD.Web.Common.Contracts.Enums
{
    public enum OrderStatus
    {
        [Description("Otvorena")]
        Open = 0,
        [Description("Čeka obradu")]
        PendingReview = 1,
        [Description("Na obradi")]
        InReview = 2,
        [Description("Spremna za preuzimanje")]
        WaitingCollection = 3,
        [Description("Preuzeta")]
        Collected = 4,
        [Description("Otkazan")]
        Canceled = 5
    }
}
