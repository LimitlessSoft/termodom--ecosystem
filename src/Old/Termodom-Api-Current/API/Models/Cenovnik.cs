using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace API.Models
{
	public partial class Cenovnik : IList<Cenovnik.Artikal> // TODO: Izbaciti iz ovog namespace-a ili preimenovati u CenovnikModel
	{
		/// <summary>
		/// Procentualni iskaz koliko nama marze zasigurno istaje cak i u
		/// slucaju najjeftinije cene
		/// </summary>
		public static double OD_UKUPNE_RAZLIKE_NAMA_OSTAJE_SIGURNIH = 0.4;

		private List<Cenovnik.Artikal> _uslovi = new List<Cenovnik.Artikal>();

		public int KorisnikID { get; set; }

		public bool IsFixedSize => false;

		public bool IsReadOnly => false;

		public int Count => _uslovi.Count;

		public bool IsSynchronized => throw new NotImplementedException();

		public object SyncRoot => throw new NotImplementedException();

		public Artikal this[int identifier]
		{
			get => _uslovi.FirstOrDefault(t => t.ID == identifier);
			set => _uslovi[identifier] = value;
		}

		public Cenovnik() { }

		public void Clear()
		{
			_uslovi.Clear();
		}

		public bool Contains(Cenovnik.Artikal item)
		{
			return _uslovi.Contains(item);
		}

		public int IndexOf(Cenovnik.Artikal item)
		{
			return _uslovi.IndexOf(item);
		}

		public void Insert(int index, Cenovnik.Artikal item)
		{
			_uslovi.Insert(index, item);
		}

		public void Remove(Cenovnik.Artikal item)
		{
			_uslovi.Remove(item);
		}

		public void RemoveAt(int index)
		{
			_uslovi.RemoveAt(index);
		}

		public void CopyTo(Cenovnik.Artikal[] array, int index)
		{
			_uslovi.CopyTo(array, index);
		}

		public IEnumerator GetEnumerator()
		{
			return _uslovi.GetEnumerator();
		}

		public void Add(Cenovnik.Artikal item)
		{
			_uslovi.Add(item);
		}

		bool ICollection<Cenovnik.Artikal>.Remove(Cenovnik.Artikal item)
		{
			return _uslovi.Remove(item);
		}

		IEnumerator<Cenovnik.Artikal> IEnumerable<Cenovnik.Artikal>.GetEnumerator()
		{
			return _uslovi.GetEnumerator();
		}
	}
}
