using Xunit.Sdk;
using Xunit.v3;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Tools
{
    public class PriorityOrderer : ITestMethodOrderer
    {
        public IReadOnlyCollection<TTestMethod?> OrderTestMethods<TTestMethod>
            (IReadOnlyCollection<TTestMethod?> testMethods) where TTestMethod
            : ITestMethod
        {
            var sortedMethods = testMethods.OrderBy(tm =>
                tm is IXunitTestMethod xunitTestMethod
                    ? (xunitTestMethod.Method
                        .GetMatchingCustomAttributes(typeof(TestPriorityAttribute))
                        .FirstOrDefault() as TestPriorityAttribute)?.Priority ?? 0
                    : 0);
            return sortedMethods.ToArray();
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        public int Priority { get; }
        public TestPriorityAttribute(int priority)
            => Priority = priority;
    }
}
