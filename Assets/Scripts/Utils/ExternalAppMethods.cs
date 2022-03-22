using System.Runtime.InteropServices;

namespace Utils
{
	public static class ExternalAppMethods
	{
		[DllImport("__Internal")]
		public static extern void Auth(string callbackName);

		[DllImport("__Internal")]
		public static extern void GetUserData(string callbackName);
	}
}
