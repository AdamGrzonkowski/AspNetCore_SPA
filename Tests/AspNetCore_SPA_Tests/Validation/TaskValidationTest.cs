using AspNetCore_SPA_Tests.Builders;
using AspNetCore_SPA_Tests.Helpers;
using Entities.Tasks;
using NUnit.Framework;

namespace AspNetCore_SPA_Tests.Validation
{
    [TestFixture]
    public class TaskValidationTest
    {
        [Test]
        public void ModelValidationTest()
        {
            var model = new TaskBuilder().WithName("SomeName").Build();

            bool isValid = ValidationHelper.TryValidateObject(model);

            Assert.IsTrue(isValid);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesProvider), nameof(TestCasesProvider.StringTestCases))]
        public void ValidateNameTest(string input)
        {
            var model = new TaskBuilder().WithName(input).Build();

            bool isValid = ValidationHelper.TryValidateProperty(model, model.Name, nameof(Task.Name));
            ValidationHelper.CheckValidationRules(isValid, () =>
                !string.IsNullOrWhiteSpace(model.Name) && model.Name.Length >= 2 && model.Name.Length <= 20);
        }

        [Test]
        [TestCaseSource(typeof(TestCasesProvider), nameof(TestCasesProvider.StringTestCases))]
        public void ValidateDescriptionTest(string input)
        {
            var model = new TaskBuilder().WithDescription(input).Build();

            bool isValid = ValidationHelper.TryValidateProperty(model, model.Description, nameof(Task.Description));
            ValidationHelper.CheckValidationRules(isValid, () => model.Description == null || model.Description?.Length <= 100);
        }
    }
}
