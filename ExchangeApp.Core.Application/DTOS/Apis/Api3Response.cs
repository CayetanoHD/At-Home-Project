using System.Xml.Serialization;

namespace ExchangeApp.Core.Application.DTOS.Apis
{
    [XmlRoot("xml")]
    public class Api3Response
    {
        [XmlElement("result")]
        public decimal Result { get; set; }
    }
}
