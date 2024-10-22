using System;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class FragilityCurveElementXmlEntity : XmlEntityBase
    {
        [XmlAttribute(AttributeName = "waterlevel")]
        public double WaterLevel { get; set; }

        [XmlAttribute(AttributeName = "probability")]
        public double Probability { get; set; }

        [XmlAttribute(AttributeName = "order")]
        public long Order { get; set; }
    }
}