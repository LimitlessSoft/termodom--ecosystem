using System;

namespace Termodom.Models
{
    /// <summary>
    /// Predstavlja klasu pojedinacne cene
    /// </summary>
    public class Cena // TODO: Izbaciti iz Models ili preimenovati u CenaModel
    {
        private double _nabavnaCena;
        private double _vpCena;
        private double _pdv;

        /// <summary>
        /// Predstavlja nabavnu cenu (bez PDV-a)
        /// </summary>
        public double NabavnaCena { get => _nabavnaCena; set => _nabavnaCena = value; }
        /// <summary>
        /// Cena bez uracunatog PDV-a
        /// </summary>
        public double VPCena { get => _vpCena; set => _vpCena = value; }
        /// <summary>
        /// PDV stopa
        /// </summary>
        public double PDV
        {
            get
            {
                return _pdv;
            }
            set
            {
                if(value > 1 || value < 0)
                    throw new Exception("Vrednos PDV-a mora biti u opsegu 0 - 1");

                _pdv = value;
            }
        }
        /// <summary>
        /// Cena sa uracunatim PDV-om
        /// </summary>
        public double MPCena
        {
            get
            {
                return VPCena * (1 + PDV);
            }
        }

        public Cena()
        {

        }
    }
}
