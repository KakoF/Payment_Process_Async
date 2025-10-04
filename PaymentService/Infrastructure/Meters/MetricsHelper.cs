using System.Diagnostics.Metrics;

namespace Infrastructure.Meters
{
	public class MetricsHelper : IDisposable
	{
		private readonly Meter _meter;
		private readonly Dictionary<string, Counter<int>> _counters = new();
		private readonly Dictionary<string, Histogram<double>> _histograms = new();
		private readonly ReaderWriterLockSlim _lock = new();

		public MetricsHelper(string meterName = "PaymentService.Custom.Metrics", string meterVersion = "1.0.0")
		{
			_meter = new Meter(meterName, meterVersion);
		}

		// Método genérico para Counter
		public void RecordCounter(string metricName, int value = 1, params KeyValuePair<string, object>[] tags)
		{
			try
			{
				_lock.EnterReadLock();

				if (!_counters.TryGetValue(metricName, out var counter))
				{
					_lock.ExitReadLock();
					_lock.EnterWriteLock();

					try
					{
						if (!_counters.TryGetValue(metricName, out counter))
						{
							counter = _meter.CreateCounter<int>(
								metricName,
								unit: "count",
								description: $"Contador para {metricName}");

							_counters[metricName] = counter;
						}
					}
					finally
					{
						_lock.ExitWriteLock();
						_lock.EnterReadLock();
					}
				}

				counter.Add(value, tags);
			}
			finally
			{
				_lock.ExitReadLock();
			}
		}

		// Método genérico para Counter com tags como dicionário
		public void RecordCounter(string metricName, int value, IDictionary<string, object> tags)
		{
			var tagArray = tags.Select(t => new KeyValuePair<string, object>(t.Key, t.Value)).ToArray();
			RecordCounter(metricName, value, tagArray);
		}

		// Método genérico para Histogram
		public void RecordHistogram(string metricName, double value, params KeyValuePair<string, object>[] tags)
		{
			try
			{
				_lock.EnterReadLock();

				if (!_histograms.TryGetValue(metricName, out var histogram))
				{
					_lock.ExitReadLock();
					_lock.EnterWriteLock();

					try
					{
						if (!_histograms.TryGetValue(metricName, out histogram))
						{
							histogram = _meter.CreateHistogram<double>(
								metricName,
								unit: "ms",
								description: $"Histograma para {metricName}");

							_histograms[metricName] = histogram;
						}
					}
					finally
					{
						_lock.ExitWriteLock();
						_lock.EnterReadLock();
					}
				}

				histogram.Record(value, tags);
			}
			finally
			{
				_lock.ExitReadLock();
			}
		}

		// Método genérico para Histogram com tags como dicionário
		public void RecordHistogram(string metricName, double value, IDictionary<string, object> tags)
		{
			var tagArray = tags.Select(t => new KeyValuePair<string, object>(t.Key, t.Value)).ToArray();
			RecordHistogram(metricName, value, tagArray);
		}

		public void Dispose()
		{
			_lock?.Dispose();
			_meter?.Dispose();
		}
	}
}

