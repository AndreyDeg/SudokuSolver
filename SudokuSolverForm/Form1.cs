using SudokuSolver;
using System;
using System.Linq;
using System.Windows.Forms;

namespace SudokuSolverForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private byte[] ParseNums()
        {
            byte[] nums = new byte[81];
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    nums[i * 9 + j] = Utils.CharToByte(textBox1.Lines[i / 3 * 4 + i % 3][j / 3 * 4 + j % 3]);

            return nums;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] nums = ParseNums();

            long ctr1 = 0, ctr2 = 0, freq = 0;
            Utils.QueryPerformanceFrequency(ref freq);
            if (Utils.QueryPerformanceCounter(ref ctr1) != 0) // Begin timing.
            {
                var solves = new SudokuField().Solve(nums).Take(100).ToList();
                Utils.QueryPerformanceCounter(ref ctr2); // Finish timing.

                if (solves.Count >= 100)
                {
                    textBox2.Text = solves[0];
                    label1.Text = $"Всего решений более{solves.Count} за {(ctr2 - ctr1) * 1.0 / freq:0.0000}";
                }
                else if (solves.Count > 0)
                {
                    textBox2.Text = solves[0];
                    label1.Text = $"Всего решений {solves.Count} за {(ctr2 - ctr1) * 1.0 / freq:0.0000}";
                }
                else
                {
                    textBox2.Text = "";
                    label1.Text = $"Решений нет за {(ctr2 - ctr1) * 1.0 / freq:0.0000}";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] nums = ParseNums();

            long ctr1 = 0, ctr2 = 0, freq = 0;
            Utils.QueryPerformanceFrequency(ref freq);
            if (Utils.QueryPerformanceCounter(ref ctr1) != 0) // Begin timing.
            {
                var solve = new SudokuField().Solve(nums).FirstOrDefault();
                Utils.QueryPerformanceCounter(ref ctr2); // Finish timing.

                if (solve != null)
                {
                    textBox2.Text = solve;
                    label1.Text = $"Всего решений 1 за {(ctr2 - ctr1) * 1.0 / freq:0.0000}";
                }
                else
                {
                    textBox2.Text = "";
                    label1.Text = $"Решений нет за {(ctr2 - ctr1) * 1.0 / freq:0.0000}";
                }
            }
        }
    }
}