using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Sudoku
{
    public class PuzzleInputHandler
    {
        private bool dragging;
        private bool ctrlHeld;
        private bool shiftHeld;

        private int mouseRow;
        private int mouseColumn;

        private SudokuPuzzleViewModel vm;

        public PuzzleInputHandler(SudokuPuzzleViewModel puzzleViewModel)
        {
            vm = puzzleViewModel;
        }

        public void MouseDown(PuzzleGrid puzzle, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(puzzle);
            int column = (int)((pos.X / puzzle.ActualWidth) * 9);
            int row = (int)((pos.Y / puzzle.ActualHeight) * 9);
            mouseRow = row;
            mouseColumn = column;
            dragging = true;
            bool replace = !ctrlHeld;
            vm.SelectCell(row, column, replace);

        }

        public void MouseUp(PuzzleGrid puzzle, MouseButtonEventArgs e)
        {
            dragging = false;
        }

        public void MouseMove(PuzzleGrid puzzle, MouseEventArgs e)
        {
            if (!dragging) return;

            var pos = e.GetPosition(puzzle);
            int column = (int)((pos.X / puzzle.ActualWidth) * 9);
            int row = (int)((pos.Y / puzzle.ActualHeight) * 9);
            row = Math.Max(0, Math.Min(row, 8));
            column = Math.Max(0, Math.Min(column, 8));
            if (row != mouseRow || column != mouseColumn)
            {
                vm.SelectCell(row, column, false);
                mouseRow = row;
                mouseColumn = column;
            }
        }

        public void KeyDown(KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Left:
                    vm.MoveLeft();
                    break;
                case Key.Right:
                    vm.MoveRight();
                    break;
                case Key.Up:
                    vm.MoveUp();
                    break;
                case Key.Down:
                    vm.MoveDown();
                    break;

                case Key.NumPad1:
                case Key.D1:
                    vm.SetDigit(1);
                    break;
                case Key.NumPad2:
                case Key.D2:
                    vm.SetDigit(2);
                    break;
                case Key.NumPad3:
                case Key.D3:
                    vm.SetDigit(3);
                    break;
                case Key.NumPad4:
                case Key.D4:
                    vm.SetDigit(4);
                    break;
                case Key.NumPad5:
                case Key.D5:
                    vm.SetDigit(5);
                    break;
                case Key.NumPad6:
                case Key.D6:
                    vm.SetDigit(6);
                    break;
                case Key.NumPad7:
                case Key.D7:
                    vm.SetDigit(7);
                    break;
                case Key.NumPad8:
                case Key.D8:
                    vm.SetDigit(8);
                    break;
                case Key.NumPad9:
                case Key.D9:
                    vm.SetDigit(9);
                    break;

                case Key.Delete:
                case Key.Back:
                    vm.SetDigit(0);
                    break;

                case Key.LeftCtrl:
                case Key.RightCtrl:
                    ctrlHeld = true;
                    break;

                case Key.LeftShift:
                case Key.RightShift:
                    shiftHeld = true;
                    break;

                default:
                    break;
            }
        }

        public void KeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    ctrlHeld = false;
                    break;

                case Key.LeftShift:
                case Key.RightShift:
                    shiftHeld = false;
                    break;

                default:
                    break;
            }
        }
    }
}
