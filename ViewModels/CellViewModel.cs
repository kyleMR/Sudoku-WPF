using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sudoku
{
    public class CellViewModel : BaseViewModel
    {

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

        public int Digit
        {
            get => digit;
            set
            {
                SetProperty(ref digit, value);
                NotifyMarkDisplayChanged();
            }
        }

        public bool IsHighlighted
        {
            get => isHighlighted;
            set => SetProperty(ref isHighlighted, value);
        }

        public bool IsLockedDigit
        {
            get => isLockedDigit;
            set => SetProperty(ref isLockedDigit, value);
        }

        public int Row
        {
            get => row;
            set => SetProperty(ref row, value);
        }

        public int Column
        {
            get => column;
            set => SetProperty(ref column, value);
        }

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
        public Thickness BorderThickness
        {
            get
            {
                return new Thickness(LeftThickness, TopThickness, RightThickness, BottomThickness); 
            }
        }

        public bool ShowOuterMark1 => Digit == 0 && outerPencilMarks.Contains(1);
        public bool ShowOuterMark2 => Digit == 0 && outerPencilMarks.Contains(2);
        public bool ShowOuterMark3 => Digit == 0 && outerPencilMarks.Contains(3);
        public bool ShowOuterMark4 => Digit == 0 && outerPencilMarks.Contains(4);
        public bool ShowOuterMark5 => Digit == 0 && outerPencilMarks.Contains(5);
        public bool ShowOuterMark6 => Digit == 0 && outerPencilMarks.Contains(6);
        public bool ShowOuterMark7 => Digit == 0 && outerPencilMarks.Contains(7);
        public bool ShowOuterMark8 => Digit == 0 && outerPencilMarks.Contains(8);
        public bool ShowOuterMark9 => Digit == 0 && outerPencilMarks.Contains(9);

        public string CenterMarks => string.Join("", centerPencilMarks);
        public bool CenterMarksVisible => Digit == 0;

        public CellViewModel()
        {
            outerPencilMarks = new HashSet<int>();
            centerPencilMarks = new SortedSet<int>();
        }

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
