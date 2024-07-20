namespace MMS.Models.ViewModels
{
	public class UserDataViewModel
	{
        public UserDataViewModel(int id)
        {
            Id = id;
        }

        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
