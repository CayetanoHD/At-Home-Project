using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Core.Application.DTOS.Apis.API
{
    public class Api1Request
    {
        [Required(ErrorMessage = "Source currency is required.")]
        public string From { get; set; } = null!;

        [Required(ErrorMessage = "Target currency is required.")]
        public string To { get; set; } = null!;

        [Required(ErrorMessage = "Amount is required.")]
        public decimal Value { get; set; }
    }
}
