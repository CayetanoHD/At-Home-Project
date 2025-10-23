using System.ComponentModel.DataAnnotations;

namespace ExchangeApp.Core.Application.Dtos
{
    public class ExchangeRequestDto
    {
        [Required(ErrorMessage = "Source currency is required.")]
        public string SourceCurrency { get; set; } = null!;

        [Required(ErrorMessage = "Target currency is required.")]
        public string TargetCurrency { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }
}
