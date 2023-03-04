namespace TDBrain_v3.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public class PathToDatabaseNotFoundException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public int MagacinID { get; private set; }
        /// <summary>
        /// /
        /// </summary>
        public int Godina { get; private set; }
        /// <summary>
        /// Inicijalizuje novi objekat PathToMainDatabaseNotFoundExcpetion sa podacima o
        /// magacinu i godini zatrazene baze
        /// </summary>
        /// <param name="zatrazeniMagacinID"></param>
        /// <param name="godina"></param>
        public PathToDatabaseNotFoundException(int zatrazeniMagacinID, int godina) : base($"Putanja ka bazi magacina {zatrazeniMagacinID} za godinu {godina} nije pronadjena!")
        {
            this.MagacinID = zatrazeniMagacinID;
            this.Godina = godina;
        }
    }
}
