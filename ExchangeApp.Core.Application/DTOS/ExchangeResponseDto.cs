using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Core.Application.DTOS
{
    using System.Xml.Serialization;

    public class ExchangeResponseDto
    {
        public string ProviderName { get; set; }

        public decimal ConvertedAmount { get; set; }

        public string? Error { get; set; }
    }

}
