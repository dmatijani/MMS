namespace MMS.Models.ViewModels
{
	public class MembershipRequestViewModel
	{
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PrimaryEmail { get; set; }
        public string MembershipReason { get; set; }
        public List<UserDataViewModel> UserData { get; set; }
    }
}
