using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using System;
using System.Collections.Generic;

namespace TDOffice_v2.TDOffice
{
    public partial class Planer
    {
        public class Stavka
        {
            private static Buffer<List<Stavka>> _buffer = new Buffer<List<Stavka>>(List);

            public int ID { get; set; }
            public int UserID { get; set; }
            public DateTime Datum { get; set; }
            public string Body { get; set; }

            /// <summary>
            /// Azurira podatke korisnika u bazi. Osnov: ID.
            /// Kolone koje se azuriraju su: Body
            /// </summary>
            public void Update()
            {
                using (FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    Update(con);
                }
            }
            /// <summary>
            /// Azurira podatke korisnika u bazi. Osnov: ID.
            /// Kolone koje se azuriraju su: Body
            /// </summary>
            /// <param name="con"></param>
            public void Update(FbConnection con)
            {
                
                using (FbCommand cmd = new FbCommand(@"UPDATE PLANER_STAVKA SET
                                                        USERID = @USERID,
                                                        DATUM = @DATUM,
                                                        BODY = @BODY
                                                        WHERE ID = @SID", con))
                {
                    cmd.Parameters.AddWithValue("@USERID", UserID);
                    cmd.Parameters.AddWithValue("@DATUM", Datum);
                    cmd.Parameters.AddWithValue("@BODY", Body);
                    cmd.Parameters.AddWithValue("@SID", ID);

                    cmd.ExecuteNonQuery();
                }
                
                //throw new NotImplementedException();
            }

            /// <summary>
            /// Dodaje novu planer stavku unutar baze i vraca ID novokreirane stavke
            /// </summary>
            /// <param name="userID"></param>
            /// <param name="datum"></param>
            /// <param name="body"></param>
            /// <returns></returns>
            public static int Insert(int userID, DateTime datum, string body)
            {
                using (FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                   
                    return Insert(con, userID, datum, body);
                }
            }
            /// <summary>
            /// Dodaje novu planer stavku unutar baze i vraca ID novokreirane stavke
            /// </summary>
            /// <param name="con"></param>
            /// <param name="userID"></param>
            /// <param name="datum"></param>
            /// <param name="body"></param>
            /// <returns></returns>
            public static int Insert(FbConnection con, int userID, DateTime datum, string body)
            {
                using (FbCommand cmd = new FbCommand(@"INSERT INTO PLANER_STAVKA
                (ID, USERID, DATUM, BODY)
                VALUES (((SELECT COALESCE(MAX(ID), 0) FROM PLANER_STAVKA) + 1), @USERID, @DATUM, @BODY) RETURNING ID", con))
                {
                    cmd.Parameters.AddWithValue("@USERID", userID);
                    cmd.Parameters.AddWithValue("@DATUM", datum);
                    cmd.Parameters.AddWithValue("@BODY", body);
                    cmd.Parameters.Add(new FbParameter { ParameterName = "ID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });

                    cmd.ExecuteNonQuery();


                    return Convert.ToInt32(cmd.Parameters["ID"].Value);
                }
                //throw new NotImplementedException();
            }
            /// <summary>
            /// Vraca planer stavku iz baze. Ukoliko stavka nije pronadjena vraca null
            /// </summary>
            /// <param name="id"></param>
            /// <returns></returns>
            public static Stavka Get(int id)
            {
                using (FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return Get(con, id);
                }
            }
            /// Vraca planer stavku iz baze. Ukoliko stavka nije pronadjena vraca null
            /// </summary>
            /// <param name="id"></param>
            /// <param name="con">
            /// <returns></returns>
            public static Stavka Get(FbConnection con, int id)
            {
                using (FbCommand cmd = new FbCommand(@"SELECT
                                                        ID,
                                                        USERID,
                                                        DATUM,
                                                        BODY    
                                                        FROM PLANER_STAVKA
                                                        WHERE ID = @SID", con))
                {
                    cmd.Parameters.AddWithValue("@SID", id);
                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Stavka()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                UserID = Convert.ToInt32(dr["USERID"]),
                                Datum = Convert.ToDateTime(dr["DATUM"]),
                                Body = dr["BODY"] is DBNull ? null : dr["BODY"].ToString()

                            };
                }
                return null;
                //throw new NotImplementedException();
            }
            /// <summary>
            /// Vraca listu svih planer stavki iz baze
            /// </summary>
            /// <returns></returns>
            public static List<Stavka> List()
            {
                using (FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return List(con);
                }
            }
            /// <summary>
            /// Vraca listu svih planer stavki iz baze
            /// </summary>
            /// <returns></returns>
            public static List<Stavka> List(FbConnection con)
            {
                List<Stavka> list = new List<Stavka>();

                // ovde ide citanje iz baze
                using (FbCommand cmd = new FbCommand(@"SELECT
                                                        ID,
                                                        USERID,
                                                        DATUM,
                                                        BODY
                                                        FROM PLANER_STAVKA", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new Stavka()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                UserID = Convert.ToInt32(dr["USERID"]),
                                Datum = Convert.ToDateTime(dr["DATUM"]),
                                Body = dr["BODY"] is DBNull ? null : dr["BODY"].ToString()

                            });
                }
                //throw new NotImplementedException();

                _buffer.Set(list);
                return list;
            }
            /// <summary>
            /// Vraca listu planer stavki iz baze za odredjenog korisnika
            /// </summary>
            /// <returns></returns>
            public static List<Stavka> ListByUserID(int userID)
            {
                using(FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return ListByUserID(con, userID);
                }
            }
            /// <summary>
            /// Vraca listu planer stavki iz baze za odredjenog korisnika
            /// </summary>
            /// <returns></returns>
            public static List<Stavka> ListByUserID(FbConnection con, int userID)
            {
                List<Stavka> list = new List<Stavka>();
                using (FbCommand cmd = new FbCommand(@"SELECT
                                                        ID,
                                                        USERID,
                                                        DATUM,
                                                        BODY
                                                        FROM PLANER_STAVKA
                                                        WHERE USERID = @UID", con))
                {
                    cmd.Parameters.AddWithValue("@UID", userID);
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new Stavka()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                UserID = Convert.ToInt32(dr["USERID"]),
                                Datum = Convert.ToDateTime(dr["DATUM"]),
                                Body = dr["BODY"] is DBNull ? null : dr["BODY"].ToString()

                            });
                }
                return list;
                //throw new NotImplementedException();
            }
            /// <summary>
            /// Vraca listu svih planer stavki iz buffera
            /// </summary>
            /// <param name="maximumObselence"></param>
            /// <returns></returns>
            public static List<Stavka> BufferedList(TimeSpan maximumObselence)
            {
                return _buffer.Get(maximumObselence);
            }
            /// <summary>
            /// Uklanja planer stavku iz baze
            /// </summary>
            /// <param name="id"></param>
            public static void Remove(int id)
            {
                using (FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    Remove(con, id);
                }
            }
            /// <summary>
            /// Uklanja planer stavku iz baze
            /// </summary>
            /// <param name="con"></param>
            /// <param name="id"></param>
            public static void Remove(FbConnection con, int id)
            {
                using (FbCommand cmd = new FbCommand("DELETE FROM PLANER_STAVKA WHERE ID = @SID", con))
                {
                    cmd.Parameters.AddWithValue("@SID", id);

                    cmd.ExecuteNonQuery();
                    _buffer.UpdateAsync();
                }
                //throw new NotImplementedException();
            }
        }
    }
}
