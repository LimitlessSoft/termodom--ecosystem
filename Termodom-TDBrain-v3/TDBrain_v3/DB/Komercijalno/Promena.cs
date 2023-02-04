using FirebirdSql.Data.FirebirdClient;
using System.Collections;

namespace TDBrain_v3.DB.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class Promena
    {
        /// <summary>
        /// 
        /// </summary>
        public class PromenaCollection : IEnumerable<Promena>
        {
            private Dictionary<int, Promena> _dict = new Dictionary<int, Promena>();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="promenaID"></param>
            /// <returns></returns>
            public Promena this[int promenaID] => _dict[promenaID];
            /// <summary>
            /// 
            /// </summary>
            /// <param name="dict"></param>
            public PromenaCollection(Dictionary<int, Promena> dict) => _dict = dict;

            /// <summary>
            /// Returns new dictionary containing data from collection
            /// </summary>
            /// <returns></returns>
            public Dictionary<int, Promena> ToDictionary()
            {
                return new Dictionary<int, Promena>(_dict);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IEnumerator<Promena> GetEnumerator() => _dict.Values.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public int PromenaID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int VrstaNal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string BrNaloga { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public DateTime DatNal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Konto { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        public string? Opis { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? PPID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? BrDok { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? VrDok { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DatDPO { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? Duguje { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? Potrazuje { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? DatVal { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? VDuguje { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double? VPotrazuje { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Valuta { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Kurs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? MTID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? A { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? StavkaID { get; set; }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="godina"></param>
        /// <param name="whereParameters"></param>
        /// <returns></returns>
        public static PromenaCollection Collection(int godina, List<string>? whereParameters = null)
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[Settings.MainMagacinKomercijalno, godina]))
            {
                con.Open();

                return Collection(con, whereParameters);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="whereParameters"></param>
        /// <returns></returns>
        public static PromenaCollection Collection(FbConnection con, List<string>? whereParameters = null)
        {
            string whereQuery = "";
            if(whereParameters != null)
                whereQuery = " WHERE " + string.Join(" AND ", whereParameters);

            Dictionary<int, Promena> dict = new Dictionary<int, Promena>();
            using(FbCommand cmd = new FbCommand("SELECT * FROM PROMENE" + whereQuery, con))
            {
                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        dict.Add(Convert.ToInt32(dr["PROMENAID"]), new Promena()
                        {
                            PromenaID = Convert.ToInt32(dr["PROMENAID"]),
                            VrstaNal = Convert.ToInt32(dr["VRSTANAL"]),
                            BrNaloga = dr["BRNALOGA"].ToString(),
                            DatNal = Convert.ToDateTime(dr["DATNAL"]),
                            Konto = dr["KONTO"].ToString(),
                            Opis = dr["OPIS"] is DBNull ? null : dr["OPIS"].ToString(),
                            PPID = dr["PPID"] is DBNull ? null : Convert.ToInt32(dr["PPID"]),
                            BrDok = dr["BRDOK"] is DBNull ? null : Convert.ToInt32(dr["BRDOK"]),
                            VrDok = dr["VRDOK"] is DBNull ? null : Convert.ToInt32(dr["VRDOK"]),
                            DatDPO = dr["DATDPO"] is DBNull ? null : Convert.ToDateTime(dr["DATDPO"]),
                            Duguje = dr["DUGUJE"] is DBNull ? null : Convert.ToDouble(dr["DUGUJE"]),
                            Potrazuje = dr["POTRAZUJE"] is DBNull ? null : Convert.ToDouble(dr["POTRAZUJE"]),
                            DatVal = dr["DATVAL"] is DBNull ? null : Convert.ToDateTime(dr["DATVAL"]),
                            VDuguje = dr["VDUGUJE"] is DBNull ? null : Convert.ToDouble(dr["VDUGUJE"]),
                            VPotrazuje = dr["VPOTRAZUJE"] is DBNull ? null : Convert.ToDouble(dr["VPOTRAZUJE"]),
                            Valuta = dr["VALUTA"].ToStringOrDefault(),
                            Kurs = Convert.ToDouble(dr["KURS"]),
                            MTID = dr["MTID"].ToStringOrDefault(),
                            A = dr["A"] is DBNull ? null : Convert.ToInt32(dr["A"]),
                            StavkaID = dr["STAVKAID"] is DBNull ? null : Convert.ToInt32(dr["STAVKAID"])
                        });
                    }
                }
            }

            return new PromenaCollection(dict);
        }
    }
}
