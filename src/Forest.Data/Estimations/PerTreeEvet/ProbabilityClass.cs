using System.ComponentModel;

namespace Forest.Data.Estimations.PerTreeEvet
{
    public enum ProbabilityClass
    {
        [Description("0")]
        None = 0,

        [Description("1")]
        One = 1,

        [Description("2")]
        Two = 2,

        [Description("3")]
        Three = 3,

        [Description("4")]
        Four = 4,

        [Description("5")]
        Five = 5,

        [Description("6")]
        Six = 6,

        [Description("7")]
        Seven = 7
    }
}