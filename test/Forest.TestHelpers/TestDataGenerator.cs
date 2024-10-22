using Forest.Data;
using Forest.Data.Estimations;
using Forest.Data.Hydraulics;
using Forest.Data.Tree;

namespace Forest.TestHelpers
{
    public static class TestDataGenerator
    {
        public static EventTreeProject CreateTestViewModel()
        {
            return new EventTreeProject
            {
                EventTree =
                {
                    MainTreeEvent = CreateEventTree("Second event tree", 2)
                },
                Experts =
                {
                    new Expert
                    {
                        Name = "Klaas",
                        Email = "email@domein.nl",
                        Expertise = "Alles",
                        Organization = "Eigen bedrijf",
                        Telephone = "088-3358339"
                    },
                    new Expert
                    {
                        Name = "Piet",
                        Email = "piet@email.nl",
                        Expertise = "Niets",
                        Organization = "Ander bedrijf",
                        Telephone = "088-3358339"
                    }
                }
            };
        }

        private static TreeEvent CreateEventTree(string treeName, int numberTreeEvents)
        {
            var mainTreeEvent = new TreeEvent
            {
                Name = "First element"
            };

            var currentTreeEvent = mainTreeEvent;
            for (var i = 0; i < numberTreeEvents - 1; i++)
            {
                var falseEvent = new TreeEvent
                {
                    Name = string.Format("Event no. {0}", i + 1)
                };
                currentTreeEvent.FailingEvent = falseEvent;
                currentTreeEvent = falseEvent;
            }

            return mainTreeEvent;
        }

        public static EventTreeProject GenerateAsphaltProject()
        {
            var andre = new Expert
            {
                Name = "Andre van Hoven",
                Email = "Andre.vanHoven@deltares.nl",
                Expertise = "Asfalt bekleding",
                Organization = "Deltares",
                Telephone = ""
            };
            var erik = new Expert
            {
                Name = "Erik Vastenburg",
                Email = "e.vastenburg@hhnk.nl",
                Expertise = "Faalpaden, geotechniek",
                Organization = "HHNK",
                Telephone = ""
            };
            var roy = new Expert
            {
                Name = "Roy Mom",
                Email = "",
                Expertise = "Bekledingen",
                Organization = "",
                Telephone = ""
            };
            var andries = new Expert
            {
                Name = "Andries Nederpel",
                Email = "a.nederpel@hhnk.nl",
                Expertise = "",
                Organization = "HHNK",
                Telephone = ""
            };
            var dirk = new Expert
            {
                Name = "Dirk",
                Email = "",
                Expertise = "Asfalt",
                Organization = "HKV",
                Telephone = ""
            };

            var hydraulicCondition23 = new HydraulicCondition(2.3, (Probability)3.33E-02, double.NaN, double.NaN);
            var hydraulicCondition26 = new HydraulicCondition(2.6, (Probability)1.00E-02, double.NaN, double.NaN);
            var hydraulicCondition29 = new HydraulicCondition(2.9, (Probability)3.33E-03, double.NaN, double.NaN);
            var hydraulicCondition32 = new HydraulicCondition(3.2, (Probability)1.00E-03, double.NaN, double.NaN);
            var hydraulicCondition35 = new HydraulicCondition(3.5, (Probability)3.33E-04, double.NaN, double.NaN);
            var hydraulicCondition38 = new HydraulicCondition(3.8, (Probability)1.00E-04, double.NaN, double.NaN);

            return new EventTreeProject
            {
                Name = "AGK - HHNK",
                ProjectLeader =
                {
                    Email = "g.deVries@hhnk.nl",
                    Name = "Goaitske de Vries"
                },
                AssessmentSection = "12-3",
                EventTree =
                {
                    MainTreeEvent = new TreeEvent
                    {
                        Name = "Knoop 1",
                        ProbabilitySpecificationType = ProbabilitySpecificationType.Classes,
                        ClassesProbabilitySpecification =
                        {
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Six,
                                MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Six
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Six
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Six,
                                MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five
                            }
                        },
                        FailingEvent = new TreeEvent
                        {
                            Name = "Knoop 2",
                            ProbabilitySpecificationType = ProbabilitySpecificationType.Classes,
                            ClassesProbabilitySpecification =
                            {
                                new ExpertClassEstimation
                                {
                                    Expert = andre, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Six,
                                    MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Seven
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andre, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five,
                                    MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Six
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andre, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four,
                                    MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andre, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Three,
                                    MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andre, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Three,
                                    MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andre, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Three,
                                    MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four
                                },
                                new ExpertClassEstimation
                                    { Expert = erik, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Six },
                                new ExpertClassEstimation
                                    { Expert = erik, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five },
                                new ExpertClassEstimation
                                    { Expert = erik, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four },
                                new ExpertClassEstimation
                                {
                                    Expert = erik, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Three
                                },
                                new ExpertClassEstimation
                                    { Expert = erik, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Two },
                                new ExpertClassEstimation
                                    { Expert = erik, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.One },
                                new ExpertClassEstimation
                                {
                                    Expert = roy, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.None,
                                    MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = roy, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.None,
                                    MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = roy, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.None,
                                    MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = roy, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.None,
                                    MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = roy, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.None,
                                    MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = roy, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.None,
                                    MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andries, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                    MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andries, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Six,
                                    MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Seven
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andries, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Five,
                                    MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Six
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andries, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Four,
                                    MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andries, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Two,
                                    MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Three
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = andries, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.One,
                                    MinEstimation = ProbabilityClass.One, MaxEstimation = ProbabilityClass.Two
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = dirk, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Five,
                                    MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Five
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = dirk, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five,
                                    MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = dirk, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four,
                                    MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = dirk, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Four,
                                    MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = dirk, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Three,
                                    MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Four
                                },
                                new ExpertClassEstimation
                                {
                                    Expert = dirk, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Three,
                                    MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four
                                }
                            }
                        }
                    }
                },
                Experts =
                {
                    andre,
                    erik,
                    roy,
                    andries,
                    dirk
                },
                HydraulicConditions =
                {
                    hydraulicCondition23,
                    hydraulicCondition26,
                    hydraulicCondition29,
                    hydraulicCondition32,
                    hydraulicCondition35,
                    hydraulicCondition38
                }
            };
        }
    }
}