namespace Sudoku
{
    public interface IPuzzleProvider
    {
        SudokuPuzzleViewModel Puzzle { get; }
        PuzzleInputHandler InputHandler { get; }
    }
}
