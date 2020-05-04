using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    /// <summary>
    /// Puzzle solved states
    /// </summary>
    public enum SolutionState
    {
        // The puzzle is solved correctly
        Correct,
        // The puzzle has digits in invalid positions
        Incorrect,
        // The puzzle has digits missing
        Incomplete,
    }

    /// <summary>
    /// Viewmodel governing the Sudoku puzzle grid
    /// </summary>
    public class SudokuPuzzleViewModel : BaseViewModel
    {
        private const int numDigits = 9;

        /// <summary>
        /// Main collection of cell viewmodels
        /// </summary>
        public ObservableCollection<CellViewModel> Cells { get; } = new ObservableCollection<CellViewModel>();

        /// <summary>
        /// List of currently selected cells
        /// </summary>
        private List<CellViewModel> selectedCells = new List<CellViewModel>();

        /// <summary>
        /// Most recently selected cell, used as the target for keyboard movement
        /// </summary>
        private CellViewModel lastSelectedCell;

        /// <summary>
        /// Default constructor
        /// </summary>
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

        /// <summary>
        /// Convenience getter for retrieving a cell viewmodel based on row/col coordinates
        /// </summary>
        /// <param name="row">Row number of cell to retrieve</param>
        /// <param name="col">Column number of cell to retrieve</param>
        /// <returns></returns>
        private CellViewModel GetCell(int row, int col)
        {
            return Cells[row * numDigits + col];
        }

        /// <summary>
        /// Move the selection highlight to the left by one cell
        /// </summary>
        /// <param name="add">If true, the cell is added to the selection rather than replacing it</param>
        public void MoveLeft(bool add = false)
        {
            if (lastSelectedCell != null)
            {
                var newCol = Modulo(lastSelectedCell.Column - 1, numDigits);
                var select = GetCell(lastSelectedCell.Row, newCol);
                SelectCell(select, add);
            }
            else
            {
                var select = GetCell(0, numDigits - 1);
                SelectCell(select);
            }
        }
        
        /// <summary>
        /// Move the selection highlight to the right by one cell
        /// </summary>
        /// <param name="add">If true, the cell is added to the selection rather than replacing it</param>
        public void MoveRight(bool add = false)
        {
            if (lastSelectedCell != null)
            {
                var newCol = Modulo(lastSelectedCell.Column + 1, numDigits);
                var select = GetCell(lastSelectedCell.Row, newCol);
                SelectCell(select, add);
            }
            else
            {
                var select = GetCell(0, 0);
                SelectCell(select);
            }
        }

        /// <summary>
        /// Move the selection highlight up by one cell
        /// </summary>
        /// <param name="add">If true, the cell is added to the selection rather than replacing it</param>
        public void MoveUp(bool add = false)
        {
            if (lastSelectedCell != null)
            {
                var newRow = Modulo(lastSelectedCell.Row - 1, numDigits);
                var select = GetCell(newRow, lastSelectedCell.Column);
                SelectCell(select, add);
            }
            else
            {
                var select = GetCell(numDigits - 1, 0);
                SelectCell(select);
            }
        }

        /// <summary>
        /// Move the selection highlight down by one cell
        /// </summary>
        /// <param name="add">If true, the cell is added to the selection rather than replacing it</param>
        public void MoveDown(bool add = false)
        {
            if (lastSelectedCell != null)
            {
                var newRow = Modulo(lastSelectedCell.Row + 1, numDigits);
                var select = GetCell(newRow, lastSelectedCell.Column);
                SelectCell(select, add);
            }
            else
            {
                var select = GetCell(0, 0);
                SelectCell(select);
            }
        }

        /// <summary>
        /// Set the selected unlocked cells to the given digit
        /// </summary>
        /// <param name="value"></param>
        public void SetDigit(int value)
        {
            foreach (var cell in selectedCells)
            {
                if (cell.IsLockedDigit)
                    continue;
                cell.Digit = value;
            }
        }

        /// <summary>
        /// Clear the digit in selected unlocked cells. If the digit is not set, clears the pencil marks instead.
        /// </summary>
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
        
        /// <summary>
        /// Toggle the display of an outer pencil mark for the given digit on selected cells
        /// </summary>
        /// <param name="digit">Digit to set or unset as a mark</param>
        public void ToggleOuterMark(int digit)
        {
            foreach (var cell in selectedCells)
            {
                if (cell.IsLockedDigit)
                    continue;
                cell.ToggleOuterMark(digit);
            }
        }

        /// <summary>
        /// Toggle the display of a center pencil mark for the given digit on selected cells
        /// </summary>
        /// <param name="digit">Digit to set or unset as a mark</param>
        public void ToggleCenterMark(int digit)
        {
            foreach (var cell in selectedCells)
            {
                if (cell.IsLockedDigit)
                    continue;
                cell.ToggleCenterMark(digit);
            }
        }

        /// <summary>
        /// Select the given cell
        /// </summary>
        /// <param name="cell">Cell to select</param>
        /// <param name="add">If true, cell is added to selection, otherwise, it replaces the current selection</param>
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
        
        /// <summary>
        /// Select the given cell
        /// </summary>
        /// <param name="row">Row number of cell to select</param>
        /// <param name="column">Column number of cell to select</param>
        /// <param name="add">If true, cell is added to selection, otherwise, it replaces the current selection</param>
        public void SelectCell(int row, int column, bool add = false)
        {
            SelectCell(GetCell(row, column), add);
        }

        /// <summary>
        /// Deselect the given cell
        /// </summary>
        /// <param name="cell">Cell to deselect</param>
        public void DeselectCell(CellViewModel cell)
        {
            selectedCells.Remove(cell);
            cell.IsHighlighted = false;
            
            // If the deselected cell was also the most recently selected one, choose the next most recently selected cell as the new most recent one
            if (lastSelectedCell == cell)
            {
                lastSelectedCell = selectedCells.Count > 0 ? selectedCells[selectedCells.Count - 1] : null;
            }
        }

        /// <summary>
        /// Clear the selection of cells
        /// </summary>
        public void DeselectAllCells()
        {
            foreach (var cell in selectedCells)
            {
                cell.IsHighlighted = false;
            }
            selectedCells.Clear();
            lastSelectedCell = null;
        }

        /// <summary>
        /// Toggle the selection of the given cell
        /// </summary>
        /// <param name="cell">Cell to toggle selection of</param>
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

        /// <summary>
        /// Toggle the select of the given cell
        /// </summary>
        /// <param name="row">Row number of the cell to toggle</param>
        /// <param name="column">Column number of the cell to toggle</param>
        public void ToggleCell(int row, int column)
        {
            ToggleCell(GetCell(row, column));
        }

        /// <summary>
        /// Lock any cells with set digits
        /// </summary>
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

        /// <summary>
        /// Unlock all cells
        /// </summary>
        public void UnlockDigits()
        {
            foreach (var cell in Cells)
            {
                cell.IsLockedDigit = false;
            }
        }
        
        /// <summary>
        /// Get the current solution state of the puzzle
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Asynchronous version of <see cref="CheckSolution"/>
        /// </summary>
        /// <returns></returns>
        public async Task<SolutionState> CheckSolutionAsync()
        {
            return await Task.Run(CheckSolution);
        }

        /// <summary>
        /// Get a string representation of the locked digits in the current puzzle
        /// </summary>
        /// <returns></returns>
        public string CurrentPuzzleToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var cell in Cells)
            {
                if (cell.IsLockedDigit && cell.Digit != 0)
                {
                    sb.Append(cell.Digit);
                }
                else
                {
                    sb.Append("-");
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Set the puzzle from a string representation
        /// </summary>
        /// <param name="puzzleString">String representation of a puzzle saved from <see cref="CurrentPuzzleToString"/></param>
        /// <returns></returns>
        public bool SetPuzzleFromString(string puzzleString)
        {
            // If the given string doesn't the right number of values, the puzzle can't be set
            if (puzzleString.Length != Cells.Count)
            {
                return false;
            }

            DeselectAllCells();

            for (int i = 0; i < Cells.Count; i++)
            {
                if (puzzleString[i] == '-')
                {
                    Cells[i].Digit = 0;
                    Cells[i].IsLockedDigit = false;
                    Cells[i].ClearPencilMarks();
                }
                else
                {
                    Cells[i].Digit = puzzleString[i] - '0';
                    Cells[i].IsLockedDigit = true;
                    Cells[i].ClearPencilMarks();
                }
            }

            return true;
        }

        /// <summary>
        /// True modulo helper for wrapping row/column numbers from 0-8
        /// </summary>
        /// <param name="x"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static int Modulo(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }
    }
}
