using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sudoku
{
    /// <summary>
    /// Digit input types
    /// </summary>
    public enum DigitInputMode
    {
        // Main digit input
        Normal,
        // Outer edge pencil marks
        Outer,
        // Cell center pencil marks
        Center,
    }

    /// <summary>
    /// Main application viewmodel
    /// </summary>
    public class SudokuApplicationViewModel : BaseViewModel
    {
        private DigitInputMode inputMode;
        private bool displayPuzzleOptions;
        private bool puzzleIsLocked;

        /// <summary>
        /// This application's sudoku puzzle viewmodel
        /// </summary>
        public SudokuPuzzleViewModel Puzzle { get; }

        public ICommand SetDigitCommand { get; }
        public ICommand ClearCellCommand { get; }
        public ICommand LockUnlockPuzzleCommand { get; }
        public ICommand SavePuzzleCommand { get; }
        public ICommand LoadPuzzleCommand { get; }
        public ICommand CheckSolutionCommand { get; }

        /// <summary>
        /// Active digit input mode
        /// </summary>
        public DigitInputMode InputMode
        {
            get => inputMode;
            set
            {
                SetProperty(ref inputMode, value);
                OnPropertyChanged(nameof(IsNormalInputMode));
                OnPropertyChanged(nameof(IsOuterInputMode));
                OnPropertyChanged(nameof(IsCenterInputMode));
                OnPropertyChanged(nameof(TopRowDigitVertAlign));
                OnPropertyChanged(nameof(BottomRowDigitVertAlign));
                OnPropertyChanged(nameof(LeftColDigitHorizAlign));
                OnPropertyChanged(nameof(RightColDigitHorizAlign));
            }
        }

        public bool IsNormalInputMode
        {
            get => InputMode == DigitInputMode.Normal;
            set
            {
                if (value) InputMode = DigitInputMode.Normal;
            }
        }

        public bool IsOuterInputMode
        {
            get => InputMode == DigitInputMode.Outer;
            set
            {
                if (value) InputMode = DigitInputMode.Outer;
            }
        }

        public bool IsCenterInputMode
        {
            get => InputMode == DigitInputMode.Center;
            set
            {
                if (value) InputMode = DigitInputMode.Center;
            }
        }

        public bool DisplayPuzzleOptions
        {
            get => displayPuzzleOptions;
            set
            {
                SetProperty(ref displayPuzzleOptions, value);
                OnPropertyChanged(nameof(DisplayDigitButtons));
            }
        }

        public bool DisplayDigitButtons => !DisplayPuzzleOptions;

        /// <summary>
        /// Lock state of the puzzle digits
        /// </summary>
        public bool PuzzleIsLocked
        {
            get => puzzleIsLocked;
            set
            {
                SetProperty(ref puzzleIsLocked, value);
                OnPropertyChanged(nameof(LockButtonText));
            }
        }

        public string LockButtonText
        {
            get => PuzzleIsLocked ? "Unlock" : "Lock";
        }

        /// Various alignment properties for digit input buttons while the 'Outer' digit input mode is active
        public VerticalAlignment TopRowDigitVertAlign
        {
            get => InputMode == DigitInputMode.Outer ? VerticalAlignment.Top : VerticalAlignment.Center;
        }

        public VerticalAlignment BottomRowDigitVertAlign
        {
            get => InputMode == DigitInputMode.Outer ? VerticalAlignment.Bottom : VerticalAlignment.Center;
        }

        public HorizontalAlignment LeftColDigitHorizAlign
        {
            get => InputMode == DigitInputMode.Outer ? HorizontalAlignment.Left : HorizontalAlignment.Center;
        }

        public HorizontalAlignment RightColDigitHorizAlign
        {
            get => InputMode == DigitInputMode.Outer ? HorizontalAlignment.Right : HorizontalAlignment.Center;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public SudokuApplicationViewModel()
        {
            Puzzle = new SudokuPuzzleViewModel();
            InputMode = DigitInputMode.Normal;

            SetDigitCommand = new RelayCommand<int>((digit) => InputDigit(digit));
            ClearCellCommand = new RelayCommand(() => Puzzle.ClearCell());
            LockUnlockPuzzleCommand = new RelayCommand(LockUnlockDigits);
            CheckSolutionCommand = new RelayCommand(async () => await CheckSolution());
            LoadPuzzleCommand = new RelayCommand(LoadPuzzle);
            SavePuzzleCommand = new RelayCommand(SavePuzzle);
        }

        /// <summary>
        /// Input the given digit into the selected puzzle cells based on the current input mode
        /// </summary>
        /// <param name="digit">Digit to input</param>
        public void InputDigit(int digit)
        {
            if (InputMode == DigitInputMode.Normal)
            {
                Puzzle.SetDigit(digit);
            }
            else if (InputMode == DigitInputMode.Outer)
            {
                Puzzle.ToggleOuterMark(digit);
            }
            else if (InputMode == DigitInputMode.Center)
            {
                Puzzle.ToggleCenterMark(digit);
            }
        }

        /// <summary>
        /// Toggle the lock/unlock state of puzzle digits
        /// </summary>
        private void LockUnlockDigits()
        {
            if (PuzzleIsLocked)
            {
                Puzzle.UnlockDigits();
            }
            else
            {
                Puzzle.LockDigits();
            }
            PuzzleIsLocked = !PuzzleIsLocked;
        }

        /// <summary>
        /// Check whether the current state of the puzzle is solved, incorrect, or incomplete
        /// </summary>
        /// <returns></returns>
        private async Task CheckSolution()
        {
            var result = await Puzzle.CheckSolutionAsync();
            switch (result)
            {
                case SolutionState.Correct:
                    MessageBox.Show("Everything looks correct!");
                    break;
                case SolutionState.Incomplete:
                    MessageBox.Show("Missing some digits!");
                    break;
                case SolutionState.Incorrect:
                    MessageBox.Show("Something's not quite right.");
                    break;
            }
        }

        /// <summary>
        /// Save the current locked digits as a new puzzle file
        /// </summary>
        private void SavePuzzle()
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Puzzle file (*.pzl)|*.pzl"
            };
            if (saveDialog.ShowDialog() == true)
            {
                var puzzleString = Puzzle.CurrentPuzzleToString();
                File.WriteAllText(saveDialog.FileName, puzzleString);
            }
        }

        /// <summary>
        /// Load a previously saved puzzle file and display it in the puzzle grid
        /// </summary>
        private void LoadPuzzle()
        {
            OpenFileDialog openDialog = new OpenFileDialog
            {
                Filter = "Puzzle file (*.pzl)|*.pzl"
            };

            if (openDialog.ShowDialog() == true)
            {
                var puzzleString = File.ReadAllText(openDialog.FileName);

                // If the puzzle can't be set, notify the user
                if (!Puzzle.SetPuzzleFromString(puzzleString))
                {
                    MessageBox.Show(
                        "There appears to be an issue with this puzzle file. The puzzle could not be loaded.", 
                        "Load Puzzle",
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
                else
                {
                    PuzzleIsLocked = true;
                }
            }
        }
    }
}
