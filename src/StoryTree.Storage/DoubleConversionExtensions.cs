namespace StoryTree.Storage
{
    public static class DoubleConversionExtensions
    {
        public static double? ToNaNAsNull(this double value)
        {
            if (double.IsNaN(value))
            {
                return null;
            }
            return value;
        }

        public static double ToNullAsNaN(this double? value)
        {
            if (value == null)
            {
                return double.NaN;
            }
            return value.Value;
        }
    }
}