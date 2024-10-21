// Copyright (C) Stichting Deltares 2017. All rights reserved.
//
// This file is part of Ringtoets.
//
// Ringtoets is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System.Windows.Media;

namespace StoryTree.Storage
{
    /// <summary>
    ///     Class that contains extension methods for <see cref="Color" /> to convert them to
    ///     other value types.
    /// </summary>
    internal static class ColorConversionExtensions
    {
        public static string ToHexString(this Color value)
        {
            return "#" +
                   value.A.ToString("X2") +
                   value.R.ToString("X2") +
                   value.G.ToString("X2") +
                   value.B.ToString("X2");
        }

        public static Color ToColor(this string value)
        {
            var color = ColorConverter.ConvertFromString(value);
            if (color == null)
                return Colors.Transparent;
            return (Color)color;
        }
    }
}