using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public enum TDTask
    {
        NereseneKalkulacije = 1,
        AutomatskeNivelacije = 2,
        ProveraMarzeProdajnogDokumenta = 3,
        AutomatskoGenerisanjePopisa = 4,
        ProveraDaLiImaPraznihFakturaUPoslednjih15Dana = 5,
        PrometMagacina = 6,
        MinimalneZalihe = 7,
        PrekomerneZalihe = 8,
        NedovrsenaRazduzenja = 9,
        TimeKeeper = 10,
        ZaposleniUgovor = 11,
        KomercijaloParametri = 12
    }
}
