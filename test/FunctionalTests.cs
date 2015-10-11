namespace test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using static Kfl.Utils.Functional.Functional;

    public class FunctionalTests
    {
        [Xunit.Fact]
        public void TestMinMax()
        {
            Assert.Equal(1, Min(1, 2));
            Assert.Equal(2.0, Max(2.0, -1));
        }
    }
}
