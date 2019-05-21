using System;
using System.Collections.Generic;
using HttpUtils;
using NUnit.Framework;
using NUnit.Framework.Interfaces;

namespace UnitTests
{
    public class TokenCacheTests
    {
        private static DateTime _utcNow = DateTime.UtcNow;
        
        [Test, TestCaseSource(nameof(IsUsableTokenTestCases))]
        public static bool IsUsableTests(DateTime validToUtc, DateTime comparisonInstant, TimeSpan buffer)
            => TokenCache.IsUsableToken(validToUtc, comparisonInstant, buffer);

        public static IEnumerable<ITestCaseData> IsUsableTokenTestCases()
        {
            var twoMinuteBuffer = TimeSpan.FromMinutes(2);
            var tomorrow = _utcNow.AddDays(1);
            var twoMinutesFromNow = _utcNow.AddMinutes(2);
            var twoMinutesAgo = _utcNow.Subtract(TimeSpan.FromMinutes(2));
            
            yield return new TestCaseData(tomorrow, _utcNow, twoMinuteBuffer)
                .SetName("Token expiring tomorrow is usable")
                .Returns(true);
            
            yield return new TestCaseData(_utcNow, twoMinutesAgo, twoMinuteBuffer)
                .SetName("Token that expired two minutes ago is not usable")
                .Returns(false);
            
            yield return new TestCaseData(_utcNow.AddMinutes(1), _utcNow, twoMinuteBuffer)
                .SetName("Token expiring in one minute with a 2 minute buffer is not usable")
                .Returns(false);
        }
    }
}