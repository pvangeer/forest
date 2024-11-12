using System;
using System.ComponentModel;
using Forest.Data.Tree;

namespace Forest.Storage
{
    public static class TreeEventTypeUtils
    {
        public static string ToStorageName(this TreeEventType type)
        {
            switch (type)
            {
                case TreeEventType.Failing:
                    return "failing";
                case TreeEventType.Passing:
                    return "passing";
                case TreeEventType.MainEvent:
                    return "main";
                default:
                    throw new InvalidEnumArgumentException(nameof(type));
            }
        }

        public static TreeEventType FromStorageName(string storageName)
        {
            switch (storageName)
            {
                case "failing":
                    return TreeEventType.Failing;
                case "passing":
                    return TreeEventType.Passing;
                case "main":
                    return TreeEventType.MainEvent;
                default:
                    throw new ArgumentException();
            }
        }
    }
}