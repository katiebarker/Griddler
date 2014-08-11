using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Griddler.Solver;
using Griddler.PuzzleModel;

namespace Griddler.Test
{
    [TestClass]
    public class Test1
    {
        private Puzzle Puzzle;

        [TestInitialize]
        public void TestSetUp()
        {
            Puzzle = new Puzzle(5, 5, new List<Line>
            {
                new Row(new[] {1, 1}, 0),
                new Row(new[] {5}, 1),
                new Row(new[] {1, 1}, 2),
                new Row(new[] {5}, 3),
                new Row(new[] {1, 1}, 4),
                new Column(new[] {1, 1}, 0),
                new Column(new[] {5}, 1),
                new Column(new[] {1, 1}, 2),
                new Column(new[] {5}, 3),
                new Column(new[] {1, 1}, 4)
            });

        }

        [TestMethod]
        public void TestMethod1()
        {
            Puzzle.Solve();
            int[,] expected = { { -1, 1, -1, 1, -1 }, { 1, 1, 1, 1, 1 }, { -1, 1, -1, 1, -1 }, { 1, 1, 1, 1, 1 }, { -1, 1, -1, 1, -1 } };
            
            foreach (var cell in Puzzle.Cells)
            {
                Assert.AreEqual(expected[cell.Key.X,cell.Key.Y], cell.Value);
            }

        }

        [TestMethod]
        public void TestMethod2()
        {
            var list = Puzzle.StringToList("1 , 1 , 1 ; 1 ; 2 , 3");
            var expected = new List<int[]>();
            int[] a = { 1 , 1 , 1 };
            int[] b = { 1 };
            int[] c = { 2 , 3 };
            expected.Add(a);
            expected.Add(b);
            expected.Add(c);

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Length; j++)
                {
                    var x = expected[i][j];
                    var y = list[i][j];
                    Assert.AreEqual(x,y);
                }
            }
        }
    }
}
