using System;
using System.Collections;
using System.Collections.Generic;

namespace Termodom.API
{
	public class APIRequestFailedLog : IList<APIRequestFailedLogItem>
	{
		private List<APIRequestFailedLogItem> _items = new List<APIRequestFailedLogItem>();

		public APIRequestFailedLogItem this[int index]
		{
			get { return _items[index]; }
			set { _items[index] = value; }
		}

		public int Count => _items.Count;
		public bool IsReadOnly => false;

		public void Add(APIRequestFailedLogItem item)
		{
			_items.Add(item);
		}

		public void Clear()
		{
			_items.Clear();
		}

		public bool Contains(APIRequestFailedLogItem item)
		{
			return _items.Contains(item);
		}

		public void CopyTo(APIRequestFailedLogItem[] array, int arrayIndex)
		{
			_items.CopyTo(array, arrayIndex);
		}

		public IEnumerator<APIRequestFailedLogItem> GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		public int IndexOf(APIRequestFailedLogItem item)
		{
			return _items.IndexOf(item);
		}

		public void Insert(int index, APIRequestFailedLogItem item)
		{
			_items.Insert(index, item);
		}

		public bool Remove(APIRequestFailedLogItem item)
		{
			return _items.Remove(item);
		}

		public void RemoveAt(int index)
		{
			_items.RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _items.GetEnumerator();
		}
	}
}
