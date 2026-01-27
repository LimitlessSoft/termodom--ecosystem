using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Permissions;
using TD.Office.Public.Contracts.Dtos.Users;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Domain.Managers;

public class UserManager(
	LSCoreAuthContextEntity<string> contextEntity,
	IUserRepository userRepository
) : IUserManager
{
	public string Login(UsersLoginRequest request)
	{
		request.Validate();

		// I am not checking permissions nor username nor password here
		// because this is handled in validator
		var user = userRepository
			.GetMultiple()
			.FirstOrDefault(x => x.Username.ToUpper() == request.Username!.ToUpper());

		if (user == null)
			throw new LSCoreForbiddenException();

		// return user.GenerateJSONWebToken(configurationRoot);
		return "asd";
	}

	public UserMeDto Me() =>
		contextEntity.IsAuthenticated == false
			? new UserMeDto()
			: userRepository.GetCurrentUser().ToMapped<UserEntity, UserMeDto>();

	public UserDto GetSingle(LSCoreIdRequest request) =>
		userRepository
			.GetMultiple()
			.Include(x => x.TipKorisnika)
			.FirstOrDefault(x => x.Id == request.Id)
			?.ToMapped<UserEntity, UserDto>()
			?? throw new LSCoreNotFoundException();

	public LSCoreSortedAndPagedResponse<UserDto> GetMultiple(UsersGetMultipleRequest request) =>
		userRepository
			.GetMultiple()
			.Include(x => x.TipKorisnika)
			.Where(x => x.IsActive)
			.ToSortedAndPagedResponse<UserEntity, UsersSortColumnCodes.Users, UserDto>(
				request,
				UsersSortColumnCodes.UsersSortRules,
				x => x.ToMapped<UserEntity, UserDto>()
			);

	public void UpdateNickname(UsersUpdateNicknameRequest request) =>
		userRepository.UpdateNickname(request.Id!.Value, request.Nickname);

	public UserDto Create(UsersCreateRequest request)
	{
		var entity = new UserEntity
		{
			IsActive = true,
			Username = request.Username,
			Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password),
			Nickname = request.Nickname,
			Type = UserType.User,
			TipKorisnikaId = request.TipKorisnikaId
		};

		userRepository.Insert(entity);
		return entity.ToMapped<UserEntity, UserDto>();
	}

	public List<PermissionDto> GetPermissions(LSCoreIdRequest request)
	{
		var user = userRepository
			.GetMultiple()
			.AsNoTracking()
			.Include(x => x.Permissions)
			.FirstOrDefault(x => x.Id == request.Id);

		if (user == null)
			throw new LSCoreNotFoundException();

		var allPermissions = Enum.GetValues<Permission>();
		return allPermissions
			.Select(p =>
			{
				var permission = user.Permissions?.FirstOrDefault(up =>
					up.Permission == p && up.IsActive
				);
				return new PermissionDto
				{
					Name = p.ToString(),
					Description = p.GetDescription()!,
					IsGranted = permission != null,
					Id = (long)p
				};
			})
			.ToList();
	}

	public void UpdatePermission(UsersUpdatePermissionRequest request)
	{
		request.Validate();

		var user = userRepository
			.GetMultiple()
			.Include(x => x.Permissions)
			.FirstOrDefault(x => x.Id == request.Id);

		if (user is null)
			throw new LSCoreNotFoundException();

		if (user.Permissions!.All(x => x.Permission != request.Permission))
			user.Permissions.Add(
				new UserPermissionEntity
				{
					UserId = request.Id!.Value,
					Permission = request.Permission!.Value,
					IsActive = request.IsGranted
				}
			);

		var perm = user.Permissions!.First(x => x.Permission == request.Permission);
		perm!.IsActive = request.IsGranted;
		userRepository.Update(user);
	}

	/// <summary>
	/// Check if current user has permission
	/// </summary>
	/// <param name="permission"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public bool HasPermission(Permission permission)
	{
		var currentUser = userRepository.GetCurrentUser();

		return userRepository
			.GetMultiple()
			.Where(x => x.Id == currentUser.Id)
			.Include(x => x.Permissions)
			.Select(x => x.Permissions)
			.First()!
			.Any(x => x.Permission == permission);
	}

	public void UpdatePassword(UsersUpdatePasswordRequest request)
	{
		request.Validate();

		var user = userRepository.Get(request.Id!.Value);
		user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
		userRepository.Update(user);
	}

	public void UpdateMaxRabatMpDokumenti(UpdateMaxRabatMPDokumentiRequest request)
	{
		request.Validate();

		var user = userRepository.Get(request.Id!.Value);
		user.MaxRabatMPDokumenti = request.MaxRabatMPDokumenti;
		userRepository.Update(user);
	}

	public void UpdateMaxRabatVpDokumenti(UpdateMaxRabatVPDokumentiRequest request)
	{
		request.Validate();

		var user = userRepository.Get(request.Id!.Value);
		user.MaxRabatVPDokumenti = request.MaxRabatVPDokumenti;
		userRepository.Update(user);
	}

	public void UpdateStoreId(UpdateStoreIdRequest request)
	{
		var user = userRepository.Get(request.Id);
		user.StoreId = request.StoreId;
		userRepository.Update(user);
	}

	public void UpdateVPMagacinId(UpdateVPMagacinIdRequest request)
	{
		var user = userRepository.Get(request.Id);
		user.VPMagacinId = request.VPMagacinId;
		userRepository.Update(user);
	}

	public void UpdateTipKorisnikaId(UpdateTipKorisnikaIdRequest request)
	{
		var user = userRepository.Get(request.Id);
		user.TipKorisnikaId = request.TipKorisnikaId;
		userRepository.Update(user);
	}

	public void UpdatePPIDZaNarudzbenicu(UpdatePPIDZaNarudzbenicuRequest request)
	{
		var user = userRepository.Get(request.Id);
		user.PPIDZaNarudzbenicu = request.PPIDZaNarudzbenicu;
		userRepository.Update(user);
	}
}
