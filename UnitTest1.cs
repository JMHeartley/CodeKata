using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CodeKata
{
    public class UnitTest1
    {
        public static int CompareVersions(string version1, string version2)
        {
            var version1Parts = VerifyVersionFormat(version1);
            var version2Parts = VerifyVersionFormat(version2);

            var maxPartCount = Math.Max(version1Parts.Count, version2Parts.Count);
            for (var partIndex = 0; partIndex <= maxPartCount; partIndex++)
            {
                var partCompareResult = CompareParts(partIndex, version1Parts, version2Parts);
                if (partCompareResult != 0)
                {
                    return partCompareResult;
                }
            }

            return 0;
        }

        private static int CompareParts(int partIndex, List<int> version1Parts, List<int> version2Parts)
        {
            var version1Part = 0;
            var version2Part = 0;
            if (version1Parts.Count >= partIndex + 1)
            {
                version1Part = version1Parts[partIndex];
            }
            if (version2Parts.Count >= partIndex + 1)
            {
                version2Part = version2Parts[partIndex];
            }

            if (version1Part < version2Part)
            {
                return -1;
            }
            if (version1Part > version2Part)
            {
                return 1;
            }
            return 0;
        }

        private static List<int> VerifyVersionFormat(string version)
        {
            var versionPartsAsStrings = version.Split(".");
            var versionPartsAsInts = new List<int>();
            const int partNumberLimit = 5;

            var isFormatValid = false;
            foreach (var part in versionPartsAsStrings)
            {
                isFormatValid = int.TryParse(part, out var partAsInt);
                versionPartsAsInts.Add(partAsInt);
            }

            if (!isFormatValid
                || versionPartsAsInts.Count > partNumberLimit
                || versionPartsAsInts.Any(part => part < 0)
                || versionPartsAsInts.All(part => part == 0))
            {
                throw new ArgumentException("One of the given version numbers is not in a valid format");
            }

            return versionPartsAsInts;
        }

        [Theory]
        [InlineData("1", "2", -1)]
        [InlineData("2", "1", 1)]
        [InlineData("2", "2", 0)]
        [InlineData("2", "2.0.0.0", 0)]
        [InlineData("2", "2.0.1", -1)]
        [InlineData("2", "2.0.0.1", -1)]
        [InlineData("2.0.0.1", "2.0.0.2", -1)]
        [InlineData("2.0.0.0.1", "2.0.0.0.2", -1)]
        [InlineData("2.0.1", "2", 1)]
        [InlineData("2.0.0.1", "2", 1)]
        [InlineData("2.0.0.2", "2.0.0.1", 1)]
        [InlineData("2.0.0.0.2", "2.0.0.0.1", 1)]
        [InlineData("2", "2.1", -1)]
        [InlineData("2.1", "2", 1)]
        public void CompareVersions_GetsValidData_ReturnsExpectResult(string version1, string version2, int expected)
        {
            var actual = CompareVersions(version1, version2);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("invalidVersion", "1")]
        [InlineData("1", "invalidVersion")]
        [InlineData("1", "1.0.0.0.0.0")]
        [InlineData("1.0.0.0.0.0", "1")]
        [InlineData("1", "-1")]
        [InlineData("-1", "1")]
        [InlineData("0", "0.0.0")]
        public void CompareVersions_GetsInvalidData_ThrowsArgumentException(string version1, string version2)
        {
            Assert.Throws<ArgumentException>(() => CompareVersions(version1, version2));
        }
    }
}
