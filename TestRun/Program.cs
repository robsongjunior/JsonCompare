using JsonCompareDiff;

namespace TestRun
{
    sealed class Program
    {
        static void Main()
        {
            string beforeJson = @"{
            ""Name"": ""John Doe"",
            ""Age"": 30,
            ""Address"": {
                ""Street"": ""123 Main St"",
                ""City"": ""Anytown"",
                ""State"": ""CA""
            },
            ""Skills"": [""C#"", ""SQL"", ""JavaScript""]
        }";

            string afterJson = @"{
            ""Name"": ""John Doe"",
            ""Age"": 31,
            ""Address"": {
                ""Street"": ""123 Main St"",
                ""City"": ""New Haven"",
                ""State"": ""NY""
            },
            ""Skills"": [""C#"", ""SQL"", ""Python""]
        }";

            List<string> ignoredKeys = new List<string> { "State" };

            var differences = JsonCompare.Compare(beforeJson, afterJson, ignoredKeys);

            var differenceObject = new Difference
            {
                Differences = differences
            };

        }

        public class Difference
        {
            public object Differences { get; set; }
        }

    }
}