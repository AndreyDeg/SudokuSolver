using System;

namespace SudokuSolver
{
    [Flags]
    internal enum NumberType : uint
    {
        n1 = 1,
        n2 = 2,
        n3 = 4,
        n4 = 8,
        n5 = 16,
        n6 = 32,
        n7 = 64,
        n8 = 128,
        n9 = 256,
        all = 511,
    }
}
