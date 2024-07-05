using FluentValidation;
using LSCore.Domain.Validators;
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
            
            RuleFor(x => x.Note)
                .NotEmpty();
            
            RuleFor(x => x.Address)
                .NotEmpty();
            
            RuleFor(x => x.VrDok)
                .GreaterThan(0);
            
            RuleFor(x => x.BrDok)
                .GreaterThan(0);
            
            RuleFor(x => x.StoreId)
                .GreaterThan(0);

            RuleFor(x => x.Prevoznik)
                .NotEmpty();
        }
    }
}