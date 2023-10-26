using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.DTO.TDBrain_v3.Komercijalno
{
    public class PartnerGetDTO
    {
        public int ppid { get; set; }
        public string naziv { get; set; }
        public string adresa { get; set; }
        public string posta { get; set; }
        public string mesto { get; set; }
        public string telefon { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string kontakt { get; set; }
        public double popust { get; set; }
        public int vaziDana { get; set; }
        public int neplFakt { get; set; }
        public double duguje { get; set; }
        public double potrazuje { get; set; }
        public int? vpcid { get; set; }
        public double? procpc { get; set; }
        public long? kategorija { get; set; }
        public string mBroj { get; set; }
        public int? sdel { get; set; }
        public int? nppid { get; set; }
        public string mestoID { get; set; }
        public int? imaUgovor { get; set; }
        public int? imaIzjave { get; set; }
        public int? cenovnikID { get; set; }
        public int opstinaID { get; set; }
        public int drzavaID { get; set; }
        public int zapID { get; set; }
        public string valuta { get; set; }
        public string mobilni { get; set; }
        public float dozvoljeniMinus { get; set; }
        public string pib { get; set; }
        public int vrstaCenovnika { get; set; }
        public int refID { get; set; }
        public int drzavljanstvoID { get; set; }
        public int zanimanjeID { get; set; }
        public int weB_Status { get; set; }
        public int gppid { get; set; }
        public int cene_od_grupe { get; set; }
        public int pdvo { get; set; }
        public string nazivZaStampu { get; set; }
        public string katNaziv { get; set; }
        public int katID { get; set; }
        public int aktivan { get; set; }

        public TDOffice_v2.Komercijalno.Partner ToPartner()
        {
            TDOffice_v2.Komercijalno.Partner p = new TDOffice_v2.Komercijalno.Partner();
            p.PPID = this.ppid;
            p.Naziv = this.naziv;
            p.Adresa = this.adresa;
            p.Posta = this.posta;
            p.Mesto = this.mesto;
            p.Telefon = this.telefon;
            p.Fax = this.fax;
            p.Email = this.email;
            p.Kontakt = this.kontakt;
            p.MBroj = this.mBroj;
            p.MestoID = this.mestoID;
            p.OpstinaID = this.opstinaID;
            p.DrzavaID = this.drzavaID;
            p.ZapID = this.zapID;
            p.Valuta = this.valuta;
            p.Mobilni = this.mobilni;
            p.Kategorija = this.kategorija;
            p.DozvoljeniMinus = this.dozvoljeniMinus;
            p.NPPID = this.nppid;
            p.PIB = this.pib;
            p.ImaUgovor = this.imaUgovor;
            p.VrstaCenovnika = this.vrstaCenovnika;
            p.RefID = this.refID;
            p.DrzavljanstvoID = this.drzavljanstvoID;
            p.ZanimanjeID = this.zanimanjeID;
            p.WEB_Status = this.weB_Status;
            p.GPPID = this.gppid;
            p.Cene_od_grupe = this.cene_od_grupe;
            p.VPCID = this.vpcid;
            p.PROCPC = this.procpc;
            p.PDVO = this.pdvo;
            p.NazivZaStampu = this.nazivZaStampu;
            p.KatNaziv = this.katNaziv;
            p.KatID = this.katID;
            p.Aktivan = this.aktivan;
            p.Duguje = this.duguje;
            p.Potrazuje = this.potrazuje;
            return p;
        }
        public static List<TDOffice_v2.Komercijalno.Partner> ToPartnerList(List<PartnerGetDTO> source)
        {
            List<TDOffice_v2.Komercijalno.Partner> list = new List<TDOffice_v2.Komercijalno.Partner>();

            foreach (var dto in source)
                list.Add(dto.ToPartner());

            return list;
        }
    }
}
