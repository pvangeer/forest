using System.Windows.Media;
using StoryTree.Data;
using StoryTree.Data.Estimations;
using StoryTree.Data.Hydraulics;
using StoryTree.Data.Tree;

namespace StoryTree.Gui
{
    public static class TestDataGenerator
    {
        public static Project CreateTestViewModel()
        {
            return new Project
            {
                EventTrees =
                {
                    CreateEventTree("First event tree", 3),
                    CreateEventTree("Second event tree", 2),
                    CreateEventTree("3", 4)
                },
                Experts =
                {
                    new Expert
                    {
                        Name = "Klaas",
                        Email = "email@domein.nl",
                        Expertise = "Alles",
                        Organisation = "Eigen bedrijf",
                        Telephone = "088-3358339"
                    },
                    new Expert
                    {
                        Name = "Piet",
                        Email = "piet@email.nl",
                        Expertise = "Niets",
                        Organisation = "Ander bedrijf",
                        Telephone = "088-3358339"
                    },
                }
            };
        }

        private static EventTree CreateEventTree(string treeName, int numberTreeEvents)
        {
            var mainTreeEvent = new TreeEvent
            {
                Name = "First element"
            };

            var tree = new EventTree
            {
                Name = treeName,
                MainTreeEvent = mainTreeEvent
            };

            var currentTreeEvent = mainTreeEvent;
            for (int i = 0; i < numberTreeEvents - 1; i++)
            {
                var falseEvent = new TreeEvent
                {
                    Name = string.Format("Event no. {0}", i + 1)
                };
                currentTreeEvent.FailingEvent = falseEvent;
                currentTreeEvent = falseEvent;
            }

            return tree;
        }

        public static Project GenerateAsphalProject()
        {
            var andre = new Expert
            {
                Name = "Andre van Hoven",
                Email = "Andre.vanHoven@deltares.nl",
                Expertise = "Asfalt bekleding",
                Organisation = "Deltares",
                Telephone = ""
            };
            var erik = new Expert
            {
                Name = "Erik Vastenburg",
                Email = "e.vastenburg@hhnk.nl",
                Expertise = "Faalpaden, geotechniek",
                Organisation = "HHNK",
                Telephone = ""
            };
            var roy = new Expert
            {
                Name = "Roy Mom",
                Email = "",
                Expertise = "Bekledingen",
                Organisation = "",
                Telephone = ""
            };
            var andries = new Expert
            {
                Name = "Andries Nederpel",
                Email = "a.nederpel@hhnk.nl",
                Expertise = "",
                Organisation = "HHNK",
                Telephone = ""
            };
            var dirk = new Expert
            {
                Name = "Dirk",
                Email = "",
                Expertise = "Asfalt",
                Organisation = "HKV",
                Telephone = ""
            };

            var hydraulicCondition23 = new HydraulicCondition(2.3, (Probability)3.33E-02,double.NaN,double.NaN);
            var hydraulicCondition26 = new HydraulicCondition(2.6, (Probability)1.00E-02,double.NaN,double.NaN);
            var hydraulicCondition29 = new HydraulicCondition(2.9, (Probability)3.33E-03,double.NaN,double.NaN);
            var hydraulicCondition32 = new HydraulicCondition(3.2, (Probability)1.00E-03,double.NaN,double.NaN);
            var hydraulicCondition35 = new HydraulicCondition(3.5, (Probability)3.33E-04,double.NaN,double.NaN);
            var hydraulicCondition38 = new HydraulicCondition(3.8, (Probability)1.00E-04,double.NaN,double.NaN);

            return new Project
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
                    new EventTree
                    {
                        Name = "AGK - HHNK",
                        MainTreeEvent = new TreeEvent
                        {
                            Name = "Knoop 1",
                            ProbabilitySpecificationType = ProbabilitySpecificationType.Classes,
                            ClassesProbabilitySpecification = 
                            {
                                new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Six, MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Six},
                                new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Five, MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Six},
                                new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Six, MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Seven},
                                new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Five, MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five},
                                new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Five, MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five},
                            },
                            FailingEvent = new TreeEvent
                            {
                                Name = "Knoop 2",
                                ProbabilitySpecificationType = ProbabilitySpecificationType.Classes,
                                ClassesProbabilitySpecification =
                                {
                                    new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Six, MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Seven},
                                    new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five, MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Six},
                                    new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four, MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five},
                                    new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Three, MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four},
                                    new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Three, MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four},
                                    new ExpertClassEstimation{Expert = andre, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Three, MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four},
                                    new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Six},
                                    new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five},
                                    new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four},
                                    new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Three},
                                    new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Two},
                                    new ExpertClassEstimation{Expert = erik, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.One},
                                    new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.None, MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None},
                                    new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.None, MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None},
                                    new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.None, MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None},
                                    new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.None, MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None},
                                    new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.None, MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None},
                                    new ExpertClassEstimation{Expert = roy, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.None, MinEstimation = ProbabilityClass.None, MaxEstimation = ProbabilityClass.None},
                                    new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Seven, MinEstimation = ProbabilityClass.Seven, MaxEstimation = ProbabilityClass.Seven},
                                    new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Six, MinEstimation = ProbabilityClass.Six, MaxEstimation = ProbabilityClass.Seven},
                                    new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Five, MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Six},
                                    new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Four, MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five},
                                    new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Two, MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Three},
                                    new ExpertClassEstimation{Expert = andries, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.One, MinEstimation = ProbabilityClass.One, MaxEstimation = ProbabilityClass.Two},
                                    new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition23, AverageEstimation = ProbabilityClass.Five, MinEstimation = ProbabilityClass.Five, MaxEstimation = ProbabilityClass.Five},
                                    new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition26, AverageEstimation = ProbabilityClass.Five, MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five},
                                    new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition29, AverageEstimation = ProbabilityClass.Four, MinEstimation = ProbabilityClass.Four, MaxEstimation = ProbabilityClass.Five},
                                    new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition32, AverageEstimation = ProbabilityClass.Four, MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Five},
                                    new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition35, AverageEstimation = ProbabilityClass.Three, MinEstimation = ProbabilityClass.Three, MaxEstimation = ProbabilityClass.Four},
                                    new ExpertClassEstimation{Expert = dirk, HydraulicCondition = hydraulicCondition38, AverageEstimation = ProbabilityClass.Three, MinEstimation = ProbabilityClass.Two, MaxEstimation = ProbabilityClass.Four},
                                }
                            }
                        },
                        Color = Colors.CornflowerBlue,
                        NeedsSpecification = true
                    }
                },
                Experts =
                {
                    andre,
                    erik,
                    roy,
                    andries,
                    dirk,
                },
                HydraulicConditions =
                {
                    hydraulicCondition23,
                    hydraulicCondition26,
                    hydraulicCondition29,
                    hydraulicCondition32,
                    hydraulicCondition35,
                    hydraulicCondition38,
                }
            };
        }
    }
}
