namespace test
{
    using Kfl.Utils.Reporting;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    public class ProgressTests
    {
        private ITestOutputHelper output;
        
        public ProgressTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestProgress()
        {
            List<int> x = new List<int>() { 1, 2, 3, 4, 5 };
            List<string> buf = new List<string>();

            x.ShowProgress(x.Count, _ => buf.Add(_)).ShowProgress(x.Count, output.WriteLine).ToList();

            var expected = new[]
            {
                "1/5 (20.00 %)",
                "2/5 (40.00 %)",
                "3/5 (60.00 %)",
                "4/5 (80.00 %)",
                "5/5 (100.00 %)",
            };

            Assert.Equal(expected, buf);
        }
    }
}
