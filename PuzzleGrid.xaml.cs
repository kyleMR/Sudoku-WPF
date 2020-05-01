using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for PuzzleGrid.xaml
    /// </summary>
    public partial class PuzzleGrid : UserControl
    {
        private int mouseRow;
        private int mouseColumn;
        private bool mouseDown;
        private bool toggleSelect;

        public SudokuPuzzleViewModel Puzzle => DataContext as SudokuPuzzleViewModel;

        public PuzzleGrid()
        {
            InitializeComponent();
        }

        private void Puzzle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var puzzle = (PuzzleGrid)sender;
            if (e.ChangedButton == MouseButton.Left)
            {
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

        private void Puzzle_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown) return;

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

        private bool IsAddSelectHotkeyDown()
        {
            return (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift));
        }

        private bool IsToggleSelectHotkeyDown()
        {
            return (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl));
        }
    }
}
