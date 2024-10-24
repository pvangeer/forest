using System;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class HydrodynamicConditionXmlEntity : FragilityCurveElementXmlEntity
    {
        [XmlAttribute(AttributeName = "waveperiod")]
        public double WavePeriod { get; set; }

        [XmlAttribute(AttributeName = "waveheight")]
        public double WaveHeight { get; set; }
    }
}