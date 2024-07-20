namespace MMS.Models.ViewModels
{
	public class UserDataViewModel
	{
        public UserDataViewModel(int id, string name, string value = "")
        {
            Id = id;
            Name = name;
            Value = value;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
