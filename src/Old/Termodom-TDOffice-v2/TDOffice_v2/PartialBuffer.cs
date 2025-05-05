using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public class PartialBuffer<T>
	{
		public delegate List<T> List(DateTime odDatuma, DateTime doDatuma);
		public delegate DateTime? MinDatum();

		#region Properties
		public PartialBufferState State
		{
			get
			{
				if (_ucitavanjePodataka == null)
					return PartialBufferState.Initializing;

				if (_ucitavanjePodataka.IsCompleted)
					return PartialBufferState.Finished;

				return PartialBufferState.Running;
			}
		}
		private int _periodUcitavanja { get; set; } = 10;
		private List<T> _list { get; set; } = new List<T>();
		private DateTime? _najranijeUcitanDatum { get; set; } = null;
		private List _funkcijaZaUcitavanjePodataka { get; set; }
		private MinDatum _funkcijaZaUcitavanjeMinimalnogDatumaIzTabele { get; set; }
		private Task _ucitavanjePodataka { get; set; }
		public DateTime? NajranijeUcitanDatum
		{
			get { return _najranijeUcitanDatum; }
		}
		private bool _loaded { get; set; } = false;
		public EventHandler Load { get; set; }
		#endregion

		public PartialBuffer(
			List funkcijaZaUcitavanjePoDatumu,
			MinDatum funkcijaZaUcitavanjeMinimalnogDatumaIzTabele,
			int periodUcitavanja
		)
		{
			_funkcijaZaUcitavanjePodataka = funkcijaZaUcitavanjePoDatumu;
			_funkcijaZaUcitavanjeMinimalnogDatumaIzTabele =
				funkcijaZaUcitavanjeMinimalnogDatumaIzTabele;
			_periodUcitavanja = periodUcitavanja;
			StartLoadingDataAsync();
		}

		private void StartLoadingDataAsync()
		{
			_ucitavanjePodataka = Task.Run(() =>
			{
				_loaded = false;
				DateTime? minDatum = _funkcijaZaUcitavanjeMinimalnogDatumaIzTabele();
				if (minDatum == null)
				{
					_loaded = true;
					return;
				}

				DateTime datum = DateTime.Now.AddDays(1);
				DateTime datum1 = DateTime.Now.AddDays(1).AddDays(++_periodUcitavanja * -1);
				while (datum >= minDatum)
				{
					List<T> list = _funkcijaZaUcitavanjePodataka(datum1, datum);
					_list.AddRange(list);
					_najranijeUcitanDatum = datum1;

					datum = datum.AddDays(_periodUcitavanja * -1);
					datum1 = datum1.AddDays(_periodUcitavanja * -1);
				}
				if (Load != null)
					Load(null, null);
				_loaded = true;
			});
		}

		public List<T> Data(DateTime najranijeUcitanDatum)
		{
			while (
				!_loaded
				&& (
					_najranijeUcitanDatum == null
					|| ((DateTime)_najranijeUcitanDatum).Date >= najranijeUcitanDatum.Date
				)
			) { }
			return new List<T>(_list);
		}

		public void Append(T obj)
		{
			_list.Add(obj);
		}
	}

	public enum PartialBufferState
	{
		Initializing = 0,
		Running = 1,
		Finished = 2
	}
}
