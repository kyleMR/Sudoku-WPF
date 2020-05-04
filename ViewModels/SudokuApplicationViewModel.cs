using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Sudoku
{
    public enum DigitInputMode
    {
        Normal,
        Outer,
        Center,
    }

    public class SudokuApplicationViewModel : BaseViewModel
    {
        private DigitInputMode inputMode;
        private bool displayPuzzleOptions;
        private bool puzzleIsLocked;

        public SudokuPuzzleViewModel Puzzle { get; }

        public ICommand SetDigitCommand { get; }
        public ICommand ClearCellCommand { get; }
        public ICommand LockUnlockPuzzleCommand { get; }
        public ICommand SavePuzzleCommand { get; }
        public ICommand LoadPuzzleCommand { get; }
        public ICommand CheckSolutionCommand { get; }

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

        public SudokuApplicationViewModel()
        {
            Puzzle = new SudokuPuzzleViewModel();
            InputMode = DigitInputMode.Normal;

            SetDigitCommand = new RelayCommand<int>((digit) => InputDigit(digit));
            ClearCellCommand = new RelayCommand(() => Puzzle.ClearCell());
            LockUnlockPuzzleCommand = new RelayCommand(() => LockUnlockDigits());
            CheckSolutionCommand = new RelayCommand(() => CheckSolution());
        }

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

        private void CheckSolution()
        {
            var result = Puzzle.CheckSolution();
            switch (result)
            {
                case SolutionState.Correct:
                    Debug.WriteLine("Correct!");
                    break;
                case SolutionState.Incomplete:
                    Debug.WriteLine("Missing some digits!");
                    break;
                case SolutionState.Incorrect:
                    Debug.WriteLine("Something's not quite right.");
                    break;
            }
        }
    }
}
