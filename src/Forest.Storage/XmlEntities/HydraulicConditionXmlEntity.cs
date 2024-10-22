using System;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class HydraulicConditionXmlEntity : FragilityCurveElementXmlEntity
    {
        [XmlAttribute(AttributeName = "waveperiod")]
        public double WavePeriod { get; set; }

        [XmlAttribute(AttributeName = "waveheight")]
        public double WaveHeight { get; set; }
    }
}