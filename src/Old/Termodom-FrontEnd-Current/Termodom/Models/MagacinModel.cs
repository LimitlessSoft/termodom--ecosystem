using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Termodom.Models
{
    public class MagacinModel
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string Grad { get; set; }
        public string Adresa { get; set; }
        public string Telefon { get; set; }
        public string Koordinate { get; set; }
        public string Emali { get; set; }

        /// <summary>
        /// Vraca predefinisani dictionary iz koda
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, MagacinModel> PreDefined()
        {
            return new Dictionary<int, MagacinModel>()
            {
                { 12, new MagacinModel() { ID = 12, Naziv = "Smederevski Put - Beograd", Adresa = "Smederevski Put 14", Grad = "Beograd", Telefon = "0698801506", Koordinate = "lat: 44.77558935087874, lng: 20.548545715864016" } },
                { 13, new MagacinModel() { ID = 13, Naziv = "Zrenjaninski Put 84g - Beograd", Adresa = "Zrenjaninski Put 84g", Grad = "Beograd", Telefon = "0698801513", Koordinate = "lat: 44.8527085410664, lng: 20.48574956115255" } },
                {15, new MagacinModel() { ID=15, Naziv = "Pančevački Put 189 - Beograd", Adresa = "Pančevački Put 189", Grad = "Pančevo", Telefon = "0618801515", Koordinate = "lat: 44.87241963606168, lng: 20.604882508683012" } },
                {16, new MagacinModel() {ID = 16, Naziv = "Novosadski Put 10 - Novi Sad", Adresa = "Novosadski Put 10", Grad = "Novi Sad", Telefon = "0618801516", Koordinate = "lat: 45.249210806909204, lng: 19.75847661350905"} },
                {17, new MagacinModel() {ID = 17, Naziv = "Bulevar Cara Konstantina bb - Niš", Adresa = "Bulevar Cara Konstantina bb", Grad = "Niš", Telefon = "0618801517", Koordinate = "lat: 43.30580029781865, lng: 21.966495809741325"} },
                {18, new MagacinModel() {ID = 18, Naziv = "Kružni put bb - Čačak", Adresa = "Kružni put bb", Grad="Čačak", Telefon = "0618801518", Koordinate = "lat: 43.89298993166289, lng: 20.376776665728606"} },
                {19, new MagacinModel() {ID = 19, Naziv = "Petrovački put bb - Požarevac", Adresa = "Petrovački put bb", Grad = "Požarevac", Telefon = "0618801519", Koordinate = "lat: 44.58878241978912, lng: 21.222297591376552"} },
                {20, new MagacinModel() {ID = 20, Naziv = "Horgoški put 15a - Subotica", Adresa = "Horgoški put 15a", Grad = "Subotica", Telefon = "0618801520", Koordinate = "lat: 46.10453699546766, lng: 19.738768098161124"} },
                {21, new MagacinModel() {ID = 21, Naziv = "Ribarski Put bb - Jagodina", Adresa = "Ribarski Put bb", Grad="Jagodina", Telefon = "0618801521", Koordinate = "lat: 43.98798062563133, lng: 21.263195740438967" } },
                {22, new MagacinModel() {ID = 22, Naziv = "Obilauni put bb - Šabac", Adresa = "Obilauni put bb", Grad = "Šabac", Telefon = "0618801522", Koordinate = "lat: 44.76357675805132, lng: 19.647400821662497"} },
                {23, new MagacinModel() {ID = 23, Naziv = "Kolarski put 167 - Smederevo", Adresa = "Kolarski put 167", Grad = "Smederevo", Telefon = "0618801523", Koordinate = " lat: 44.63995788647128, lng: 20.913881266771384"} },
                {25, new MagacinModel() {ID = 25, Naziv = "Kraljevačkog Bataljona 156 - Kragujevac", Adresa = "Kraljevačkog Bataljona 156", Grad = "Kragujevac", Telefon = "0618801525", Koordinate = "lat: 43.992547108284356, lng: 20.863783140439114"} },
                {26, new MagacinModel() {ID = 26, Naziv = "Kružni put bb - Resnik", Adresa = "Kružni put bb", Grad = "Beograd", Telefon = "0618801526", Koordinate = "lat: 44.71893285361133, lng: 20.469270031904518"} },
                {27, new MagacinModel(){ID = 27, Naziv = "Višnjevačka 20 - Sremska Mitrovica", Adresa = "Višnjevačka 20", Grad = "Sremska Mitrovica", Telefon = "0618801527", Koordinate = "lat: 44.9735747281746, lng: 19.637101782792353"} },
                {28, new MagacinModel() { ID = 28, Naziv = "Batajnički Drum 6a - Beograd", Adresa = "Batajnički Drum 6a", Grad = "Beograd", Telefon = "0618801528", Koordinate = "lat: 44.867066659396755, lng: 20.362153341385802"} },
                {-5, new MagacinModel(){ID = -5, Naziv = "Dostava",Adresa = "", Grad = "", Telefon = "", Koordinate = ""} }
            };
            //    { 12, "Smederevski Put 14 - Beograd", "0698801506"},
            //{ 13, "Zrenjaninski Put 84g - Beograd", "0698801513"},
            //{ 15, "Pančevački Put 189 - Pančevo", "0618801515"},
            //{ 16, "Novosadski Put 10 - Novi Sad", "0618801516"},
            //{ 17, "Bulevar Cara Konstantina bb - Niš", "0618801517"},
            //{ 18, "Kružni put bb - Čačak", "0618801518"},
            //{ 19, "Petrovački put bb - Požarevac", "0618801519"},
            //{ 20, "Horgoški put 15a - Subotica", "0618801520"},
            //{ 21, "Ribarski Put bb - Jagodina", "0618801521"},
            //{ 22, "Obilauni put bb - Šabac", "0618801522"},
            //{ 23, "Kolarski put 167 - Smederevo", "0618801523"},
            //{ 25, "Kraljevačkog Bataljona 156 - Kragujevac", "0618801525"},
            //{ 26, "Kružni put bb - Resnik", "0618801526"},
            //{ 27, "Višnjevačka 20 - Sremska Mitrovica", "0618801527"},
            //{ 28, "Batajnički Drum 6a - Beograd", "0618801528"},
            //{
            //        -5, "Dostava", "")
        }
    }
}
