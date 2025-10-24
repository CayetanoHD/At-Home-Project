using System.Xml.Serialization;

namespace ExchangeApp.Core.Application.DTOS.Apis.Api3
{
    [XmlRoot("xml")]
    public class Api3Response
    {
        [XmlElement("result")]
        public decimal Result { get; set; }
    }
}
