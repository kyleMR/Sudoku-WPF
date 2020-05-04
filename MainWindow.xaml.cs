using System.Windows;
using System.Windows.Input;

namespace Sudoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Modifier key states for changing mouse input behavior
        private bool shiftHeld;
        private bool ctrlHeld;

        /// <summary>
        /// Main application viewmodel
        /// </summary>
        private SudokuApplicationViewModel AppViewModel { get; }

        /// <summary>
        /// Application puzzle viewmodel
        /// </summary>
        private SudokuPuzzleViewModel Puzzle => AppViewModel.Puzzle;

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = AppViewModel = new SudokuApplicationViewModel();
        }

        /// <summary>
        /// Keyboard input handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                // Handle arrow key movement input
                case Key.Left:
                    Puzzle.MoveLeft(shiftHeld || ctrlHeld);
                    break;
                case Key.Right:
                    Puzzle.MoveRight(shiftHeld || ctrlHeld);
                    break;
                case Key.Up:
                    Puzzle.MoveUp(shiftHeld || ctrlHeld);
                    break;
                case Key.Down:
                    Puzzle.MoveDown(shiftHeld || ctrlHeld);
                    break;

                //  Handle numeric inputs
                case Key.NumPad1:
                case Key.D1:
                    AppViewModel.InputDigit(1);
                    break;
                case Key.NumPad2:
                case Key.D2:
                    AppViewModel.InputDigit(2);
                    break;
                case Key.NumPad3:
                case Key.D3:
                    AppViewModel.InputDigit(3);
                    break;
                case Key.NumPad4:
                case Key.D4:
                    AppViewModel.InputDigit(4);
                    break;
                case Key.NumPad5:
                case Key.D5:
                    AppViewModel.InputDigit(5);
                    break;
                case Key.NumPad6:
                case Key.D6:
                    AppViewModel.InputDigit(6);
                    break;
                case Key.NumPad7:
                case Key.D7:
                    AppViewModel.InputDigit(7);
                    break;
                case Key.NumPad8:
                case Key.D8:
                    AppViewModel.InputDigit(8);
                    break;
                case Key.NumPad9:
                case Key.D9:
                    AppViewModel.InputDigit(9);
                    break;

                // Delete handling
                case Key.Delete:
                case Key.Back:
                    Puzzle.ClearCell();
                    break;

                // Shift and ctrl modifier handling
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    ctrlHeld = true;
                    AppViewModel.InputMode = DigitInputMode.Center;
                    break;

                case Key.LeftShift:
                case Key.RightShift:
                    shiftHeld = true;
                    AppViewModel.InputMode = DigitInputMode.Outer;
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Keyboard input handling, specifically for releasing keyboard modifiers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.LeftCtrl:
                case Key.RightCtrl:
                    ctrlHeld = false;
                    if (shiftHeld)
                    {
                        AppViewModel.InputMode = DigitInputMode.Outer;
                    }
                    else
                    {
                        AppViewModel.InputMode = DigitInputMode.Normal;
                    }
                    break;

                case Key.LeftShift:
                case Key.RightShift:
                    shiftHeld = false;
                    if (ctrlHeld)
                    {
                        AppViewModel.InputMode = DigitInputMode.Center;
                    }
                    else
                    {
                        AppViewModel.InputMode = DigitInputMode.Normal;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Mouse input handling. Only called if a mouse click wasn't handled by the puzzle or various buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // If the user clicks in open space in the app, clear the puzzle selection
            if (e.ChangedButton == MouseButton.Left)
            {
                Puzzle.DeselectAllCells();
            }
        }
    }
}
