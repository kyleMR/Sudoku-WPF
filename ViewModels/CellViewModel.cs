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

        private const double OuterBorder = 6.0;
        private const double NonetBorder = 4.0;
        private const double CellBorder = 1.5;
        private int row;
        private int column;
        private int digit;
        private bool isHighlighted;
        private bool isGivenDigit;

        public int Digit
        {
            get => digit;
            set => SetProperty(ref digit, value);
        }

        public bool IsHighlighted
        {
            get => isHighlighted;
            set => SetProperty(ref isHighlighted, value);
        }

        public bool IsGivenDigit
        {
            get => isGivenDigit;
            set => SetProperty(ref isGivenDigit, value);
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
    }
}
