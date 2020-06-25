﻿using System;
using System.Collections.Generic;
using Octopus.Versioning.Docker;
using Octopus.Versioning.Maven;
using Octopus.Versioning.Semver;
using SemanticVersion = Octopus.Versioning.Semver.SemanticVersion;

namespace Octopus.Versioning
{
    public static class VersionFactory 
    {
        public static IVersion CreateVersion(string input, VersionFormat format)
        {
            switch (format)
            {
                case VersionFormat.Maven:
                    return CreateMavenVersion(input);
                case VersionFormat.Docker:
                    return CreateDockerTag(input);
                default:
                    return CreateSemanticVersion(input);
            }
        }

        public static IVersion? TryCreateVersion(string input, VersionFormat format)
        {
            switch (format)
            {
                case VersionFormat.Maven:
                    return TryCreateMavenVersion(input);
                case VersionFormat.Docker:
                    return TryCreateDockerTag(input);
                default:
                    return TryCreateSemanticVersion(input);
            }
        }

        public static IVersion CreateMavenVersion(string input)
        {
            return new MavenVersionParser().Parse(input);
        }

        public static IVersion CreateSemanticVersion(string input, bool preserveMissingComponents = false)
        {
            return SemVerFactory.CreateVersion(input, preserveMissingComponents);
        }

        public static IVersion CreateSemanticVersion(int major, int minor, int patch, string releaseLabel)
        {
            return new SemanticVersion(major, minor, patch, releaseLabel);
        }

        public static IVersion CreateSemanticVersion(int major, int minor, int patch)
        {
            return new SemanticVersion(major, minor, patch);
        }

        public static IVersion CreateSemanticVersion(int major, int minor, int patch, int revision)
        {
            return new SemanticVersion(major, minor, patch, revision);
        }

        public static IVersion CreateSemanticVersion(Version version, string? releaseLabel = null, string? metadata = null)
        {
            return new SemanticVersion(version, releaseLabel, metadata);
        }

        public static IVersion CreateSemanticVersion(int major, int minor, int patch, int revision,
            IEnumerable<string> releaseLabels,
            string metadata, string originalVersion)
        {
            return new SemanticVersion(major, minor, patch, revision, releaseLabels, metadata);
        }

        public static IVersion TryCreateMavenVersion(string input)
        {
            /*
             * Any version is valid for Maven
             */
            return new MavenVersionParser().Parse(input);
        }

        public static IVersion? TryCreateSemanticVersion(string input, bool preserveMissingComponents = false)
        {
            return SemVerFactory.TryCreateVersion(input, preserveMissingComponents);
        }

        public static IVersion? CreateSemanticVersionOrNone(string input, bool preserveMissingComponents = false)
        {
            return SemVerFactory.CreateVersionOrNone(input, preserveMissingComponents);
        }

        public static IVersion CreateSemanticVersion(Version version, IEnumerable<string> releaseLabels, string metadata,
            string originalVersion)
        {
            return new SemanticVersion(
                version,
                releaseLabels,
                metadata,
                originalVersion);
        }

        public static IVersion CreateDockerTag(string input)
        {
            return new DockerTag(input);
        }

        public static IVersion TryCreateDockerTag(string input)
        {
            return new DockerTag(input);
        }
    }
}