using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Termodom.Models
{
	public class CenovnikBuffer
	{
		private static TimeSpan OUTDATED_TIMESPAN = TimeSpan.FromMinutes(10);
		private ConcurrentDictionary<int, CenovnikBufferItem> _items =
			new ConcurrentDictionary<int, CenovnikBufferItem>(); // KorisnikID, BufferItem

		public CenovnikBufferItem this[int korisnikID]
		{
			get
			{
				if (_items.ContainsKey(korisnikID))
				{
					CenovnikBufferItem c = _items[korisnikID];
					if (c != null)
						if (
							(DateTime.Now - c.Updated).TotalSeconds < OUTDATED_TIMESPAN.TotalSeconds
						)
							return _items[korisnikID];
				}

				return null;
			}
			set { _items[korisnikID] = value; }
		}
	}
}
