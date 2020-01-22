using System.Runtime.InteropServices;

namespace SudokuSolverForm
{
	public static class Utils
	{
		[DllImport("kernel32.dll")]
		public static extern short QueryPerformanceCounter(ref long x);

		[DllImport("kernel32.dll")]
		public static extern short QueryPerformanceFrequency(ref long x);

		public static byte CharToByte(char c)
		{
			switch (c)
			{
				case '1': return 1;
				case '2': return 2;
				case '3': return 3;
				case '4': return 4;
				case '5': return 5;
				case '6': return 6;
				case '7': return 7;
				case '8': return 8;
				case '9': return 9;
				default: return 0;
			}
		}
	}
}