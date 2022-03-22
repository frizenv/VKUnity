using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
	public class ExternalIncomingScriptCall : MonoBehaviour
	{
		public const string GAME_OBJECT_NAME = "ExternalIncomingScriptsCall";

		private static ExternalIncomingScriptCall _instance;

		private static ExternalIncomingScriptCall GetMono
		{
			get
			{
				if (ReferenceEquals(_instance, null))
				{
					var coroutineSource = new GameObject(GAME_OBJECT_NAME);
					_instance = coroutineSource.AddComponent<ExternalIncomingScriptCall>();
					DontDestroyOnLoad(_instance.gameObject);
				}

				return _instance;
			}
		}



		private abstract class CallbackItem : IDisposable
		{
			public abstract void Execute(string jsonString);
			public virtual void Dispose() { }
		}

		private class CallbackItem<T> : CallbackItem
		{
			public Action<T> Callback;

			public override void Execute(string jsonString)
			{
				Debug.Log($"ExternalIncomingScriptCall execute, raw: {jsonString}");
				Callback.Invoke(JsonUtility.FromJson<T>(jsonString));
			}

			public override void Dispose()
			{
				base.Dispose();

				Callback = null;
			}
		}

		private Dictionary<string, CallbackItem> _callbacks = new Dictionary<string, CallbackItem>();


		public static void Add<T>(string key, Action<T> callback)
		{
			if (!GetMono._callbacks.ContainsKey(key))
			{
				GetMono._callbacks.Add(key, new CallbackItem<T>()
				{
					Callback = callback
				});
			}
			else
			{
				throw new Exception($"Key {key} is Exist");
			}
		}

		public static bool Remove(string key)
		{

			if (_instance && _instance._callbacks.TryGetValue(key, out var item))
			{
				item.Dispose();

				return _instance._callbacks.Remove(key);
			}

			return false;
		}

		public static void RemoveAll()
		{
			if (_instance)
			{
				foreach (var pair in _instance._callbacks)
				{
					pair.Value.Dispose();
				}

				_instance._callbacks.Clear();
			}

		}


		[Serializable]
		private class ExtCallData
		{
			public string key;
			public string payload;//json
		}

		public void ExternalIncomingCall(string json)
		{
			var callData = JsonUtility.FromJson<ExtCallData>(json);

			if (_callbacks.TryGetValue(callData.key, out var item))
			{
				item.Execute(callData.payload);
			}
		}
	}
}