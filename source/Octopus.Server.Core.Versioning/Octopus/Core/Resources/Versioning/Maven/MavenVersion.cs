﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Octopus.Core.Resources.Versioning.Maven
{
    public class MavenVersion : IVersion
    {
        readonly string originalVersion;
        
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public int Revision { get; }

        public bool IsPrerelease => ReleaseLabels.Any(label => label == "SNAPSHOT");

        public IEnumerable<string> ReleaseLabels { get; }

        public string Release
        {
            get
            {
                if (ReleaseLabels != null)
                {
                    return String.Join(".", ReleaseLabels);
                }

                return string.Empty;
            }
        }

        public string Metadata => null;
        public bool HasMetadata => false;
        public object ToType(Type type)
        {
            if (type == typeof(string))
            {
                return originalVersion;
            }

            throw new ArgumentException("Invalid type supplied to MavenVersion.ToType()");
        }

        public MavenVersion(int major, int minor, int patch, int revision, IEnumerable<string> releaseLabels, string originalVersion)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Revision = revision;
            ReleaseLabels = releaseLabels ?? Enumerable.Empty<string>();
            this.originalVersion = originalVersion;
        }

        public int CompareTo(object obj)
        {
            return new ComparableVersion(originalVersion)
                .CompareTo(new ComparableVersion((obj as MavenVersion)?.originalVersion ?? ""));
        }

        public override string ToString()
        {
            return originalVersion;
        } 
    }
}