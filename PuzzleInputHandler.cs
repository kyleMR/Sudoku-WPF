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
        private SudokuPuzzleViewModel vm;

        public PuzzleInputHandler(SudokuPuzzleViewModel puzzleViewModel)
        {
            vm = puzzleViewModel;
        }

        public void KeyDown(KeyEventArgs e)
        {
        }

        public void KeyUp(KeyEventArgs e)
        {
        }
    }
}
