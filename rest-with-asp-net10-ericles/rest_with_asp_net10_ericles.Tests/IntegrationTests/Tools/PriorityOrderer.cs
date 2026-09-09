using Xunit.Sdk;
using Xunit.v3;

namespace RestWithASPNET10Erudio.Tests.IntegrationTests.Tools
{
    public class PriorityOrderer : ITestCaseOrderer
    {
        public IReadOnlyCollection<TTestCase> OrderTestCases<TTestCase>
            (IReadOnlyCollection<TTestCase> testCases) where TTestCase
            : ITestCase
        {
            var sortedMethods = testCases.OrderBy(
                tc => ((tc as IXunitTestCase)?.TestMethod.Method
                    .GetMatchingCustomAttributes(typeof(TestPriorityAttribute))
                    .FirstOrDefault() as TestPriorityAttribute)
                    ?.Priority ?? 0);
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
