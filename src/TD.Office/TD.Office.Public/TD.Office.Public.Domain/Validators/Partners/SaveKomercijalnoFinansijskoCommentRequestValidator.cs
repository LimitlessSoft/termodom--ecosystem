using FluentValidation;
using LSCore.Domain.Validators;
using TD.Office.Public.Contracts.Requests.Partneri;

namespace TD.Office.Public.Domain.Validators.Partners;
public class SaveKomercijalnoFinansijskoCommentRequestValidator : LSCoreValidatorBase<SaveKomercijalnoFinansijskoCommentRequest>
{
    private static readonly Int16 _commentMinimumLength = 0;
    private static readonly Int16 _commentMaximumLength = 512;
    public SaveKomercijalnoFinansijskoCommentRequestValidator()
    {
        RuleFor(x => x.Komentar)
            .MinimumLength(_commentMinimumLength)
            .MaximumLength(_commentMaximumLength);
    }
}
