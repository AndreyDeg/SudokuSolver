namespace SudokuSolver
{
	internal class Cell
	{
		public byte value;
		public NumberType variant;
		public Rule[] rules;

		public Cell(params Rule[] _rules)
		{
			rules = _rules;
			foreach (var rule in rules)
				rule.cells.Add(this);
		}

		internal bool CheckVariant()
		{
			if (value > 0 || (variant & variant - 1) > 0)
				return false;

			byte number = (byte)Utils.number_of_bits((uint)variant);
			SetValue(number);
			return true;
		}

		public void SetValue(byte v)
		{
			value = v;
			variant = (NumberType)(1 << v - 1);
			foreach (var rule in rules)
			{
				rule.mask |= variant;
				var cells = rule.cells;
				for (int j = 0; j < cells.Count; j++)
					if (cells[j].value == 0)
						cells[j].variant &= ~variant;
			}
		}
	}
}