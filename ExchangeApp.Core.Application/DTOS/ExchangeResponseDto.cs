using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeApp.Core.Application.DTOS
{
    using System.Xml.Serialization;

    [XmlRoot("ExchangeResponse")] 
    public class ExchangeResponseDto
    {
        [XmlElement("ProviderName")]
        public string ProviderName { get; set; }

        [XmlElement("ConvertedAmount")]
        public decimal ConvertedAmount { get; set; }

        public string? Error { get; set; }
    }

}
