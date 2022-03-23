using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;
using VKMessages;

namespace Gameplay.MainMenu
{
	public class MainMenu : MonoBehaviour
	{
		[SerializeField] private Button _getUserDataButton = default;
		[SerializeField] private Button _getEmailButton = default;
		[SerializeField] private TextMeshProUGUI _answerText = default;

		private void Awake()
		{
			_getUserDataButton.onClick.AddListener(GetUserDataButtonClickedEventHandler);
			_getEmailButton.onClick.AddListener(GetEmailButtonClickedEventHandler);
		}

		private void OnDestroy()
		{
			_getUserDataButton.onClick.RemoveListener(GetUserDataButtonClickedEventHandler);
			_getEmailButton.onClick.RemoveListener(GetEmailButtonClickedEventHandler);
		}

		private void GetUserDataButtonClickedEventHandler()
		{
			try
			{
				ExternalIncomingScriptCall.Add<GetUserDataMessage>(nameof(GetUserDataCallback), GetUserDataCallback);
				ExternalAppMethods.GetUserData(nameof(GetUserDataCallback));
			}
			catch (Exception ex)
			{
				ExternalIncomingScriptCall.Remove(nameof(GetUserDataCallback));
				Debug.LogError(ex.Message + "\n" + ex.StackTrace);
			}
		}

		private void GetEmailButtonClickedEventHandler()
		{
			try
			{
				ExternalIncomingScriptCall.Add<GetEmailMessage>(nameof(GetEmailCallback), GetEmailCallback);
				ExternalAppMethods.GetEmail(nameof(GetEmailCallback));
			}
			catch (Exception ex)
			{
				ExternalIncomingScriptCall.Remove(nameof(GetEmailCallback));
				Debug.LogError(ex.Message + "\n" + ex.StackTrace);
			}
		}

		private void GetUserDataCallback(GetUserDataMessage data)
		{
			ExternalIncomingScriptCall.Remove(nameof(GetUserDataCallback));
			_answerText.text = data.id.ToString();
		}

		private void GetEmailCallback(GetEmailMessage data)
		{
			ExternalIncomingScriptCall.Remove(nameof(GetEmailCallback));
			_answerText.SetText($"Email: {data.email}<br>Sign: {data.sign}");
		}
	}
}