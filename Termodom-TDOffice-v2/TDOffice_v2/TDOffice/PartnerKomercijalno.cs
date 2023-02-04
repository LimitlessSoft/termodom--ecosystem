using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class PartnerKomercijalno
    {
        public class SpecijalniCenovnikDTO
        {
            public List<int> SpecijalniCenovnikList { get; set; }
            public List<int> NaciniUplateZaKojiVazeSpecijalniCenovnici { get; set; }
            public int OtherTip { get; set; } = 2;
            public double OtherModifikator { get; set; } = 0;
        }
        public int PPID { get; set; }
        public SpecijalniCenovnikDTO SpecijalniCenovnikPars
        {
            get
            {
                if(_specijalniCenovnikParsBuffer == null)
                {
                    _specijalniCenovnikParsBuffer = JsonConvert.DeserializeObject<SpecijalniCenovnikDTO>(_specijalniCenovnikPars);
                }
                return _specijalniCenovnikParsBuffer;
            }
            set
            {
                _specijalniCenovnikParsBuffer = value;
            }
        }
        private SpecijalniCenovnikDTO _specijalniCenovnikParsBuffer { get; set; } = null;
        private string _specijalniCenovnikPars { get; set; }

        public void Update()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand("UPDATE PARTNER_KOMERCIJALNO SET SPECIJALNI_CENOVNIK_PARS = @SCP WHERE PPID = @PPID", con))
            {
                cmd.Parameters.AddWithValue("@SCP", JsonConvert.SerializeObject(SpecijalniCenovnikPars));
                cmd.Parameters.AddWithValue("@PPID", PPID);

                cmd.ExecuteNonQuery();
            }
        }

        public static void Insert(int ppid, SpecijalniCenovnikDTO specijalniCenovnikParameters)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Insert(con, ppid, specijalniCenovnikParameters);
            }
        }
        public static void Insert(FbConnection con, int ppid, SpecijalniCenovnikDTO specijalniCenovnikParameters)
        {
            using (FbCommand cmd = new FbCommand("INSERT INTO PARTNER_KOMERCIJALNO (PPID, SPECIJALNI_CENOVNIK_PARS) VALUES (@PPID, @SCP)", con))
            {
                cmd.Parameters.AddWithValue("@SCP", JsonConvert.SerializeObject(specijalniCenovnikParameters));
                cmd.Parameters.AddWithValue("@PPID", ppid);

                cmd.ExecuteNonQuery();
            }
        }

        public static PartnerKomercijalno Get(int ppid)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, ppid);
            }
        }
        public static PartnerKomercijalno Get(FbConnection con, int ppid)
        {
            using(FbCommand cmd = new FbCommand("SELECT PPID, SPECIJALNI_CENOVNIK_PARS FROM PARTNER_KOMERCIJALNO WHERE PPID = @PPID", con))
            {
                cmd.Parameters.AddWithValue("@PPID", ppid);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new PartnerKomercijalno()
                        {
                            PPID = Convert.ToInt32(dr["PPID"]),
                            _specijalniCenovnikPars = dr["SPECIJALNI_CENOVNIK_PARS"].ToStringOrDefault()
                        };
            }
            return null;
        }

        public static List<PartnerKomercijalno> List()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<PartnerKomercijalno> List(FbConnection con)
        {
            List<PartnerKomercijalno> list = new List<PartnerKomercijalno>();
            using (FbCommand cmd = new FbCommand("SELECT PPID, SPECIJALNI_CENOVNIK_PARS FROM PARTNER_KOMERCIJALNO", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new PartnerKomercijalno()
                        {
                            PPID = Convert.ToInt32(dr["PPID"]),
                            _specijalniCenovnikPars = dr["SPECIJALNI_CENOVNIK_PARS"].ToStringOrDefault()
                        });
            }
            return list;
        }

    }
}
