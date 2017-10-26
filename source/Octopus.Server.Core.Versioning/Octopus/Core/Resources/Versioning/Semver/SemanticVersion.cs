﻿// This class is based on NuGet's NuGetVersion class from https://github.com/NuGet/NuGet.Client
// NuGet is licensed under the Apache license: https://github.com/NuGet/NuGet.Client/blob/dev/LICENSE.txt

using System;
using System.Collections.Generic;
using System.Linq;
using NuGet.Versioning;

namespace Octopus.Core.Resources.Versioning.Semver
{

    /// <summary>
    /// A hybrid implementation of SemVer that supports semantic versioning as described at http://semver.org while
    /// not strictly enforcing it to
    /// allow older 4-digit versioning schemes to continue working.
    /// </summary>
    public class SemanticVersion : StrictSemanticVersion
    {
        static readonly SemVerFactory SemVerFactory = new SemVerFactory();
        static readonly ISemanticVersionUtils utils = new SemanticVersionUtils();
        readonly string _originalString;
        
        /// <summary>
        /// This is used by the JSON deserialisation to to pass a SemanticVersion
        /// to the constructor of a complex object.
        /// </summary>
        /// <param name="versionString">String represnetation of the version</param>
        /// <returns>The SemanticVersion parsed from the supplied string</returns>
        public static explicit operator SemanticVersion(string versionString)
        {
            return SemVerFactory.CreateVersion(versionString);
        }
        
        /// <summary>
        /// Creates a NuGetVersion using NuGetVersion.Parse(string)
        /// </summary>
        /// <param name="version">Version string</param>
        public SemanticVersion(string version)
            : this(SemVerFactory.CreateVersion(version))
        {
        }
        
        /// <summary>
        /// Creates a NuGetVersion from an existing NuGetVersion
        /// </summary>
        public SemanticVersion(SemanticVersion version)
            : this(version.Version, version.ReleaseLabels, version.Metadata, version.ToString())
        {
        }

        /// <summary>
        /// Creates a NuGetVersion from a .NET Version
        /// </summary>
        /// <param name="version">Version numbers</param>
        /// <param name="releaseLabel">Prerelease label</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(Version version, string releaseLabel = null, string metadata = null)
            : this(version, 
                utils.ParseReleaseLabels(releaseLabel), 
                metadata, 
                utils.GetLegacyString(version, utils.ParseReleaseLabels(releaseLabel), metadata))
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        public SemanticVersion(int major, int minor, int patch)
            : this(major, minor, patch, Enumerable.Empty<string>(), null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabel">Prerelease label</param>
        public SemanticVersion(int major, int minor, int patch, string releaseLabel)
            : this(major, minor, patch, utils.ParseReleaseLabels(releaseLabel), null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha#build01
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabel">Prerelease label</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(int major, int minor, int patch, string releaseLabel, string metadata)
            : this(major, minor, patch, utils.ParseReleaseLabels(releaseLabel), metadata)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion X.Y.Z-alpha.1.2#build01
        /// </summary>
        /// <param name="major">X.y.z</param>
        /// <param name="minor">x.Y.z</param>
        /// <param name="patch">x.y.Z</param>
        /// <param name="releaseLabels">Prerelease labels</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(int major, int minor, int patch, IEnumerable<string> releaseLabels, string metadata)
            : this(new Version(major, minor, patch, 0), releaseLabels, metadata, null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion W.X.Y.Z
        /// </summary>
        /// <param name="major">W.x.y.z</param>
        /// <param name="minor">w.X.y.z</param>
        /// <param name="patch">w.x.Y.z</param>
        /// <param name="revision">w.x.y.Z</param>
        public SemanticVersion(int major, int minor, int patch, int revision)
            : this(major, minor, patch, revision, Enumerable.Empty<string>(), null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion W.X.Y.Z-alpha#build01
        /// </summary>
        /// <param name="major">W.x.y.z</param>
        /// <param name="minor">w.X.y.z</param>
        /// <param name="patch">w.x.Y.z</param>
        /// <param name="revision">w.x.y.Z</param>
        /// <param name="releaseLabel">Prerelease label</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(int major, int minor, int patch, int revision, string releaseLabel, string metadata)
            : this(major, minor, patch, revision, utils.ParseReleaseLabels(releaseLabel), metadata)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion W.X.Y.Z-alpha.1#build01
        /// </summary>
        /// <param name="major">W.x.y.z</param>
        /// <param name="minor">w.X.y.z</param>
        /// <param name="patch">w.x.Y.z</param>
        /// <param name="revision">w.x.y.Z</param>
        /// <param name="releaseLabels">Prerelease labels</param>
        /// <param name="metadata">Build metadata</param>
        public SemanticVersion(int major, int minor, int patch, int revision, IEnumerable<string> releaseLabels, string metadata)
            : this(new Version(major, minor, patch, revision), releaseLabels, metadata, null)
        {
        }

        /// <summary>
        /// Creates a NuGetVersion from a .NET Version with additional release labels, build metadata, and a
        /// non-normalized version string.
        /// </summary>
        /// <param name="version">Version numbers</param>
        /// <param name="releaseLabels">prerelease labels</param>
        /// <param name="metadata">Build metadata</param>
        /// <param name="originalVersion">Non-normalized original version string</param>
        /// <param name="preserveMissingComponents">Indicates whether to normalize to semantic version</param>
        public SemanticVersion(Version version, IEnumerable<string> releaseLabels, string metadata, string originalVersion, bool preserveMissingComponents = false)
            : base(version, releaseLabels, metadata, preserveMissingComponents)
        {
            _originalString = originalVersion;
        }

        /// <summary>
        /// Returns the version string.
        /// </summary>
        /// <remarks>This method includes legacy behavior. Use ToNormalizedString() instead.</remarks>
        public override string ToString()
        {
            if (string.IsNullOrEmpty(_originalString))
            {
                return ToNormalizedString();
            }

            return _originalString;
        }

        /// <summary>
        /// A System.Version representation of the version without metadata or release labels.
        /// </summary>
        public Version Version => _version;

        /// <summary>
        /// True if the NuGetVersion is using legacy behavior.
        /// </summary>
        public bool IsLegacyVersion => Version.Revision > 0;

        /// <summary>
        /// Returns true if version is a SemVer 2.0.0 version
        /// </summary>
        public bool IsSemVer2 => this.ReleaseLabels.Count() > 1 || this.HasMetadata;

        public override object ToType(Type type)
        {
            if (type.IsAssignableFrom(typeof(NuGetVersion)))
            {
                return new NuGetVersion(Version, ReleaseLabels, Metadata, _originalString);
            }
            
            throw new InvalidCastException();
        } 
    }
}