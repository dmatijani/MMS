using Radzen;

namespace MMS.Components.Helpers
{
	public class Notifications : INotifications
	{
		private NotificationService _service { get; set; }

		public Notifications(NotificationService service)
		{
			_service = service;
		}

		public void ShowErrorNotification(string error)
		{
			_service.Notify(new NotificationMessage
			{
				Severity = NotificationSeverity.Error,
				Duration = 5000,
				Summary = "Greška",
				Detail = error
			});
		}

		public void ShowSuccessNotification(string message)
		{
			_service.Notify(new NotificationMessage
			{
				Severity = NotificationSeverity.Success,
				Duration = 5000,
				Summary = "Uspjeh",
				Detail = message
			});
		}
	}
}
