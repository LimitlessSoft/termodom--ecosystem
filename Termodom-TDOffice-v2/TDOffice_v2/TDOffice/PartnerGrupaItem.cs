using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    /// <summary>
    /// Tabela za vodjenje evidencije kojim grupama pripada partner
    /// </summary>
    public class PartnerGrupaItem
    {
        private int PartnerID { get; set; }
        public int PartnerGrupaID { get; set; }

        /// <summary>
        /// Proverava da li partner ima dodeljenu grupu
        /// </summary>
        /// <param name="partnerID"></param>
        /// <param name="partnerGrupaID"></param>
        /// <returns></returns>
        public static bool Exists(int partnerID, int partnerGrupaID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Exists(con, partnerID, partnerGrupaID);
            }
        }
        /// <summary>
        /// Proverava da li partner ima dodeljenu grupu
        /// </summary>
        /// <param name="partnerID"></param>
        /// <param name="partnerGrupaID"></param>
        /// <returns></returns>
        public static bool Exists(FbConnection con, int partnerID, int partnerGrupaID)
        {
            using (FbCommand cmd = new FbCommand("SELECT COUNT(PARTNER_ID) FROM PARTNER_GRUPA_ITEM WHERE PARTNER_GRUPA_ID = @PGID AND PARTNER_ID = @PID", con))
            {
                cmd.Parameters.AddWithValue("@PID", partnerID);
                cmd.Parameters.AddWithValue("@PGID", partnerGrupaID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return Convert.ToInt32(dr[0]) > 0 ? true : false;
            }

            return false;
        }
        /// <summary>
        /// Vraca iteme grupa partnera za prosledjenog partnera
        /// </summary>
        /// <param name="partnerID"></param>
        /// <returns></returns>
        public static List<PartnerGrupaItem> ListByPartner(int partnerID)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return ListByPartner(con, partnerID);
            }
        }
        /// <summary>
        /// Vraca iteme grupa partnera za prosledjenog partnera
        /// </summary>
        /// <param name="partnerID"></param>
        /// <returns></returns>
        public static List<PartnerGrupaItem> ListByPartner(FbConnection con, int partnerID)
        {
            List<PartnerGrupaItem> pgi = new List<PartnerGrupaItem>();

            using(FbCommand cmd = new FbCommand("SELECT PARTNER_ID, PARTNER_GRUPA_ID FROM PARTNER_GRUPA_ITEM WHERE PARTNER_ID = @PID", con))
            {
                cmd.Parameters.AddWithValue("@PID", partnerID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        pgi.Add(new PartnerGrupaItem()
                        {
                            PartnerID = Convert.ToInt32(dr[0]),
                            PartnerGrupaID = Convert.ToInt32(dr[1])
                        });
            }

            return pgi;
        }
        /// <summary>
        /// Vraca iteme grupa partnera koji imaju prosledjenu grupu
        /// </summary>
        /// <param name="partnerGrupaID"></param>
        /// <returns></returns>
        public static List<PartnerGrupaItem> ListByGrupa(int partnerGrupaID)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return ListByGrupa(con, partnerGrupaID);
            }
        }
        /// <summary>
        /// Vraca iteme grupa partnera koji imaju prosledjenu grupu
        /// </summary>
        /// <param name="partnerGrupaID"></param>
        /// <returns></returns>
        public static List<PartnerGrupaItem> ListByGrupa(FbConnection con, int partnerGrupaID)
        {
            List<PartnerGrupaItem> pgi = new List<PartnerGrupaItem>();

            using (FbCommand cmd = new FbCommand("SELECT PARTNER_ID, PARTNER_GRUPA_ID FROM PARTNER_GRUPA_ITEM WHERE PARTNER_GRUPA_ID = @PID", con))
            {
                cmd.Parameters.AddWithValue("@PID", partnerGrupaID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        pgi.Add(new PartnerGrupaItem()
                        {
                            PartnerID = Convert.ToInt32(dr[0]),
                            PartnerGrupaID = Convert.ToInt32(dr[1])
                        });
            }

            return pgi;
        }
        /// <summary>
        /// Insertuje item grupe partnera. Ukoliko partner vec pripada grupi, nece biti ponovo insertovan!
        /// </summary>
        /// <param name="partnerID"></param>
        /// <param name="partnerGrupaID"></param>
        public static void Insert(int partnerID, int partnerGrupaID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Insert(con, partnerID, partnerGrupaID);
            }
        }
        /// <summary>
        /// Insertuje item grupe partnera. Ukoliko partner vec pripada grupi, nece biti ponovo insertovan!
        /// </summary>
        /// <param name="partnerID"></param>
        /// <param name="partnerGrupaID"></param>
        public static void Insert(FbConnection con, int partnerID, int partnerGrupaID)
        {
            if (Exists(con, partnerID, partnerGrupaID))
                return;

            using(FbCommand cmd = new FbCommand("INSERT INTO PARTNER_GRUPA_ITEM (PARTNER_ID, PARTNER_GRUPA_ID) VALUES (@PID, @PGID)", con))
            {
                cmd.Parameters.AddWithValue("@PID", partnerID);
                cmd.Parameters.AddWithValue("@PGID", partnerGrupaID);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Brise sve iteme grupa partnera za datog partnera
        /// </summary>
        /// <param name="partnerID"></param>
        public static void Delete(int partnerID)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Delete(con, partnerID);
            }
        }
        /// <summary>
        /// Brise sve iteme grupa partnera za datog partnera
        /// </summary>
        /// <param name="partnerID"></param>
        public static void Delete(FbConnection con, int partnerID)
        {
            using (FbCommand cmd = new FbCommand("DELETE FROM PARTNER_GRUPA_ITEM WHERE PARTNER_ID = @PID", con))
            {
                cmd.Parameters.AddWithValue("@PID", partnerID);

                cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Brise item grupe partnera
        /// </summary>
        /// <param name="partnerID"></param>
        /// <param name="partnerGrupaID"></param>
        public static void Delete(int partnerID, int partnerGrupaID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Delete(con, partnerID, partnerGrupaID);
            }
        }
        /// <summary>
        /// Brise item grupe partnera
        /// </summary>
        /// <param name="partnerID"></param>
        /// <param name="partnerGrupaID"></param>
        public static void Delete(FbConnection con, int partnerID, int partnerGrupaID)
        {
            using(FbCommand cmd = new FbCommand("DELETE FROM PARTNER_GRUPA_ITEM WHERE PARTNER_ID = @PID AND PARTNER_GRUPA_ID = @PGID", con))
            {
                cmd.Parameters.AddWithValue("@PID", partnerID);
                cmd.Parameters.AddWithValue("@PGID", partnerGrupaID);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
