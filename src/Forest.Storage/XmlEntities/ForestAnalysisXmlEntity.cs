﻿using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Forest.Storage.XmlEntities
{
    [Serializable]
    public class ForestAnalysisXmlEntity : XmlEntityBase
    {
        public ForestAnalysisXmlEntity()
        {
            EventTreeXmlEntities = new Collection<EventTreeXmlEntity>();
            ProbabilityEstimationPerTreeEventXmlEntities = new Collection<ProbabilityEstimationPerTreeEventXmlEntity>();
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlAttribute(AttributeName = "description")]
        public string Description { get; set; }

        [XmlAttribute(AttributeName = "assessmentsection")]
        public string AssessmentSection { get; set; }

        [XmlAttribute(AttributeName = "information")]
        public string ProjectInformation { get; set; }

        [XmlElement(ElementName = "projectleader")]
        public PersonXmlEntity ProjectLeader { get; set; }

        [XmlArray(ElementName = "eventtrees")]
        [XmlArrayItem(ElementName = "eventtree")]
        public Collection<EventTreeXmlEntity> EventTreeXmlEntities { get; set; }

        [XmlArray(ElementName = "estimationsperevent")]
        [XmlArrayItem(ElementName = "estimationperevent")]
        public Collection<ProbabilityEstimationPerTreeEventXmlEntity> ProbabilityEstimationPerTreeEventXmlEntities { get; set; }
    }
}