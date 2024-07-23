using Radzen;

namespace MMS.Components.Helpers
{
	public interface INotifications
	{
		public void ShowErrorNotification(string error);

		public void ShowSuccessNotification(string message);
	}
}
