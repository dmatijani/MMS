using Microsoft.Identity.Client;
using MMS.Data.Repositories;
using MMS.Models;
using MMS.Models.ViewModels;
using MMS.Services.Responses;
using System.Text.RegularExpressions;

namespace MMS.Services
{
	public class UserService
	{
		private readonly IUserRepository _repo;
		private readonly IUserDataRepository _dataRepo;
		private readonly IRoleRepository _roleRepo;

		public UserService(IUserRepository repository, IUserDataRepository dataRepository, IRoleRepository roleRepository)
		{
			_repo = repository;
			_dataRepo = dataRepository;
			_roleRepo = roleRepository;
		}

		public async Task<User?> GetUserByLoginInformation(LoginViewModel model)
		{
			var users = await _repo.Get();
			return users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password && u.Approved);
		}

		public async Task<ServiceResponse> SendNewMembershipRequest(MembershipRequestViewModel model)
		{
			bool membershipDataIsValid = CheckIfDataIsValid(model);
			if (!membershipDataIsValid)
			{
				return new ServiceResponse(false, "Neispravni podaci!");
			}

			var memberWithMail = (await _repo.Get()).Where(u => u.Email == model.PrimaryEmail.Trim()).FirstOrDefault();
			if (memberWithMail != null)
			{
				if (memberWithMail.Approved)
				{
					return new ServiceResponse(false, "Korisnik s tom email adresom već postoji!");
				} else
				{
					return new ServiceResponse(false, "Već je poslan zahtjev s tom email adresom!");
				}
			}

			var roleId = 0;
			try
			{
				roleId = await GetRoleOrAddNew("User");
			} catch (Exception ex)
			{
				return new ServiceResponse(false, "Problem s spajanjem na bazu.");
			}

			User user = await CreateUserFromRequestModel(model, roleId);
			try
			{
				await _repo.Create(user);
			}
			catch (Exception ex)
			{
				return new ServiceResponse(false, "Problem s spajanjem na bazu.");
			}

			return new ServiceResponse(true, "Zahtjev poslan!");
		}

		private bool CheckIfDataIsValid(MembershipRequestViewModel model)
		{
			if (model.Name == null || model.Name.Trim() == "" || model.Name.Trim().Length > 50) return false;
			if (model.Surname == null || model.Surname.Trim() == "" || model.Surname.Trim().Length > 50) return false;
			var emailRegex = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
			if (model.PrimaryEmail == null || model.PrimaryEmail.Trim() == "" || !Regex.Match(model.PrimaryEmail.Trim(), emailRegex).Success) return false;
			
			foreach (var data in model.UserData) {
				if (data.Value == null || data.Value.Trim() == "" || data.Value.Trim().Length > 80) return false;
				if (data.Name == null || data.Name.Trim() == "" || data.Name.Trim().Length > 30) return false;
			}
			
			return true;
		}

		private async Task<User> CreateUserFromRequestModel(MembershipRequestViewModel model, int roleId)
		{
			User newUser = new User
			{
				Email = model.PrimaryEmail.Trim(),
				Password = "",
				Name = model.Name.Trim(),
				Surname = model.Surname.Trim(),
				Approved = false,
				MembershipReason = model.MembershipReason.Trim(),
				RoleId = roleId,
				UserData = model.UserData.Select(d => new UserData
				{
					Name = d.Name.Trim(),
					Value = d.Value.Trim()
				}).ToList()
			};

			return newUser;
		}

		private async Task<int> GetRoleOrAddNew(string roleName)
		{
			var existingRole = (await _roleRepo.Get()).FirstOrDefault(r => r.Name == roleName);
			if (existingRole != null)
			{
				return existingRole.Id;
			}

			Role newRole = new Role
			{
				Name = roleName
			};
			await _roleRepo.Create(newRole);
			var newRoleFromDb = (await _roleRepo.Get()).FirstOrDefault(r => r.Name == roleName);
			return newRoleFromDb.Id;
		}
	}
}
