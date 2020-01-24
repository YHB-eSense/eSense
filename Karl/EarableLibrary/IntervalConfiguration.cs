using Plugin.BLE.Abstractions.Contracts;
using System;

namespace EarableLibrary
{
	public class IntervalConfiguration
	{
		private static readonly byte CMD_INTERVALS = 0x57;


		private ICharacteristic _char;
		public readonly Interval<short> AdvertisementInterval;
		public readonly Interval<short> ConnectionInterval;

		public IntervalConfiguration(ICharacteristic characteristic)
		{
			_char = characteristic;
			AdvertisementInterval = new Interval<short>(min: 100, max: 5000);
			ConnectionInterval = new Interval<short>(min: 20, max: 2000);
		}

		public void Update()
		{
			var data = new byte[8];
			var msg = new ESenseMessage(header: CMD_INTERVALS, data: data);
			_char.WriteAsync(msg);
		}
	}

	public class Interval<T> where T : IComparable
	{
		private T _a, _b, _min, _max;

		public T A
		{
			get => _a;
			set
			{
				if (Valid(value, _b)) _a = value;
			}
		}

		public T B
		{
			get => _b;
			set
			{
				Validate(_a, value);
				_b = value;
			}
		}

		public Interval(T min, T max)
		{
			_a = _min = min;
			_b = _max = max;
		}

		private void Validate(T a, T b)
		{
			if (!Valid(a, b))
			{
				throw new ArgumentException("Interval constraints violated!");
			}
		}

		protected bool Valid(T a, T b)
		{
			if (_max != null && a.CompareTo(_max) > 0) return false;
			if (b.CompareTo(a) > 0) return false;
			if (_min != null && _min.CompareTo(b) > 0) return false;
			return true;
		}

	}
}
