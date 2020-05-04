using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for PuzzleGrid.xaml
    /// </summary>
    public partial class PuzzleGrid : UserControl
    {
        // Private state for mouse interaction
        private int mouseRow;
        private int mouseColumn;
        private bool mouseDown;
        private bool toggleSelect;

        public SudokuPuzzleViewModel Puzzle => DataContext as SudokuPuzzleViewModel;

        public PuzzleGrid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle left mouse button clicks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Puzzle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var puzzle = (PuzzleGrid)sender;
            if (e.ChangedButton == MouseButton.Left)
            {
                // Determine which cell the mouse is over by finding the mouse's relative position to the puzzle in rows and columns
                puzzle.CaptureMouse();
                var pos = e.GetPosition(puzzle);
                int column = (int)((pos.X / puzzle.ActualWidth) * 9);
                int row = (int)((pos.Y / puzzle.ActualHeight) * 9);
                mouseRow = row;
                mouseColumn = column;
                mouseDown = true;
                if (IsToggleSelectHotkeyDown())
                {
                    toggleSelect = true;
                    Puzzle.ToggleCell(row, column);
                }
                else
                {
                    Puzzle.SelectCell(row, column, IsAddSelectHotkeyDown());
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handle mouse dragging when the left button is down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Puzzle_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;

            // Determine which cell the mouse is over by finding the mouse's relative position to the puzzle in rows and columns
            var puzzle = (PuzzleGrid)sender;
            var pos = e.GetPosition(puzzle);
            int column = (int)((pos.X / puzzle.ActualWidth) * 9);
            int row = (int)((pos.Y / puzzle.ActualHeight) * 9);
            row = Math.Max(0, Math.Min(row, 8));
            column = Math.Max(0, Math.Min(column, 8));
            if (row != mouseRow || column != mouseColumn)
            {
                if (toggleSelect)
                {
                    Puzzle.ToggleCell(row, column);
                }
                else
                {
                    Puzzle.SelectCell(row, column, true);
                }
                mouseRow = row;
                mouseColumn = column;
            }
        }

        /// <summary>
        /// Handle release of the left mouse button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Puzzle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var puzzle = (PuzzleGrid)sender;
                puzzle.ReleaseMouseCapture();
                mouseDown = false;
                toggleSelect = false;
            }
        }

        /// <summary>
        /// Helper to determine whether to add to the current selection
        /// </summary>
        /// <returns></returns>
        private bool IsAddSelectHotkeyDown()
        {
            return (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));
        }

        /// <summary>
        /// Helper to determine whether to toggle with the current selection
        /// </summary>
        /// <returns></returns>
        private bool IsToggleSelectHotkeyDown()
        {
            return (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl));
        }
    }
}
