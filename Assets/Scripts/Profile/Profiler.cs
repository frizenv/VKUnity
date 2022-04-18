using System;
using System.Collections;
using System.Text;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace Profile
{
	public class Profiler : MonoBehaviour
	{
		public static Profiler Instance { get; private set; }

		[SerializeField] private TextMeshProUGUI _stats = default;

		private StringBuilder _output = new StringBuilder(500);
		private ProfilerRecorder[] _recorders;
		private Coroutine _routine;

		private (string Name, Func<double, string> Format)[] _recorderNames = new (string, Func<double, string>)[]
		{
			("Total Reserved Memory", BytesToMB),
			("Total Used Memory", BytesToMB),
			("Texture Memory", BytesToMB),
			("Gfx Reserved Memory", BytesToMB),
			("Gfx Used Memory", BytesToMB),
			("GC Reserved Memory", BytesToMB),
			("GC Used Memory", BytesToMB),
			("GC Allocated In Frame", BytesToKB),


			("Texture Count", CountToString),
			("Asset Count", CountToString),
			("GC Allocation In Frame Count", CountToString),
		};

		#region Conversion functions

		private static string BytesToMB(double value)
		{
			long result = (long) (value / (1024 * 1024));
			return $"{result} MB";
		}
		private static string BytesToKB(double value)
		{
			long result = (long) (value / 1024);
			return $"{result} KB";
		}
		private static string CountToString(double value)
		{
			return ((long) value).ToString();
		}

		#endregion

		public string GetProfileProperty(string name)
		{
			int index = Array.FindIndex(_recorderNames, x => x.Name == name);
			if (index == -1)
				return null;
			return GetLastValueFormat(index);
		}

		private void Awake()
		{
			if (Instance == null)
				Instance = this;
			else
				Destroy(this);

			_recorders = new ProfilerRecorder[_recorderNames.Length];
			for (int i = 0; i < _recorderNames.Length; i++)
			{
				_recorders[i] = new ProfilerRecorder(ProfilerCategory.Memory, _recorderNames[i].Name);
			}
		}

		private IEnumerator CollectStatsCoroutine()
		{
			WaitForSeconds wait = new WaitForSeconds(0.1f);
			while (true)
			{
				_output.Clear();
				for (int i = 0; i < _recorders.Length; i++)
				{
					_output.AppendLine(GetLastValueFormat(i));
				}
				_stats.SetText(_output.ToString());
				yield return wait;
			}
		}

		private string GetLastValueFormat(int i)
		{
			double lastValue = _recorders[i].LastValueAsDouble;
			return $"{_recorderNames[i].Name}: {_recorderNames[i].Format(lastValue)}";
		}

		private void OnEnable()
		{
			foreach (ProfilerRecorder recorder in _recorders)
				recorder.Start();
			_routine = StartCoroutine(CollectStatsCoroutine());
		}
		private void OnDisable()
		{
			foreach (ProfilerRecorder recorder in _recorders)
				recorder.Stop();
			StopCoroutine(_routine);
			_routine = null;
		}
		private void OnDestroy()
		{
			foreach (ProfilerRecorder recorder in _recorders)
				recorder.Dispose();
		}

	}
}