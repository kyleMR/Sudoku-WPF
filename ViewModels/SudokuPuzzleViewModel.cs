using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public enum SolutionState
    {
        Correct,
        Incorrect,
        Incomplete,
    }

    public class SudokuPuzzleViewModel : BaseViewModel
    {
        private const int numDigits = 9;

        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();

        private List<CellViewModel> selectedCells = new List<CellViewModel>();

        private CellViewModel lastSelectedCell;

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
                        IsLockedDigit = false,
                    };
                    Cells.Add(cell);
                }
            }
        }

        private CellViewModel GetCell(int row, int col)
        {
            return Cells[row * numDigits + col];
        }

        public void MoveLeft(bool add = false)
        {
            if (lastSelectedCell != null)
            {
                var newCol = modulo(lastSelectedCell.Column - 1, numDigits);
                var select = GetCell(lastSelectedCell.Row, newCol);
                SelectCell(select, add);
            }
            else
            {
                var select = GetCell(0, numDigits - 1);
                SelectCell(select);
            }
        }
        
        public void MoveRight(bool add = false)
        {
            if (lastSelectedCell != null)
            {
                var newCol = modulo(lastSelectedCell.Column + 1, numDigits);
                var select = GetCell(lastSelectedCell.Row, newCol);
                SelectCell(select, add);
            }
            else
            {
                var select = GetCell(0, 0);
                SelectCell(select);
            }
        }

        public void MoveUp(bool add = false)
        {
            if (lastSelectedCell != null)
            {
                var newRow = modulo(lastSelectedCell.Row - 1, numDigits);
                var select = GetCell(newRow, lastSelectedCell.Column);
                SelectCell(select, add);
            }
            else
            {
                var select = GetCell(numDigits - 1, 0);
                SelectCell(select);
            }
        }

        public void MoveDown(bool add = false)
        {
            if (lastSelectedCell != null)
            {
                var newRow = modulo(lastSelectedCell.Row + 1, numDigits);
                var select = GetCell(newRow, lastSelectedCell.Column);
                SelectCell(select, add);
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
                if (cell.IsLockedDigit)
                    continue;
                cell.Digit = value;
            }
        }

        public void ClearCell()
        {
            foreach (var cell in selectedCells)
            {
                if (cell.IsLockedDigit)
                    continue;
                if (cell.Digit != 0)
                {
                    cell.Digit = 0;
                }
                else
                {
                    cell.ClearPencilMarks();
                }
            }
        }
        
        public void ToggleOuterMark(int digit)
        {
            foreach (var cell in selectedCells)
            {
                if (cell.IsLockedDigit)
                    continue;
                cell.ToggleOuterMark(digit);
            }
        }

        public void ToggleCenterMark(int digit)
        {
            foreach (var cell in selectedCells)
            {
                if (cell.IsLockedDigit)
                    continue;
                cell.ToggleCenterMark(digit);
            }
        }

        public void SelectCell(CellViewModel cell, bool add = false)
        {
            if (!add)
            {
                DeselectAllCells();
            }

            lastSelectedCell = cell;

            if (!cell.IsHighlighted)
            {
                cell.IsHighlighted = true;
                selectedCells.Add(cell);
            }
        }
        
        public void SelectCell(int row, int column, bool add = false)
        {
            SelectCell(GetCell(row, column), add);
        }

        public void DeselectCell(CellViewModel cell)
        {
            selectedCells.Remove(cell);
            cell.IsHighlighted = false;
            if (lastSelectedCell == cell)
            {
                lastSelectedCell = selectedCells.Count > 0 ? selectedCells[selectedCells.Count - 1] : null;
            }
        }

        public void DeselectAllCells()
        {
            foreach (var cell in selectedCells)
            {
                cell.IsHighlighted = false;
            }
            selectedCells.Clear();
            lastSelectedCell = null;
        }

        public void ToggleCell(CellViewModel cell)
        {
            if (cell.IsHighlighted)
            {
                DeselectCell(cell);
            }
            else
            {
                SelectCell(cell, true);
            }
        }

        public void ToggleCell(int row, int column)
        {
            ToggleCell(GetCell(row, column));
        }

        public void LockDigits()
        {
            foreach (var cell in Cells)
            {
                if (cell.Digit != 0)
                {
                    cell.IsLockedDigit = true;
                }
            }
        }

        public void UnlockDigits()
        {
            foreach (var cell in Cells)
            {
                cell.IsLockedDigit = false;
            }
        }

        public SolutionState CheckSolution()
        {
            foreach (var cell in Cells)
            {
                if (cell.Digit == 0)
                {
                    return SolutionState.Incomplete;
                }

                // Check other cells in the same row
                for (int col = 0; col < numDigits; col++)
                {
                    if (col == cell.Column) continue;
                    var otherCell = GetCell(cell.Row, col);
                    if (otherCell.Digit == cell.Digit)
                    {
                        return SolutionState.Incorrect;
                    }
                }

                // Check other cells in the same column
                for (int row = 0; row < numDigits; row++)
                {
                    if (row == cell.Row) continue;
                    var otherCell = GetCell(row, cell.Column);
                    if (otherCell.Digit == cell.Digit)
                    {
                        return SolutionState.Incorrect;
                    }
                }

                // Check the other cells in the same nonet
                int nonetRow = cell.Row / 3;
                int nonetCol = cell.Column / 3;
                for (int row = 0; row < 3; row++)
                {
                    for (int col = 0; col < 3; col++)
                    {
                        int realRow = nonetRow * 3 + row;
                        int realCol = nonetCol * 3 + col;
                        if (cell.Row == realRow && cell.Column == realCol) continue;
                        var otherCell = GetCell(realRow, realCol);
                        if (otherCell.Digit == cell.Digit)
                        {
                            return SolutionState.Incorrect;
                        }
                    }
                }
            }

            // If none of the checks failed for any cell, the puzzle is correct
            return SolutionState.Correct;
        }

        private static int modulo(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
