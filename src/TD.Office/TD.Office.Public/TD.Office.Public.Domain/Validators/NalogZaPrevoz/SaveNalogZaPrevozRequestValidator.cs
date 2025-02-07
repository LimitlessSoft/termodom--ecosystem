using FluentValidation;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Validators;
using Microsoft.Extensions.Logging;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;

namespace TD.Office.Public.Domain.Validators.NalogZaPrevoz
{
    public class SaveNalogZaPrevozRequestValidator : LSCoreValidatorBase<SaveNalogZaPrevozRequest>
    {
        public SaveNalogZaPrevozRequestValidator()
        {
            RuleFor(x => x.Mobilni)
                .NotEmpty();
            
            RuleFor(x => x.CenaPrevozaBezPdv)
                .GreaterThan(0);
            
            RuleFor(x => x.MiNaplatiliKupcuBezPdv)
                .GreaterThan(0);
            
            RuleFor(x => x.Address)
                .NotEmpty();
            
            RuleFor(x => x.StoreId)
                .GreaterThan(0);

            RuleFor(x => x.Prevoznik)
                .NotEmpty();

            RuleFor(x => x)
                .Custom((nalog, context) =>
                {
                    if(nalog.VrDok == -1)
                        if (String.IsNullOrEmpty(nalog.Note))
                            context.AddFailure(NalogZaPrevozValidationCodes.NZPVC_001.GetDescription());
                    else
                    {
                        if (nalog.BrDok <= 0)
                            context.AddFailure(String.Format(NalogZaPrevozValidationCodes.NZPVC_002.GetDescription(), nameof(nalog.BrDok)));

                        if (nalog.VrDok <= 0)
                            context.AddFailure(String.Format(NalogZaPrevozValidationCodes.NZPVC_002.GetDescription(), nameof(nalog.VrDok)));
                    }

                });
        }
    }
}