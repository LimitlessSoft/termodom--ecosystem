using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class UsersAutomatskoObavestenje
	{
		public int ID { get; set; }
		public int UserID { get; set; }
		public int ObavestenjeID { get; set; }
		public bool Prima { get; set; }

		//        /// <summary>
		//        /// Azurira ili dodaje UserAutomatskoObavestenje u bazi.
		//        /// </summary>
		//        public void Update(bool raw = false)
		//        {
		//            using (FbConnection con = new FbConnection(TDOffice.connectionString))
		//            {
		//                con.Open();
		//                Update(con, raw);
		//            }
		//        }
		//        /// <summary>
		//        /// Azurira ili dodaje UserAutomatskoObavestenje u bazi.
		//        /// </summary>
		//        public void Update(FbConnection con, bool raw = false)
		//        {
		//            using (FbCommand cmd = new FbCommand(@"UPDATE USERS_AUTOMATSKO_OBAVESTENJE
		//(ID, USER_ID, OBAVESTENJE_ID, PRIMA) VALUES (((SELECT MAX()) + 1), @USERID, @OBAVESTENJEID, @PRIMA) MATCHING(ID)", con))
		//            {
		//                cmd.Parameters.AddWithValue("@ID", ID);
		//                byte[] tagBytes = raw ? Encoding.UTF8.GetBytes(Tag.ToString()) : Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Tag));
		//                cmd.Parameters.AddWithValue("@TAG", tagBytes);

		//                cmd.ExecuteNonQuery();
		//            }
		//        }
	}
}
