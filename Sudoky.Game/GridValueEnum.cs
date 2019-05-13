using System;

namespace GEB.Sudoku
{
    [Flags]
    public enum GridValueEnum
    {
        Blank = 1 << 0,
        Digit_1 = 1 << 1,
        Digit_2 = 1 << 2,
        Digit_3 = 1 << 3,
        Digit_4 = 1 << 4,
        Digit_5 = 1 << 5,
        Digit_6 = 1 << 6,
        Digit_7 = 1 << 7,
        Digit_8 = 1 << 8,
        Digit_9 = 1 << 9
    }
}
