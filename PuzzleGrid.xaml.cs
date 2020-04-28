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
    public partial class PuzzleGrid : UserControl, IPuzzleProvider
    {
        public SudokuPuzzleViewModel Puzzle { get; }
        public PuzzleInputHandler InputHandler { get; }

        public PuzzleGrid()
        {
            InitializeComponent();
            DataContext = Puzzle = new SudokuPuzzleViewModel();
            InputHandler = new PuzzleInputHandler(Puzzle);
        }

        private void Puzzle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var puzzle = (PuzzleGrid)sender;
            puzzle.CaptureMouse();
            InputHandler.MouseDown(puzzle, e);

        }

        private void Puzzle_MouseMove(object sender, MouseEventArgs e)
        {
            var puzzle = (PuzzleGrid)sender;
            InputHandler.MouseMove(puzzle, e);
        }

        private void Puzzle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var puzzle = (PuzzleGrid)sender;
            puzzle.ReleaseMouseCapture();
            InputHandler.MouseUp(puzzle, e);
        }
    }
}
