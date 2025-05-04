using FluentValidation;
using LSCore.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Helpers.Users;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Validators.Users;

public class UserRegisterRequestValidator : UserPasswordValidatorBase<UserRegisterRequest>
{
	private readonly short _usernameMinimumLength = 3;
	private readonly short _usernameMaximumLength = 32;
	private readonly short _nicknameMinimumLength = 3;
	private readonly short _nicknameMaximumLength = 128;
	private readonly short _mobileMaximumLength = 16;
	private readonly short _addressMaximumLength = 32;
	private readonly short _mailMaximumLength = 256;
	private readonly short _minAge = 18;
	private readonly short _maxAge = 70;

	public UserRegisterRequestValidator(IWebDbContextFactory dbContextFactory)
		: base()
	{
		RuleFor(x => x.Username)
			.NotEmpty()
			.WithMessage(UsersValidationCodes.UVC_001.GetDescription())
			.MinimumLength(_usernameMinimumLength)
			.WithMessage(
				string.Format(
					UsersValidationCodes.UVC_004.GetDescription()!,
					_usernameMinimumLength
				)
			)
			.MaximumLength(_usernameMaximumLength)
			.WithMessage(
				string.Format(
					UsersValidationCodes.UVC_005.GetDescription()!,
					_usernameMaximumLength
				)
			)
			.Custom(
				(username, context) =>
				{
					if (username.IsUsernameNotValid())
					{
						context.AddFailure(UsersValidationCodes.UVC_007.GetDescription());
						return;
					}
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					var user = dbContext
						.Users.AsNoTrackingWithIdentityResolution()
						.FirstOrDefault(x => x.Username.ToUpper() == username.ToUpper());
					if (user != null)
					{
						context.AddFailure(UsersValidationCodes.UVC_002.GetDescription());
						return;
					}
				}
			);

		RuleFor(x => x.Nickname)
			.NotEmpty()
			.WithMessage(UsersValidationCodes.UVC_011.GetDescription())
			.MinimumLength(_nicknameMinimumLength)
			.WithMessage(
				string.Format(
					UsersValidationCodes.UVC_012.GetDescription()!,
					_nicknameMinimumLength
				)
			)
			.MaximumLength(_nicknameMaximumLength)
			.WithMessage(
				string.Format(
					UsersValidationCodes.UVC_013.GetDescription()!,
					_nicknameMaximumLength
				)
			);

		RuleFor(x => x.DateOfBirth)
			.NotNull()
			.Custom(
				(dateOfBirth, context) =>
				{
					var age = DateTime.Now.Year - dateOfBirth.Year;
					if (age < _minAge || age > _maxAge)
					{
						context.AddFailure(UsersValidationCodes.UVC_014.GetDescription());
						return;
					}
				}
			);

		RuleFor(x => x.Mobile)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.WithMessage(UsersValidationCodes.UVC_030.GetDescription())
			.MaximumLength(_mobileMaximumLength)
			.WithMessage(UsersValidationCodes.UVC_030.GetDescription())
			.Must(MobilePhoneHelpers.IsValidMobile)
			.WithMessage(UsersValidationCodes.UVC_030.GetDescription())
			.Must(
				(mobile) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					return !dbContext
						.Users.AsNoTrackingWithIdentityResolution()
						.Any(x => x.Mobile == mobile);
				}
			)
			.WithMessage(UsersValidationCodes.UVC_028.GetDescription());

		RuleFor(x => x.Address)
			.NotNull()
			.WithMessage(UsersValidationCodes.UVC_032.GetDescription())
			.MaximumLength(_addressMaximumLength)
			.WithMessage(
				string.Format(UsersValidationCodes.UVC_033.GetDescription()!, _addressMaximumLength)
			);

		RuleFor(x => x.Mail)
			.MaximumLength(_mailMaximumLength)
			.WithMessage(UsersValidationCodes.UVC_031.GetDescription())
			.Must(EmailHelpers.IsEmailValid)
			.WithMessage(UsersValidationCodes.UVC_031.GetDescription());

		RuleFor(x => x.CityId)
			.NotNull()
			.WithMessage(UsersValidationCodes.UVC_035.GetDescription())
			.NotEmpty()
			.WithMessage(UsersValidationCodes.UVC_035.GetDescription())
			.Custom(
				(city, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					if (
						!dbContext
							.Cities.AsNoTrackingWithIdentityResolution()
							.Any(x => x.Id == city && x.IsActive)
					)
						context.AddFailure(UsersValidationCodes.UVC_022.GetDescription());
				}
			);

		RuleFor(x => x.FavoriteStoreId)
			.NotNull()
			.WithMessage(UsersValidationCodes.UVC_034.GetDescription())
			.NotEmpty()
			.WithMessage(UsersValidationCodes.UVC_034.GetDescription())
			.Custom(
				(storeId, context) =>
				{
					using var dbContext = dbContextFactory.Create<WebDbContext>();
					if (
						!dbContext
							.Stores.AsNoTrackingWithIdentityResolution()
							.Any(x => x.Id == storeId && x.IsActive)
					)
						context.AddFailure(UsersValidationCodes.UVC_023.GetDescription());
				}
			);
	}
}
