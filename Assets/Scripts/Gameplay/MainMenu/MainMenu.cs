using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Gameplay.MainMenu
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button _authButton = default;
		[SerializeField] private Button _getDataButton = default;
		[SerializeField] private TextMeshProUGUI _answerText = default;

		private void Awake()
		{
			_authButton.onClick.AddListener(AuthButtonClickedEventHandler);
			_getDataButton.onClick.AddListener(GetDataButtonClickedEventHandler);
		}

		private void OnDestroy()
		{
			_authButton.onClick.RemoveListener(AuthButtonClickedEventHandler);
			_getDataButton.onClick.RemoveListener(GetDataButtonClickedEventHandler);
		}

		private void AuthButtonClickedEventHandler()
		{
			try
			{
				ExternalIncomingScriptCall.Add<string>(nameof(AuthCallback), AuthCallback);
				ExternalAppMethods.Auth(nameof(AuthCallback));
			}
			catch (Exception ex)
			{
				ExternalIncomingScriptCall.Remove(nameof(AuthCallback));
				Debug.LogError(ex.Message + "\n" + ex.StackTrace);
			}
		}

		private void GetDataButtonClickedEventHandler()
		{
			try
			{
				ExternalIncomingScriptCall.Add<string>(nameof(GetDataCallback), GetDataCallback);
				ExternalAppMethods.GetUserData(nameof(GetDataCallback));
			}
			catch (Exception ex)
			{
				ExternalIncomingScriptCall.Remove(nameof(GetDataCallback));
				Debug.LogError(ex.Message + "\n" + ex.StackTrace);
			}
		}

		private void AuthCallback(string data)
		{
			ExternalIncomingScriptCall.Remove(nameof(AuthCallback));
			_answerText.text = data;
		}

		private void GetDataCallback(string data)
		{
			ExternalIncomingScriptCall.Remove(nameof(GetDataCallback));
			_answerText.text = data;
		}
	}
}