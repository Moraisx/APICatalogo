using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Validation
{
    //Atributo customizado... [PrimeiraLetraMaiuscula]
    public class PrimeiraLetraMaiusculaAttribute: ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var primeiraLetra = value.ToString()[0].ToString();

            if (primeiraLetra != primeiraLetra.ToUpper())
            {
                return new ValidationResult("A primeira letra do nome deve ser maiúscula");
            }

            return ValidationResult.Success;

            //Implementado pelo [Required]
            //if (value == null || string.IsNullOrEmpty(value.ToString()))
            //{
            //    return new ValidationResult("Nome obrigatório");
            //}
        }
    }
}
