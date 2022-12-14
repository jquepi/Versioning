// Based on IVersionComparer from https://github.com/NuGet/NuGet.Client
// NuGet is licensed under the Apache license: https://github.com/NuGet/NuGet.Client/blob/dev/LICENSE.txt

using System;
using System.Collections.Generic;

namespace Octopus.Versioning
{
    /// <summary>
    /// IVersionComparer represents a version comparer capable of sorting and determining the equality of
    /// SemanticVersion objects.
    /// </summary>
    public interface IVersionComparer : IEqualityComparer<IVersion?>, IComparer<IVersion?>
    {
    }
}