using System.Collections.ObjectModel;

namespace AspNetCore_SPA_Tests.Helpers
{
    public static class TestCasesProvider
    {
        public static readonly ReadOnlyCollection<string> StringTestCases = new ReadOnlyCollection<string>(new[]
        {
            null,
            string.Empty,
            " ",
            "Test",
            "d",
            "dd",
            "dżi",
            "[BumTrum]",
            "SomeItemNumber",
            "Someds2323232mber",
            "sdsd@isdis/!",
            "oneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWo_100",
            "oneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWor_101",
            "oneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordOnoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWo_200",
            "oneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordOnoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWordoneHundredWor_201",
        });
    }
}
