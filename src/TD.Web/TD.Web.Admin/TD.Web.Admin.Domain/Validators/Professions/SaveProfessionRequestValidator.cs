using FluentValidation;
using LSCore.Common.Extensions;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Enums.ValidationCodes;
using TD.Web.Admin.Contracts.Requests.Professions;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Professions
{
	public class SaveProfessionRequestValidator : LSCoreValidatorBase<SaveProfessionRequest>
	{
		private readonly Int16 _nameMaximumLength = 32;
		private readonly Int16 _nameMinimumLength = 3;

		public SaveProfessionRequestValidator(IWebDbContextFactory dbContextFactory)
		{
			RuleFor(x => x.Name)
				.NotNull()
				.WithMessage(ProfessionValidationCodes.PVC_001.GetDescription())
				.MaximumLength(_nameMaximumLength)
				.WithMessage(
					string.Format(
						ProfessionValidationCodes.PVC_002.GetDescription(),
						_nameMaximumLength
					)
				)
				.MinimumLength(_nameMinimumLength)
				.WithMessage(
					string.Format(
						ProfessionValidationCodes.PVC_003.GetDescription(),
						_nameMinimumLength
					)
				)
				.Custom(
					(name, context) =>
					{
						if (
							dbContextFactory
								.Create<WebDbContext>()
								.Professions.Any(x =>
									x.IsActive && string.Equals(x.Name.ToLower(), name.ToLower())
								)
						)
							context.AddFailure(ProfessionValidationCodes.PVC_004.GetDescription());
					}
				);
		}
	}
}
