namespace test
{
    using Kfl.Utils.Reporting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;
    using Xunit.Abstractions;

    public class TablizerTests
    {
        private ITestOutputHelper output;

        public TablizerTests(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void TestTablizer()
        {
            List<string> buf = new List<string>();
            Action<string> addToBuf = _ => buf.Add(_);
            Tablizer.Tablize(new Bar[] { new Bar { Prop1 = 1, Prop2 = "Hi", BarProp = "Bar" } }).ToList().ForEach(addToBuf);
            Tablizer.Tablize(Enumerable.Range(0, 10).Select(i => new { A = i, B = i * i }), "B-").ToList().ForEach(addToBuf);
            List<string> expected = new List<string>()
            {
                "       BarProp     Prop2     Prop1",
                "           Bar        Hi         1",
                " AB-  ",
                " 00   ",
                " 11   ",
                " 24   ",
                " 39   ",
                " 416  ",
                " 525  ",
                " 636  ",
                " 749  ",
                " 864  ",
                " 981  ",
            };

            Assert.Equal(expected, buf);
        }

        class Foo
        {
            public int Prop1;
            public string Prop2 { get; set; }
            private int Prop3;
        }

        class Bar : Foo
        {
            public string BarProp { get; set; }
        }
    }
}
