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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SudokuPuzzleViewModel Puzzle => PuzzleGrid.Puzzle;

        private PuzzleInputHandler InputHandler => PuzzleGrid.InputHandler;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Puzzle_KeyDown(object sender, KeyEventArgs e)
        {
            InputHandler.KeyDown(e);
        }

        private void Puzzle_KeyUp(object sender, KeyEventArgs e)
        {
            InputHandler.KeyUp(e);
        }
    }
}
