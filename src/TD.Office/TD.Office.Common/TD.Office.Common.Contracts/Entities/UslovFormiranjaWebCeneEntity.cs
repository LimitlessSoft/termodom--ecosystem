using TD.Office.Common.Contracts.Enums;
using LSCore.Contracts.Entities;

namespace TD.Office.Common.Contracts.Entities
{
    public class UslovFormiranjaWebCeneEntity : LSCoreEntity
    {
        public int WebProductId { get; set; }
        public UslovFormiranjaWebCeneType Type { get; set; }
        public decimal Modifikator { get; set; }
    }
}
