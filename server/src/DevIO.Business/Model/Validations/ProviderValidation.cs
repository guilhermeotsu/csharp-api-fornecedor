using DevIO.Business.Model.Validations.Documents;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DevIO.Business.Model.Validations
{
    public class ProviderValidation : AbstractValidator<Provider>
    {
        public ProviderValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("The field {PropertyName} is required")
                .Length(2, 100).WithMessage("The field {PropertyName} must have between {MinLength} and {MaxLength}");

            When(x => x.TypeProvider == TypeProvider.PhysicalPerson, () =>
            {
                RuleFor(x => x.Document.Length).Equal(CpfValidacao.TamanhoCpf)
                    .WithMessage("The fiel Document need to have {ComparisonValue} characters and was supplied {PropertyValue}.");

                RuleFor(x => CpfValidacao.Validar(x.Document)).Equal(true)
                    .WithMessage("Invalid Document!");
            });

            When(x => x.TypeProvider == TypeProvider.LegalPerson, () =>
            {
                RuleFor(x => x.Document.Length).Equal(CnpjValidacao.TamanhoCnpj)
                 .WithMessage("The fiel Document need to have {ComparisonValue} characters and was supplied {PropertyValue}.");

                RuleFor(x => CnpjValidacao.Validar(x.Document)).Equal(true)
                .WithMessage("Invalid document!");
            });
        }
    }
}
