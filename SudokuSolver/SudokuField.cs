using System;
using System.Collections.Generic;
using System.Linq;

namespace SudokuSolver
{
    public class SudokuField
    {
        private readonly List<Cell> cells = new List<Cell>();
        private readonly List<Rule> rules = new List<Rule>();

        private static readonly List<SudokuField> pool = new List<SudokuField>();

        public SudokuField()
        {
            for (int i = 0; i < 27; i++)
                rules.Add(new Rule());

            for (int m = 0; m < 9; m++)
                for (int i = 0; i < 9; i++)
                    cells.Add(new Cell(rules[m], rules[9 + i], rules[18 + (m / 3) * 3 + i / 3]));
        }

        private void Init(byte[] data)
        {
            foreach (var rule in rules)
                rule.mask = 0;

            foreach (var cell in cells)
            {
                cell.value = 0;
                cell.variant = NumberType.all;
            }

            if (data.Length != cells.Count)
                throw new ArgumentException($"data.Length: actual {data.Length}, expected {cells.Count}", "data");

            for (int i = 0; i < data.Length; i++)
                if (data[i] > 0)
                    cells[i].SetValue(data[i]);
        }

        public IEnumerable<string> Solve(byte[] data)
        {
            Init(data);

            //Проверка, что начальные числа соответствуют правилам
            foreach (var rule in rules)
            {
                int mask = 0;
                foreach (var cell in rule.cells)
                {
                    var m = 1 << cell.value - 1;
                    if ((mask & m) > 0)
                        yield break; //Правило нарушено, нет решений
                    mask |= m;
                }
            }

            foreach (var result in SolveNext())
                yield return result;
        }

        private IEnumerable<string> SolveNext()
        {
            //Решаем судоку
            bool bCheck;
            do
            {
                bCheck = false;
                //Проверяем, что в клетке возможно единственное значение
                foreach (var cell in cells) bCheck = cell.CheckVariant() || bCheck;
                //Проверяем, что по правилам осталось единственное значение
                foreach (var rule in rules) bCheck = rule.CheckVariant() || bCheck;
            } while (bCheck);

            //Проверим сколько вариантов осталось
            int j = -1;
            uint minVariantCount = uint.MaxValue;
            for (int i = 0; i < cells.Count; i++)
                if (cells[i].value == 0)
                {
                    var variantCount = Utils.number_of_one((uint)cells[i].variant);
                    if (minVariantCount > variantCount)
                    {
                        j = i;
                        minVariantCount = variantCount;
                        if (variantCount == 2) break; //Всего два варианта, их и будем смотреть
                    }
                }

            //Попробуем перебор
            if (j >= 0)
            {
                var variant = cells[j].variant;
                for (byte i = 1; i <= 9; i++)
                    if ((variant & (NumberType)(1 << i - 1)) > 0)
                    {
                        var field = Create(this);
                        field.cells[j].SetValue(i);
                        var results = field.SolveNext(); //Возможно несколько решений
                        foreach (var res in results)
                            yield return res;
                        field.Dispose();
                    }

                yield break;
            }

            //Контрольная проверка
            if (rules.Exists(rule => rule.mask != NumberType.all))
                yield break; //Правило не выполнено, нет решений

            yield return ToString(); //Одно решение
        }

        private static SudokuField Create(SudokuField copyFrom)
        {
            var field = pool.Count > 0 ? pool[pool.Count - 1] : new SudokuField();
            pool.Remove(field);
            field.Init(copyFrom.cells.Select(s => s.value).ToArray());
            return field;
        }

        private void Dispose()
        {
            pool.Add(this);
        }

        public override string ToString()
        {
            string text = "";
            for (int m = 0; m < 3; m++)
            {
                for (int n = 0; n < 3; n++)
                {
                    var t = "   |   |   |".ToCharArray();
                    for (int i = 0; i < 3; i++)
                        for (int j = 0; j < 3; j++)
                            if (cells[m * 27 + n * 9 + i * 3 + j].value > 0)
                                t[i * 4 + j] = (char)('0' + cells[m * 27 + n * 9 + i * 3 + j].value);

                    text += new string(t) + "\r\n";
                }

                if (m + 1 < 3)
                    text += "---+---+---|\r\n";
            }

            return text;
        }
    }
}