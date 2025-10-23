using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeApp.Core.Application.DTOS.Apis
{

    [XmlRoot("Api3Request")]
    public class Api3Request
    {
        [XmlElement("from")]
        public string From { get; set; }

        [XmlElement("to")]
        public string To { get; set; }

        [XmlElement("amount")]
        public decimal Amount { get; set; }
    }




}
