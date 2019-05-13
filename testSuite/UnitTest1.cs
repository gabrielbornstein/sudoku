using NUnit.Framework;
using GEB.Sudoku;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        { 
        }

        [Test]
        public void Test1()
        {
            GridValueEnum[,] board1 = new GridValueEnum[,] {
                { GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3, GridValueEnum.Blank, GridValueEnum.Digit_5, GridValueEnum.Digit_6, GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9 },
                { GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9, GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3, GridValueEnum.Digit_4, GridValueEnum.Digit_5, GridValueEnum.Digit_6 },
                { GridValueEnum.Digit_4, GridValueEnum.Digit_5, GridValueEnum.Digit_6, GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9, GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3 },
                { GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3, GridValueEnum.Digit_4, GridValueEnum.Digit_5, GridValueEnum.Digit_6, GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9 },
                { GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3, GridValueEnum.Digit_4, GridValueEnum.Digit_5, GridValueEnum.Digit_6, GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9 },
                { GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3, GridValueEnum.Digit_4, GridValueEnum.Digit_5, GridValueEnum.Digit_6, GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9 },
                { GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3, GridValueEnum.Digit_4, GridValueEnum.Digit_5, GridValueEnum.Digit_6, GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9 },
                { GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3, GridValueEnum.Digit_4, GridValueEnum.Digit_5, GridValueEnum.Digit_6, GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9 },
                { GridValueEnum.Digit_1, GridValueEnum.Digit_2, GridValueEnum.Digit_3, GridValueEnum.Digit_4, GridValueEnum.Digit_5, GridValueEnum.Digit_6, GridValueEnum.Digit_7, GridValueEnum.Digit_8, GridValueEnum.Digit_9 },
            };

            Game currGame = new Game(board1);
            GridValueEnum value = currGame.GetGridValue(0, 3);
            Assert.AreEqual(value, GridValueEnum.Digit_4);
        }
    }
}