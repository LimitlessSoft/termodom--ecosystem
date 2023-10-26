using System;

namespace Termodom.Models
{
    public class CenovnikBufferItem
    {
        public Korisnik.Cenovnik Cenovnik { get; set; }
        /// <summary>
        /// Vreme kada je ovaj item u bufferu ucitan
        /// </summary>
        public DateTime Updated { get; private set; } = DateTime.Now;
    }
}
