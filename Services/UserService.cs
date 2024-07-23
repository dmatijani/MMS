using MMS.Data.Repositories;
using MMS.Models;
using MMS.Models.ViewModels;
using MMS.Services.Responses;
using System.Text.RegularExpressions;

namespace MMS.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _repo;
		private readonly IUserDataRepository _dataRepo;
		private readonly IRoleRepository _roleRepo;
		private readonly PasswordHasher _hasher;

		public UserService(IUserRepository repository, IUserDataRepository dataRepository, IRoleRepository roleRepository, PasswordHasher hasher)
		{
			_repo = repository;
			_dataRepo = dataRepository;
			_roleRepo = roleRepository;
			_hasher = hasher;
		}

		public async Task<User?> GetUserByLoginInformation(LoginViewModel model)
		{
			if (model.Password == null)
			{
				return null;
			}
			var users = await _repo.Get();
			return users.FirstOrDefault(u => u.Email == model.Email && _hasher.VerifyPassword(model.Password, u.Password) && u.Approved);
		}

		public async Task<List<User>> GetNonApprovedUsers()
		{
			return (await _repo.Get()).Where(u => !u.Approved).OrderByDescending(u => u.MembershipRequestDate).ToList();
		}

		public async Task<List<User>> GetExistingUsers()
		{
			return (await _repo.Get()).Where(u => u.Approved && u.Role.Name != "Admin").OrderBy(u => u.MembershipApprovalDate).ToList();
		}

		public async Task<User?> GetUserById(int userId, bool approved = true)
		{
			User? user = await _repo.Get(userId);
			if (user == null)
			{
				return null;
			}

			return (user.Approved != approved) ? null : user;
		}

		public async Task<ServiceResponse> RejectRequest(User user)
		{
			if (user.Approved == true)
			{
				return new ServiceResponse(false, "Problem s spajanjem na bazu.");
			}

			await _repo.Delete(user);
			return new ServiceResponse(true, "Uspjeh");
		}

		public async Task<ServiceResponse> DeleteUser(User user)
		{
			await _repo.Delete(user);
			return new ServiceResponse(true, "Uspjeh");
		}

		public async Task<ApprovedRequestServiceResponse> ApproveRequest(User user)
		{
			if (user.Approved == true)
			{
				return new ApprovedRequestServiceResponse(false, null, "", "Problem s spajanjem na bazu.");
			}

			user.Approved = true;
			string password = GeneratePassword();
			user.Password = _hasher.HashPassword(password);
			user.MembershipApprovalDate = DateTime.Now;
			await _repo.Update(user);
			User? updatedUser = (await _repo.Get()).Where(u => u.Email == user.Email).FirstOrDefault();
			return new ApprovedRequestServiceResponse(true, updatedUser, password, "Uspjeh");
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

		public async Task<ServiceResponse> Update(User user)
		{
			try
			{
				await _repo.Update(user);
			} catch (Exception ex)
			{
				return new ServiceResponse(false, "Problem s spajanjem na bazu.");
			}
			return new ServiceResponse(true, "Korisnik ažuriran!");
		}

		public async Task AddAdminUser()
		{
			var existingUsers = await _repo.Get();
			if (!existingUsers.Where(u => u.Role.Name == "Admin").Any())
			{
				int roleId = await GetRoleOrAddNew("Admin");
				User admin = new User
				{
					Email = "admin@admin.mms",
					Password = _hasher.HashPassword("admin"),
					RoleId = roleId,
					Approved = true,
					MembershipReason = "",
					Name = "Admin",
					Surname = "MMS"
				};

				await _repo.Create(admin);
			}
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
				MembershipRequestDate = DateTime.Now,
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
			return newRoleFromDb!.Id;
		}

		private string GeneratePassword()
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			Random random = new Random();
			int stringLength = 16;
			char[] randomArray = new char[stringLength];

			for (int i = 0; i < stringLength; i ++)
			{
				randomArray[i] = chars[random.Next(chars.Length)];
			}

			return new string(randomArray);
		}
	}
}
