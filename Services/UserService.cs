using Microsoft.Identity.Client;
using MMS.Data.Repositories;
using MMS.Models;
using MMS.Models.ViewModels;

namespace MMS.Services
{
	public class UserService
	{
		private readonly IUserRepository _repo;

		public UserService(IUserRepository repository)
		{
			_repo = repository;
		}

		public async Task<User?> GetUserByLoginInformation(LoginViewModel model)
		{
			var users = await _repo.Get();
			return users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password && u.Approved);
		}
	}
}
