namespace Termodom.Data.Entities.Komercijalno
{
    public class Roba
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string KatBr { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string KatBrPro { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Naziv { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string JM { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TarifaID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Vrsta { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NazivZaStampu { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string GrupaID { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public int? Podgrupa { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProID { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public int? DOB_PPID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string TrPak { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? TrKol { get; set; }
        #endregion
    }
}
