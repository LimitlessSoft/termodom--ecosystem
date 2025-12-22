using FluentValidation;
using LSCore.Validation.Domain;
using TD.Web.Admin.Contracts.Requests.Blogs;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Validators.Blogs;

public class BlogsSaveRequestValidator : LSCoreValidatorBase<BlogsSaveRequest>
{
	private const int TitleMaximumLength = 256;
	private const int TitleMinimumLength = 3;
	private const int SlugMaximumLength = 256;
	private const int SummaryMaximumLength = 512;

	public BlogsSaveRequestValidator(IWebDbContextFactory dbContextFactory)
	{
		ClassLevelCascadeMode = CascadeMode.Stop;

		RuleFor(x => x.Title)
			.NotEmpty()
			.WithMessage("Title is required")
			.MaximumLength(TitleMaximumLength)
			.WithMessage($"Title cannot exceed {TitleMaximumLength} characters")
			.MinimumLength(TitleMinimumLength)
			.WithMessage($"Title must be at least {TitleMinimumLength} characters");

		RuleFor(x => x.Text)
			.NotEmpty()
			.WithMessage("Text content is required");

		RuleFor(x => x.Slug)
			.MaximumLength(SlugMaximumLength)
			.WithMessage($"Slug cannot exceed {SlugMaximumLength} characters")
			.When(x => !string.IsNullOrWhiteSpace(x.Slug));

		RuleFor(x => x.Summary)
			.MaximumLength(SummaryMaximumLength)
			.WithMessage($"Summary cannot exceed {SummaryMaximumLength} characters")
			.When(x => !string.IsNullOrWhiteSpace(x.Summary));

		RuleFor(x => x)
			.Custom(
				(request, context) =>
				{
					if (string.IsNullOrWhiteSpace(request.Slug))
						return;

					using var dbContext = dbContextFactory.Create<WebDbContext>();
					var existingBlog = dbContext.Blogs.FirstOrDefault(x =>
						x.Slug == request.Slug
						&& x.IsActive
						&& (request.Id == null || x.Id != request.Id)
					);

					if (existingBlog != null)
						context.AddFailure("Slug", "A blog with this slug already exists");
				}
			);
	}
}
