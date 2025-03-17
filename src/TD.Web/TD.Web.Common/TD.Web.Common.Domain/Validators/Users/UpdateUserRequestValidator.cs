using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Helpers.Users;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Repository;

// ReSharper disable InvertIf
// ReSharper disable RedundantJumpStatement

namespace TD.Web.Common.Domain.Validators.Users;

public class UpdateUserRequestValidator : LSCoreValidatorBase<UpdateUserRequest>
{
	private readonly Int16 _usernameMinimumLength = 3;
	private readonly Int16 _usernameMaximumLength = 32;
	private readonly Int16 _nicknameMinimumLength = 3;
	private readonly Int16 _nicknameMaximumLength = 32;
	private readonly Int16 _mobileMaximumLength = 16;
	private readonly Int16 _addressMaximumLength = 32;
	private readonly Int16 _mailMaximumLength = 32;
	private readonly Int16 _minAge = 18;
	private readonly Int16 _maxAge = 70;
	private readonly Int16 _commentMaximumLength = 1024;

	public UpdateUserRequestValidator(WebDbContext dbContext)
	{
		RuleFor(x => x)
			.Custom(
				(request, context) =>
				{
					if (request.Username.IsUsernameNotValid())
					{
						context.AddFailure(UsersValidationCodes.UVC_007.GetDescription());
						return;
					}
					var user = dbContext
						.Users.AsNoTrackingWithIdentityResolution()
						.FirstOrDefault(x => x.Username.ToUpper() == request.Username.ToUpper());
					if (user != null && user.Id != request.Id)
					{
						context.AddFailure(UsersValidationCodes.UVC_002.GetDescription());
						return;
					}
				}
			)
			.When(x => x.Id == null);

		RuleFor(x => x.Id)
			.NotNull()
			.Custom(
				(userId, context) =>
				{
					if (
						!dbContext
							.Users.AsNoTrackingWithIdentityResolution()
							.Any(x => x.Id == userId)
					)
						context.AddFailure(UsersValidationCodes.UVC_027.GetDescription());
				}
			);

		RuleFor(x => x.Username)
			.NotNull()
			.WithMessage(UsersValidationCodes.UVC_021.GetDescription())
			.NotEmpty()
			.WithMessage(UsersValidationCodes.UVC_021.GetDescription())
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
			);

		RuleFor(x => x.Nickname)
			.NotNull()
			.WithMessage(UsersValidationCodes.UVC_011.GetDescription())
			.NotEmpty()
			.WithMessage(UsersValidationCodes.UVC_011.GetDescription())
			.MinimumLength(_nicknameMinimumLength)
			.WithMessage(
				string.Format(
					UsersValidationCodes.UVC_008.GetDescription()!,
					_nicknameMinimumLength
				)
			)
			.MaximumLength(_nicknameMaximumLength)
			.WithMessage(
				string.Format(
					UsersValidationCodes.UVC_009.GetDescription()!,
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
			.NotNull()
			.MaximumLength(_mobileMaximumLength)
			.Must(
				(request, mobile) =>
				{
					return !dbContext
						.Users.AsNoTrackingWithIdentityResolution()
						.Any(x => x.Id != request.Id && x.Mobile == mobile);
				}
			)
			.WithMessage(UsersValidationCodes.UVC_028.GetDescription()!);

		RuleFor(x => x.Address).NotNull().MaximumLength(_addressMaximumLength);

		RuleFor(x => x.Mail).MaximumLength(_mailMaximumLength);

		RuleFor(x => x.CityId)
			.NotNull()
			.NotEmpty()
			.Custom(
				(city, context) =>
				{
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
			.NotEmpty()
			.Custom(
				(storeId, context) =>
				{
					if (
						!dbContext
							.Stores.AsNoTrackingWithIdentityResolution()
							.Any(x => x.Id == storeId && x.IsActive)
					)
						context.AddFailure(UsersValidationCodes.UVC_023.GetDescription());
				}
			);

		RuleFor(x => x.ProfessionId)
			.Custom(
				(professionId, context) =>
				{
					if (
						professionId != null
						&& !dbContext
							.Professions.AsNoTrackingWithIdentityResolution()
							.Any(x => x.Id == professionId && x.IsActive)
					)
						context.AddFailure(UsersValidationCodes.UVC_024.GetDescription());
				}
			);

		RuleFor(x => x.Comment)
			.MaximumLength(_commentMaximumLength)
			.WithMessage(
				string.Format(UsersValidationCodes.UVC_026.GetDescription()!, _commentMaximumLength)
			);
	}
}
