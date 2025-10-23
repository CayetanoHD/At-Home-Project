using System.ComponentModel.DataAnnotations;

namespace ExchangeApp.Core.Application.Dtos
{
    public class ExchangeRequestDto
    {
        [Required(ErrorMessage = "From currency is required.")]
        public string From { get; set; } = null!;

        [Required(ErrorMessage = "To currency is required.")]
        public string To { get; set; } = null!;

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
        public decimal Amount { get; set; }
    }
}
