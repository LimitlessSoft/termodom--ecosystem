using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Termodom.Models
{
    public enum PorudzbinaStatus
    {
        Null = -1,
        NaObradi = 0,
        CekaUplatu = 1,
        ZaPreuzimanje = 2,
        Preuzeto = 3,
        Stornirana = 4
    }
}
