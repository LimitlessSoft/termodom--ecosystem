using LSCore.Auth.Contracts;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using Omu.ValueInjecter;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.DtoMappings.Users;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Helpers.Users;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.Users;

namespace TD.Web.Common.Domain.Managers;

public class UserManager(
	IOfficeServerApiManager officeServerApiManager,
	IUserRepository repository,
	IProductRepository productRepository,
	IProfessionRepository professionRepository,
	IProductGroupRepository productGroupRepository,
	IPaymentTypeRepository paymentTypeRepository,
	IProductPriceGroupRepository productPriceGroupRepository,
	IProductPriceGroupLevelRepository productPriceGroupLevelRepository,
	IOrderRepository orderRepository,
	LSCoreAuthContextEntity<string> contextEntity
) : IUserManager
{
	public void Register(UserRegisterRequest request)
	{
		request.Mobile = MobilePhoneHelpers.GenarateValidNumber(request.Mobile);
		request.Validate();

		var profession = professionRepository.GetMultiple().First(); // TODO: It always gets first profession, should be changed

		var user = new UserEntity();
		user.InjectFrom(request);
		user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
		user.CreatedAt = DateTime.UtcNow;
		user.Type = UserType.User;
		user.ProfessionId = profession.Id;
		user.DefaultPaymentTypeId = paymentTypeRepository
			.GetMultiple()
			.OrderByDescending(x => x.IsDefault)
			.First()
			.Id;

		repository.Insert(user);
	}

	public void MarkLastSeen()
	{
		var user = repository
			.GetMultiple()
			.FirstOrDefault(x => x.Username == contextEntity.Identifier);
		if (user is null)
			throw new LSCoreNotFoundException();

		user.LastTimeSeen = DateTime.UtcNow;
		repository.Update(user);
	}

	public void SetUserProductPriceGroupLevel(SetUserProductPriceGroupLevelRequest request)
	{
		request.Validate();

		var user = repository
			.GetMultiple()
			.Include(x => x.ProductPriceGroupLevels)
			.FirstOrDefault(x => x.Id == request.Id);

		if (user == null)
			throw new LSCoreNotFoundException();

		var productPriceGroupLevelEntity = user.ProductPriceGroupLevels.FirstOrDefault(x =>
			x.ProductPriceGroupId == request.ProductPriceGroupId
		);

		if (productPriceGroupLevelEntity != null)
			productPriceGroupLevelEntity.Level = request.Level!.Value;
		else
			user.ProductPriceGroupLevels.Add(
				new ProductPriceGroupLevelEntity()
				{
					UserId = user.Id,
					Level = request.Level!.Value,
					ProductPriceGroupId = request.ProductPriceGroupId!.Value
				}
			);

		repository.Update(user);
	}

	public UserInformationDto Me() =>
		contextEntity.IsAuthenticated
			? repository
				.GetMultiple()
				.First(x => x.Identifier == contextEntity.Identifier)
				.ToUserInformationDto()
			: new UserInformationDto();

	public LSCoreSortedAndPagedResponse<UsersGetDto> GetUsers(UsersGetRequest request)
	{
		request.SortColumn = UsersSortColumnCodes.Users.Id; // TODO: This is fixed to ID

		return repository
			.GetMultiple()
			.Include(x => x.Orders)
			.Where(x =>
				x.Id != 0
				&& (request.HasReferent == null || (x.Referent != null) == request.HasReferent)
				&& (request.IsActive == null || x.IsActive == request.IsActive)
			)
			.ToSortedAndPagedResponse<UserEntity, UsersSortColumnCodes.Users, UsersGetDto>(
				request,
				UsersSortColumnCodes.UsersSortRules,
				x => x.ToMapped<UserEntity, UsersGetDto>()
			);
	}

	public GetSingleUserDto GetSingleUser(GetSingleUserRequest request)
	{
		var user = repository
			.GetMultiple()
			.Include(x => x.Profession)
			.Include(x => x.City)
			.Include(x => x.FavoriteStore)
			.Include(x => x.Referent)
			.FirstOrDefault(x => string.Equals(x.Username, request.Username));

		if (user == null)
			throw new LSCoreNotFoundException();

		var dto = user.ToMapped<UserEntity, GetSingleUserDto>();
		dto.AmIOwner =
			user.ReferentId != null && user.Referent!.Username == contextEntity.Identifier;
		return dto;
	}

	public List<UserProductPriceLevelsDto> GetUserProductPriceLevels(
		GetUserProductPriceLevelsRequest request
	)
	{
		var user = repository
			.GetMultiple()
			.Include(x => x.ProductPriceGroupLevels)
			.FirstOrDefault(x => x.Id == request.UserId);

		if (user == null)
			throw new LSCoreNotFoundException();

		var groups = productPriceGroupRepository.GetMultiple().ToList();
		return user.ProductPriceGroupLevels.ToUserPriceLevelsDto(groups);
	}

	public void UpdateUser(UpdateUserRequest request)
	{
		request.Mobile = MobilePhoneHelpers.GenarateValidNumber(request.Mobile);

		var user = repository.GetMultiple().FirstOrDefault(x => x.Id == request.Id);
		if (user is null)
			throw new LSCoreNotFoundException();

		user.InjectFrom(request);
		repository.Update(user);
	}

	public void PutUserProductPriceLevel(PutUserProductPriceLevelRequest request)
	{
		var priceLevel = productPriceGroupLevelRepository
			.GetMultiple()
			.FirstOrDefault(x =>
				x.UserId == request.UserId && x.ProductPriceGroupId == request.ProductPriceGroupId
			);

		if (priceLevel == null)
		{
			productPriceGroupLevelRepository.Insert(
				new ProductPriceGroupLevelEntity
				{
					UserId = request.UserId,
					ProductPriceGroupId = request.ProductPriceGroupId,
					Level = request.Level
				}
			);
			return;
		}

		priceLevel.Level = request.Level;
		productPriceGroupLevelRepository.Update(priceLevel);
	}

	public void PutUserType(PutUserTypeRequest request)
	{
		var user = repository.GetMultiple().FirstOrDefault(x => x.Username == request.Username);
		if (user == null)
			throw new LSCoreNotFoundException();
		user.Type = request.Type;
		repository.Update(user);
	}

	public void PutUserStatus(PutUserStatusRequest request)
	{
		var user = repository.GetMultiple(true).FirstOrDefault(x => x.Username == request.Username);
		if (user == null)
			throw new LSCoreNotFoundException();

		user.IsActive = request.IsActive;
		repository.Update(user);
	}

	public void GetOwnership(GetOwnershipRequest request)
	{
		var user = repository
			.GetMultiple()
			.FirstOrDefault(x => x.IsActive && x.Username == request.Username);
		if (user == null)
			throw new LSCoreNotFoundException();

		var referentUser = repository
			.GetMultiple()
			.FirstOrDefault(x => x.IsActive && x.Username == contextEntity.Identifier);
		if (referentUser == null)
			throw new LSCoreNotFoundException();

		user.ReferentId = referentUser.Id;
		repository.Update(user);
	}

	public void ApproveUser(ApproveUserRequest request)
	{
		var user = repository.GetMultiple().FirstOrDefault(x => x.Username == request.Username);
		if (user == null)
			throw new LSCoreNotFoundException();

		user.ProcessingDate = DateTime.UtcNow;
		user.IsActive = true;
		repository.Update(user);
	}

	public void ChangeUserPassword(ChangeUserPasswordRequest request)
	{
		request.Validate();

		var user = repository.GetMultiple().FirstOrDefault(x => x.Username == request.Username);
		if (user == null)
			throw new LSCoreNotFoundException();

		user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
		repository.Update(user);

		officeServerApiManager.SmsQueueAsync(
			new SMSQueueRequest
			{
				Numbers = [user.Mobile],
				Text =
					$"{user.Nickname}, Vasa lozinka je promenjena na {request.Password}. Lozinku u svakom trenutku mozete promeniti u delu Moj Kutak."
			}
		);
	}

	public void ResetPassword(UserResetPasswordRequest request)
	{
		// Doing it this way so 404 is not thrown
		var user = repository
			.GetMultiple(true)
			.FirstOrDefault(x => x.IsActive && x.Username.ToLower() == request.Username.ToLower());
		if (user == null)
			return;

		if (
			MobilePhoneHelpers.GenarateValidNumber(user.Mobile)
			!= MobilePhoneHelpers.GenarateValidNumber(request.Mobile)
		)
			return;

		var rawPassword = UsersHelpers.GenerateNewPassword();
		user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(rawPassword);
		repository.Update(user);

		officeServerApiManager.SmsQueueAsync(
			new SMSQueueRequest
			{
				Numbers = [user.Mobile],
				Text =
					user.Nickname
					+ ", Vasa nova lozinka je: "
					+ rawPassword
					+ ". U svakom trenutku samostalno mozete promeniti lozinku u delu Moj Kutak. https://termodom.rs"
			}
		);
	}

	public async Task SendBulkSms(SendBulkSmsRequest request)
	{
		var users = repository.GetMultiple().Where(x => x.ProcessingDate != null);

		var mobilePhones = users.Select(x => x.Mobile).ToList();
		await officeServerApiManager.SmsQueueAsync(
			new SMSQueueRequest() { Numbers = mobilePhones, Text = request.Text }
		);
	}

	public void SetPassword(UserSetPasswordRequest request)
	{
		var user = repository
			.GetMultiple()
			.FirstOrDefault(x => x.IsActive && x.Username == contextEntity.Identifier);
		if (user == null)
			throw new LSCoreNotFoundException();

		if (!BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, user.Password))
			throw new LSCoreBadRequestException("Stara lozinka nije ispravna");

		user.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
		repository.Update(user);
	}

	public UsersAnalyzeOrderedProductsDto AnalyzeOrderedProducts(
		UsersAnalyzeOrderedProductsRequest request
	)
	{
		request.Validate();

		var dateFromUtc = request.Range switch
		{
			UsersAnalyzeOrderedProductsRange.Last30Days => DateTime.UtcNow.AddDays(-30),
			UsersAnalyzeOrderedProductsRange.LastYear => DateTime.UtcNow.AddYears(-1),
			UsersAnalyzeOrderedProductsRange.SinceCreation => new DateTime(),
			UsersAnalyzeOrderedProductsRange.ThisYear => new DateTime(DateTime.UtcNow.Year, 1, 1),
			_ => DateTime.UtcNow
		};

		var orders = orderRepository
			.GetMultiple()
			.Include(x => x.Items)
			.Include(x => x.User)
			.Where(x =>
				x.IsActive && x.User.Username == request.Username && x.CheckedOutAt >= dateFromUtc
			);

		// Get sum of items.quantity from orders grouped by item.productId
		var products = orders
			.SelectMany(x => x.Items)
			.Where(x => x.IsActive)
			.Select(x => x.ProductId)
			.Distinct()
			.ToList();

		var dto = new UsersAnalyzeOrderedProductsDto();

		foreach (var productId in products)
		{
			var product = productRepository.GetOrDefault(productId);
			if (product == null)
				continue;

			try
			{
				dto.Items.Add(
					new UsersAnalyzeOrderedProductsItemDto()
					{
						Id = productId,
						Name = product.Name,
						ValueSum = orders
							.SelectMany(x => x.Items)
							.Where(x => x.IsActive && x.ProductId == productId)
							.ToList()
							.Sum(x => x.Price * x.Quantity),
						DiscountSum = orders
							.SelectMany(x => x.Items)
							.Where(x => x.IsActive && x.ProductId == productId)
							.ToList()
							.Sum(x => (x.PriceWithoutDiscount - x.Price) * x.Quantity),
						QuantitySum = orders
							.SelectMany(x => x.Items)
							.Where(x => x.IsActive && x.ProductId == productId)
							.ToList()
							.Sum(x => x.Quantity)
					}
				);
			}
			catch (Exception)
			{
				// ignored
			}
		}

		return dto;
	}

	/// <summary>
	/// Check if current user has permission
	/// </summary>
	/// <param name="permission"></param>
	/// <returns></returns>
	/// <exception cref="NotImplementedException"></exception>
	public bool HasPermission(Permission permission) =>
		contextEntity.IsAuthenticated
		&& (
			repository
				.GetMultiple()
				.Include(x => x.Permissions)
				.FirstOrDefault(x => x.IsActive && x.Identifier == contextEntity.Identifier)
				?.Permissions.Any(x => x.IsActive && x.Permission == permission) ?? false
		);

	public List<long> GetManagingProductsGroups(string username) =>
		repository
			.GetMultiple()
			.Where(x => x.IsActive && x.Username == username)
			.Include(x => x.ManaginProductGroups)
			.SelectMany(x => x.ManaginProductGroups!.Select(z => z.Id))
			.ToList();

	public void SetManagingProductsGroups(string username, List<long> managingGroups)
	{
		var user = repository
			.GetMultiple()
			.Include(x => x.ManaginProductGroups)
			.FirstOrDefault(x => x.Username == username);

		if (user == null)
			throw new LSCoreNotFoundException();

		user.ManaginProductGroups ??= [];

		user.ManaginProductGroups.AddRange(
			productGroupRepository
				.GetMultiple()
				.Where(x => managingGroups.Any(y => y == x.Id))
				.ToList()
		);

		repository.Update(user);
	}
}
