namespace SudokuSolver
{
	internal static class Utils
	{
		// Подсчет единичных битов в слове
		public static uint number_of_one(uint X)
		{
			X = X - (X >> 1 & 0x55555555);
			X = (X & 0x33333333) + (X >> 2 & 0x33333333);
			X = X + (X >> 4) & 0x0F0F0F0F;
			X = X + (X >> 8);
			X = X + (X >> 16);
			return X & 0x0000003F;
		}

		static readonly uint[] lut = {0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18,
			22, 25, 3, 30, 8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31};

		//Подсчет значащих битов
		public static uint number_of_bits(uint v)
		{
			v |= v >> 1;
			v |= v >> 2;
			v |= v >> 4;
			v |= v >> 8;
			v |= v >> 16;

			return lut[v * 0x07C4ACDDU >> 27] + 1;
		}
	}
}