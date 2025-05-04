using FluentValidation;
using LSCore.Validation.Domain;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Domain.Validators.Users
{
	public class UsersCreateRequestValidator : LSCoreValidatorBase<UsersCreateRequest>
	{
		public UsersCreateRequestValidator(OfficeDbContext dbContext)
		{
			RuleFor(x => x.Username)
				.NotEmpty()
				.MinimumLength(3)
				.Custom(
					(username, context) =>
					{
						if (dbContext.Users.Any(x => x.Username == username))
							context.AddFailure("Korisnik sa tim korisničkim imenom već postoji.");
					}
				);

			RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
		}
	}
}
