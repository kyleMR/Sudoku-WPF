using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class SudokuPuzzleViewModel : BaseViewModel
    {
        private const int numDigits = 9;

        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();

        private List<CellViewModel> selectedCells = new List<CellViewModel>();

        public SudokuPuzzleViewModel()
        {
            for (int y = 0; y < numDigits; y++)
            {
                for (int x = 0; x < numDigits; x++)
                {
                    var cell = new CellViewModel()
                    {
                        Column = x,
                        Row = y,
                        Digit = 0,
                        IsGivenDigit = false,
                    };
                    Cells.Add(cell);
                }
            }
        }

        private CellViewModel GetCell(int row, int col)
        {
            return Cells[row * numDigits + col];
        }

        public void MoveLeft()
        {
            if (selectedCells.Count > 0)
            {
                var headCell = selectedCells[selectedCells.Count - 1];
                var newCol = modulo(headCell.Column - 1, numDigits);
                var select = GetCell(headCell.Row, newCol);
                SelectCell(select);
            }
            else
            {
                var select = GetCell(0, numDigits - 1);
                SelectCell(select);
            }
        }
        
        public void MoveRight()
        {
            if (selectedCells.Count > 0)
            {
                var headCell = selectedCells[selectedCells.Count - 1];
                var newCol = modulo(headCell.Column + 1, numDigits);
                var select = GetCell(headCell.Row, newCol);
                SelectCell(select);
            }
            else
            {
                var select = GetCell(0, 0);
                SelectCell(select);
            }
        }

        public void MoveUp()
        {
            if (selectedCells.Count > 0)
            {
                var headCell = selectedCells[selectedCells.Count - 1];
                var newRow = modulo(headCell.Row - 1, numDigits);
                var select = GetCell(newRow, headCell.Column);
                SelectCell(select);
            }
            else
            {
                var select = GetCell(numDigits - 1, 0);
                SelectCell(select);
            }
        }

        public void MoveDown()
        {
            if (selectedCells.Count > 0)
            {
                var headCell = selectedCells[selectedCells.Count - 1];
                var newRow = modulo(headCell.Row + 1, numDigits);
                var select = GetCell(newRow, headCell.Column);
                SelectCell(select);
            }
            else
            {
                var select = GetCell(0, 0);
                SelectCell(select);
            }
        }

        public void SetDigit(int value)
        {
            foreach (var cell in selectedCells)
            {
                if (cell.IsGivenDigit)
                    continue;
                cell.Digit = value;
            }
        }

        public void SelectCell(CellViewModel cell, bool replace = true)
        {
            if (replace)
            {
                DeselectCells();
            }
            else
            {
                if (cell.IsHighlighted)
                {
                    return;
                }
            }
            cell.IsHighlighted = true;
            selectedCells.Add(cell);
        }
        
        public void SelectCell(int row, int column, bool replace = true)
        {
            SelectCell(GetCell(row, column), replace);
        }

        public void DeselectCells()
        {
            foreach (var cell in selectedCells)
            {
                cell.IsHighlighted = false;
            }
            selectedCells.Clear();
        }

        private static int modulo(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
