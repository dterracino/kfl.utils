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
            Func<int, int, int> min = Min<int>;
            Func<double, double, double> max = Max<double>;
            Assert.Equal(1, min(1, 2));
            Assert.Equal(2.0, max(2.0, -1));
        }
    }
}
