using System;

namespace API.DTO.Magacin
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class StavkaUpdateDTO
    {
        public Int16 ID { get; set; }
        public Int16 VrDok { get; set; }
        public Int16 BrDok { get; set; }
        public double Kolicina { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
