using NUnit.Framework;
using GEB.Sudoku;

namespace Tests
{
    public class Testing
    {
        [SetUp]
        public void Setup()
        { 
        }

        [Test]
        public void Test1()
        {
            GridValueEnum[,] board1 = new GridValueEnum[,] 
            {
                { GridValueEnum.Digit_5, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Digit_5, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Digit_9, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank },
                { GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_3 },
                { GridValueEnum.Digit_4, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1 },
                { GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6 },
                { GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Digit_8, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_4, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_5 },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Digit_9 },
            };

            Game currGame = new Game(board1);
            GridValueEnum value = currGame.GetGridValue(6, 8);
            Assert.AreEqual(value, GridValueEnum.Digit_3);
        }

        [Test]
        public void Test2()
        {
            GridValueEnum[,] board1 = new GridValueEnum[,]
            {
                { GridValueEnum.Digit_5, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Digit_5, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Digit_9, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank },
                { GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_3 },
                { GridValueEnum.Digit_4, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Blank, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_1 },
                { GridValueEnum.Digit_7, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_6 },
                { GridValueEnum.Blank, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_2, GridValueEnum.Digit_8, GridValueEnum.Blank },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_4, GridValueEnum.Digit_1, GridValueEnum.Digit_9, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_5 },
                { GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Blank, GridValueEnum.Digit_8, GridValueEnum.Digit_6, GridValueEnum.Blank, GridValueEnum.Digit_7, GridValueEnum.Digit_9 },
            };

            Game currGame = new Game(board1);
            GridValueEnum value = currGame.GetGridValue(6, 8);
            Assert.AreEqual(value, GridValueEnum.Digit_4);
        }
    }
}