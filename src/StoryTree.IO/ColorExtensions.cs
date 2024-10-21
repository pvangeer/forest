using System.Drawing;

namespace StoryTree.IO
{
    public static class ColorExtensions
    {
        public static string ToHexValue(this Color color)
        {
            return color.A.ToString("X2") + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public static string ToSimpleHexValue(this Color color)
        {
            return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}