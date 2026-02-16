using FluentValidation;
using InsuranceSimpleApi.DTOs;

namespace InsuranceSimpleApi.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz")
                .MinimumLength(4).WithMessage("Şifre en az 4 karakter olmalı");
        }
    }
}
