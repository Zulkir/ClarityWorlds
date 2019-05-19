using System;
using System.Text.RegularExpressions;
using Clarity.Common.Infra.TreeReadWrite.Serialization;

namespace Clarity.Core.AppCore.Infra
{
    [TrwSerialize]
    public struct AppVersion : IEquatable<AppVersion>, IComparable<AppVersion>
    {
        public static AppVersion Current { get; } = new AppVersion(0, 10, 0);
        public static AppVersion Zero { get; } = default(AppVersion);

        [TrwSerialize] public int Major;
        [TrwSerialize] public int Minor;
        [TrwSerialize] public int Patch;

        public AppVersion(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public int CompareTo(AppVersion other)
        {
            var majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;
            var minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;
            return Patch.CompareTo(other.Patch);
        }

        public bool Equals(AppVersion other) => Major == other.Major && Minor == other.Minor && Patch == other.Patch;
        public override bool Equals(object obj) => obj is AppVersion && Equals((AppVersion)obj);
        public override int GetHashCode() => (Major << 24) + (Minor << 12) + Patch;
        public override string ToString() => $"{Major}.{Minor}.{Patch}";

        public static bool operator ==(AppVersion first, AppVersion second) => first.Equals(second);
        public static bool operator !=(AppVersion first, AppVersion second) => !(first == second);

        private static readonly Regex VersionRegex = new Regex(@"^(\d+)\.(\d+)\.(\d+)$");

        public static bool TryParse(string str, out AppVersion version)
        {
            var match = VersionRegex.Match(str);
            if (!match.Success)
            {
                version = default(AppVersion);
                return false;
            }
            var major = int.Parse(match.Groups[1].Value);
            var minor = int.Parse(match.Groups[2].Value);
            var patch = int.Parse(match.Groups[3].Value);
            version = new AppVersion(major, minor, patch);
            return true;
        }

        public static AppVersion Parse(string str)
        {
            if (!TryParse(str, out var version))
                throw new FormatException($"Failed to parse '{str}' as an AppVersion.");
            return version;
        }
    }
}