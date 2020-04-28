using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku
{
    public class SudokuApplicationViewModel : BaseViewModel
    {
        private SudokuPuzzleViewModel puzzleVM;
        public SudokuPuzzleViewModel PuzzleViewModel => puzzleVM;

        public SudokuApplicationViewModel()
        {
            puzzleVM = new SudokuPuzzleViewModel();
        }
    }
}
