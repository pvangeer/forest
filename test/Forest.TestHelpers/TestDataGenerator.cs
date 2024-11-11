using Forest.Data;
using Forest.Data.Estimations.PerTreeEvent;
using Forest.Data.Estimations.PerTreeEvent.Experts;
using Forest.Data.Hydrodynamics;
using Forest.Data.Probabilities;
using Forest.Data.Tree;

namespace Forest.TestHelpers
{
    public static class TestDataGenerator
    {
        public static ForestAnalysis CreateTestViewModel()
        {
            var treeEvent = CreateEventTree("Second event tree", 2);
            var eventTree = new EventTree { MainTreeEvent = treeEvent };
            return new ForestAnalysis
            {
                EventTrees =
                {
                    eventTree
                },
                ProbabilityEstimationsPerTreeEvent =
                {
                    new ProbabilityEstimationPerTreeEvent
                    {
                        EventTree = eventTree,
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
                    }
                }
            };
        }

        private static TreeEvent CreateEventTree(string treeName, int numberTreeEvents)
        {
            var mainTreeEvent = new TreeEvent("First element", TreeEventType.MainEvent);

            var currentTreeEvent = mainTreeEvent;
            for (var i = 0; i < numberTreeEvents - 1; i++)
            {
                var falseEvent = new TreeEvent("", TreeEventType.Failing)
                {
                    Name = string.Format("Event no. {0}", i + 1)
                };
                currentTreeEvent.FailingEvent = falseEvent;
                currentTreeEvent = falseEvent;
            }

            return mainTreeEvent;
        }

        public static ForestAnalysis GenerateAsphaltProject()
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

            var hydraulicCondition23 = new HydrodynamicCondition(2.3, (Probability)3.33E-02, double.NaN, double.NaN);
            var hydraulicCondition26 = new HydrodynamicCondition(2.6, (Probability)1.00E-02, double.NaN, double.NaN);
            var hydraulicCondition29 = new HydrodynamicCondition(2.9, (Probability)3.33E-03, double.NaN, double.NaN);
            var hydraulicCondition32 = new HydrodynamicCondition(3.2, (Probability)1.00E-03, double.NaN, double.NaN);
            var hydraulicCondition35 = new HydrodynamicCondition(3.5, (Probability)3.33E-04, double.NaN, double.NaN);
            var hydraulicCondition38 = new HydrodynamicCondition(3.8, (Probability)1.00E-04, double.NaN, double.NaN);

            var failingEvent = new TreeEvent("Knoop 2", TreeEventType.Failing);
            var mainTreeEvent = new TreeEvent("Knoop 1", TreeEventType.MainEvent)
            {
                FailingEvent = failingEvent
            };

            var probabilityEstimation1 = new ProbabilityEstimationPerTreeEvent
            {
                Experts =
                {
                    andre,
                    erik,
                    roy,
                    andries,
                    dirk
                },
                HydrodynamicConditions =
                {
                    hydraulicCondition23,
                    hydraulicCondition26,
                    hydraulicCondition29,
                    hydraulicCondition32,
                    hydraulicCondition35,
                    hydraulicCondition38
                },
                Estimates =
                {
                    new TreeEventProbabilityEstimate(mainTreeEvent)
                    {
                        ProbabilitySpecificationType = ProbabilitySpecificationType.Classes,
                        ClassProbabilitySpecifications =
                        {
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Six,
                                MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Six
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Six
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Six,
                                MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five
                            }
                        }
                    },
                    new TreeEventProbabilityEstimate(failingEvent)
                    {
                        ProbabilitySpecificationType = ProbabilitySpecificationType.Classes,
                        ClassProbabilitySpecifications =
                        {
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Six,
                                MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Six
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four,
                                MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Three,
                                MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Three,
                                MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andre, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Three,
                                MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four
                            },
                            new ExpertClassEstimation
                                { Expert = erik, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Six },
                            new ExpertClassEstimation
                                { Expert = erik, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five },
                            new ExpertClassEstimation
                                { Expert = erik, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four },
                            new ExpertClassEstimation
                            {
                                Expert = erik, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Three
                            },
                            new ExpertClassEstimation
                                { Expert = erik, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Two },
                            new ExpertClassEstimation
                                { Expert = erik, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.One },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.None,
                                MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.None,
                                MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.None,
                                MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.None,
                                MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.None,
                                MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                            },
                            new ExpertClassEstimation
                            {
                                Expert = roy, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.None,
                                MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven,
                                MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Six,
                                MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Seven
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Six
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Four,
                                MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Two,
                                MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Three
                            },
                            new ExpertClassEstimation
                            {
                                Expert = andries, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.One,
                                MinEstimation = ProbabilityClass.One, MaxEstimation = ProbabilityClass.Two
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Five
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five,
                                MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four,
                                MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Four,
                                MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Three,
                                MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Four
                            },
                            new ExpertClassEstimation
                            {
                                Expert = dirk, HydrodynamicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Three,
                                MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four
                            }
                        }
                    }
                }
            };

            var eventTree = new EventTree { MainTreeEvent = mainTreeEvent };

            return new ForestAnalysis
            {
                Name = "AGK - HHNK",
                ProjectLeader =
                {
                    Email = "g.deVries@hhnk.nl",
                    Name = "Goaitske de Vries"
                },
                AssessmentSection = "12-3",
                EventTrees =
                {
                    eventTree
                },
                ProbabilityEstimationsPerTreeEvent =
                {
                    probabilityEstimation1
                }
            };
        }
    }
}