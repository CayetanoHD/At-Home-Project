using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Core.Application.DTOS.Apis.Api2
{
    public class Api2Request
    {
        [Required(ErrorMessage = "Source currency is required.")]
        [RegularExpression("^(USD|DOP|EUR|usd|dop|eur)$", ErrorMessage = "Only USD, DOP, or EUR are allowed.")]
        public string SourceCurrency { get; set; }

        [Required(ErrorMessage = "Target currency is required.")]
        [RegularExpression("^(USD|DOP|EUR|dop|eur|usd)$", ErrorMessage = "Only USD, DOP, or EUR are allowed.")]
        public string TargetCurrency { get; set; }

        [Required(ErrorMessage = "Quantity is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
        public decimal Quantity { get; set; }
    }
}
