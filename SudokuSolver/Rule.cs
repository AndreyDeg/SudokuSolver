using System.Collections.Generic;

namespace SudokuSolver
{
	internal class Rule
	{
		public List<Cell> cells = new List<Cell>();
		public NumberType mask;

		public bool CheckVariant()
		{
			bool result = false;

			if (mask != NumberType.all)
				for (byte i = 1; i <= 9; i++)
				{
					var number = (NumberType)(1 << i - 1);
					if ((mask & number) > 0) continue;
					int count = 0;
					Cell _cell = null;
					foreach (var cell in cells)
					{
						if ((cell.variant & number) == 0) continue;
						if (count++ != 0) break;
						_cell = cell;
					}
					if (count != 1) continue;
					_cell.SetValue(i);
					result = true;
				}

			return result;
		}
	}
}