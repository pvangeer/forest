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

using System;
using System.IO;
using System.ServiceModel;
using System.Xml.Serialization;
using StoryTree.Storage.XmlEntities;

namespace StoryTree.Storage
{
    /// <summary>
    ///     This class is capable of generating a hashcode for serializable object instance
    ///     such that the hashcode can be used to detect changes.
    /// </summary>
    public static class FingerprintHelper
    {
        /// <summary>
        ///     Gets the fingerprint for the given <see cref="AnalysisXmlEntity" />.
        /// </summary>
        /// <param name="entity">The <see cref="AnalysisXmlEntity" /> to generate a hashcode for.</param>
        /// <returns>The binary hashcode for <paramref name="entity" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity" /> is <c>null</c>.</exception>
        public static int Get(EventTreeProjectXmlEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return ComputeHash(entity);
        }

        /// <summary>
        ///     While using a target file as storage, determines the fingerprint for the given
        ///     <see cref="AnalysisXmlEntity" />.
        /// </summary>
        /// <param name="entity">The <see cref="AnalysisXmlEntity" /> to generate a hashcode for.</param>
        /// <returns>The binary hashcode for <paramref name="entity" />.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="entity" /> is <c>null</c>.</exception>
        /// <exception cref="QuotaExceededException">
        ///     Thrown when <paramref name="entity" />
        ///     contains more than <see cref="int.MaxValue" /> unique object instances.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        ///     The caller does not have the
        ///     required permissions or <paramref name="filePath" /> is read-only.
        /// </exception>
        /// <exception cref="IOException">
        ///     An I/O exception occurred while creating the file
        ///     at <paramref name="filePath" />.
        /// </exception>
        private static int ComputeHash(EventTreeProjectXmlEntity entity)
        {
            var xmlSerializer = new XmlSerializer(typeof(EventTreeProjectXmlEntity));
            using (var textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, entity);
                return textWriter.ToString().GetHashCode();
            }
        }
    }
}