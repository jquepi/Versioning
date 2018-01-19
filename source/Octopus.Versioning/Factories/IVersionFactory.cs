﻿using System;
using System.Collections.Generic;
using Octopus.Core.Util;

namespace Octopus.Versioning.Factories
{
    /// <summary>
    /// Defines a service that can be used to create specific implementations
    /// of a version
    /// </summary>
    public interface IVersionFactory
    {
        /// <summary>
        /// Parses a version string based on the FeedType
        /// </summary>
        /// <param name="input">The version string</param>
        /// <param name="type">The feed type</param>
        /// <returns>An impletation of IVersion that matches the supplied feed</returns>
        IVersion CreateVersion(string input, VersionFormat format);
        /// <summary>
        /// Parses a version string based on the package id
        /// </summary>
        /// <param name="input">The version string</param>
        /// <param name="packageId">The feed type</param>
        /// <returns>An impletation of IVersion that matches the supplied feed</returns>
        IVersion CreateVersion(string input, string packageId);
        Maybe<IVersion> CreateOptionalVersion(string input, VersionFormat format);
        IVersion CreateMavenVersion(string input);
        IVersion CreateSemanticVersion(string input, bool preserveMissingComponents = false);
        IVersion CreateSemanticVersion(int major, int minor, int patch, string releaseLabel);
        IVersion CreateSemanticVersion(int major, int minor, int patch);
        IVersion CreateSemanticVersion(int major, int minor, int patch, int revision);
        IVersion CreateSemanticVersion(Version version, string releaseLabel = null, string metadata = null);
        IVersion CreateSemanticVersion(Version version, IEnumerable<string> releaseLabels, string metadata, string originalVersion);
        Maybe<IVersion> CreateSemanticVersionOrNone(string input, bool preserveMissingComponents = false);
        bool TryCreateVersion(string input, out IVersion version, VersionFormat type);
        bool TryCreateVersion(string input, out IVersion version, string packageId); 
        bool TryCreateMavenVersion(string input, out IVersion version);
        bool TryCreateSemanticVersion(string input, out IVersion version, bool preserveMissingComponents = false);
    }
}