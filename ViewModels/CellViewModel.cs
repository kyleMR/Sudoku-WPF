using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Sudoku
{
    /// <summary>
    /// Viewmodel governing the display of individual puzzle cells
    /// </summary>
    public class CellViewModel : BaseViewModel
    {
        // Border width constants
        private const double OuterBorder = 8.0;
        private const double NonetBorder = 5.0;
        private const double CellBorder = 1.5;

        private int row;
        private int column;
        private int digit;
        private bool isHighlighted;
        private bool isLockedDigit;

        private HashSet<int> outerPencilMarks;
        private SortedSet<int> centerPencilMarks;

        /// <summary>
        /// The displayed digit (0-9, 0 represents a blank cell)
        /// </summary>
        public int Digit
        {
            get => digit;
            set
            {
                SetProperty(ref digit, value);
                NotifyMarkDisplayChanged();
            }
        }

        /// <summary>
        /// Whether this cell is selected/highlighted
        /// </summary>
        public bool IsHighlighted
        {
            get => isHighlighted;
            set => SetProperty(ref isHighlighted, value);
        }

        /// <summary>
        /// Whether this cell's digit entry is locked to user input
        /// </summary>
        public bool IsLockedDigit
        {
            get => isLockedDigit;
            set => SetProperty(ref isLockedDigit, value);
        }

        /// <summary>
        /// This cell's row number
        /// </summary>
        public int Row
        {
            get => row;
            set => SetProperty(ref row, value);
        }

        /// <summary>
        /// This cell's column number
        /// </summary>
        public int Column
        {
            get => column;
            set => SetProperty(ref column, value);
        }

        /// <summary>
        /// Defines the left border thickness based on column position
        /// </summary>
        public double LeftThickness
        {
            get
            {
                if (Column == 0)
                {
                    return OuterBorder;
                }
                else if (Column % 3 == 0)
                {
                    return NonetBorder / 2f;
                }
                else
                {
                    return CellBorder / 2f;
                }
            }
        }

        /// <summary>
        /// Defines the right border thickness based on column position
        /// </summary>
        public double RightThickness
        {
            get
            {
                if (Column == 8)
                {
                    return OuterBorder;
                }
                else if ((Column + 1) % 3 == 0)
                {
                    return NonetBorder / 2f;
                }
                else
                {
                    return CellBorder / 2f;
                }
            }
        }

        /// <summary>
        /// Defines the top border thickness based on row position
        /// </summary>
        public double TopThickness
        {
            get
            {
                if (Row == 0)
                {
                    return OuterBorder;
                }
                else if (Row % 3 == 0)
                {
                    return NonetBorder / 2f;
                }
                else
                {
                    return CellBorder / 2f;
                }
            }
        }

        /// <summary>
        /// Defines the bottom border thickness based on row position
        /// </summary>
        public double BottomThickness
        {
            get
            {
                if (Row == 8)
                {
                    return OuterBorder;
                }
                else if ((Row + 1) % 3 == 0)
                {
                    return NonetBorder / 2f;
                }
                else
                {
                    return CellBorder / 2f;
                }
            }
        }

        /// <summary>
        /// Defines the cell's outer border thickness
        /// </summary>
        public Thickness BorderThickness
        {
            get
            {
                return new Thickness(LeftThickness, TopThickness, RightThickness, BottomThickness); 
            }
        }

        // Getters for display status of outer pencil marks
        public bool ShowOuterMark1 => Digit == 0 && outerPencilMarks.Contains(1);
        public bool ShowOuterMark2 => Digit == 0 && outerPencilMarks.Contains(2);
        public bool ShowOuterMark3 => Digit == 0 && outerPencilMarks.Contains(3);
        public bool ShowOuterMark4 => Digit == 0 && outerPencilMarks.Contains(4);
        public bool ShowOuterMark5 => Digit == 0 && outerPencilMarks.Contains(5);
        public bool ShowOuterMark6 => Digit == 0 && outerPencilMarks.Contains(6);
        public bool ShowOuterMark7 => Digit == 0 && outerPencilMarks.Contains(7);
        public bool ShowOuterMark8 => Digit == 0 && outerPencilMarks.Contains(8);
        public bool ShowOuterMark9 => Digit == 0 && outerPencilMarks.Contains(9);

        /// <summary>
        /// Center mark display text
        /// </summary>
        public string CenterMarks => string.Join("", centerPencilMarks);

        /// <summary>
        /// Whether to display center marks
        /// </summary>
        public bool CenterMarksVisible => Digit == 0;

        /// <summary>
        /// Default constructor
        /// </summary>
        public CellViewModel()
        {
            outerPencilMarks = new HashSet<int>();
            centerPencilMarks = new SortedSet<int>();
        }

        /// <summary>
        /// Toggle the display of the given digit as an outer pencil mark
        /// </summary>
        /// <param name="digit">Digit to set or unset</param>
        public void ToggleOuterMark(int digit)
        {
            if (outerPencilMarks.Contains(digit))
            {
                outerPencilMarks.Remove(digit);
            }
            else
            {
                outerPencilMarks.Add(digit);
            }
            var propertyName = $"ShowOuterMark{digit}";
            OnPropertyChanged(propertyName);
        }

        /// <summary>
        /// Toggle the display of the given digit as a center pencil mark
        /// </summary>
        /// <param name="digit">Digit to set or unset</param>
        public void ToggleCenterMark(int digit)
        {
            if (centerPencilMarks.Contains(digit))
            {
                centerPencilMarks.Remove(digit);
            }
            else
            {
                centerPencilMarks.Add(digit);
            }
            OnPropertyChanged(nameof(CenterMarks));
        }

        /// <summary>
        /// Clear all pencil marks on this cell
        /// </summary>
        public void ClearPencilMarks()
        {
            var markArray = outerPencilMarks.ToArray();
            foreach (int digit in markArray)
            {
                outerPencilMarks.Remove(digit);
                var propertyName = $"ShowOuterMark{digit}";
                OnPropertyChanged(propertyName);
            }

            centerPencilMarks.Clear();
            OnPropertyChanged(nameof(CenterMarks));
        }

        /// <summary>
        /// Helper to hide/show display of pencil marks when a digit is set/unset
        /// </summary>
        private void NotifyMarkDisplayChanged()
        {
            OnPropertyChanged(nameof(ShowOuterMark1));
            OnPropertyChanged(nameof(ShowOuterMark2));
            OnPropertyChanged(nameof(ShowOuterMark3));
            OnPropertyChanged(nameof(ShowOuterMark4));
            OnPropertyChanged(nameof(ShowOuterMark5));
            OnPropertyChanged(nameof(ShowOuterMark6));
            OnPropertyChanged(nameof(ShowOuterMark7));
            OnPropertyChanged(nameof(ShowOuterMark8));
            OnPropertyChanged(nameof(ShowOuterMark9));
            OnPropertyChanged(nameof(CenterMarksVisible));
        }
    }
}
