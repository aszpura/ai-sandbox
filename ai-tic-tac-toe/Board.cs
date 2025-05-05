namespace ai_tic_tac_toe;

public class Board
{
    private readonly List<string> _cells;
    private int? _lastPosition = null;

    public Board()
    {
        // Initialize empty board with 9 cells
        _cells = [.. Enumerable.Repeat(" ", 9)];
    }

    public List<string> Cells => _cells;

    public void Display()
    {
        Console.WriteLine();

        // Inline function to get cell content with color if it's the last position
        string GetCell(int index) =>
            index == _lastPosition
            ? $"\x1b[32m {_cells[index]} \x1b[0m" // Green color for last position
            : $" {_cells[index]} ";

        Console.WriteLine($"{GetCell(0)}|{GetCell(1)}|{GetCell(2)}");
        Console.WriteLine("---+---+---");
        Console.WriteLine($"{GetCell(3)}|{GetCell(4)}|{GetCell(5)}");
        Console.WriteLine("---+---+---");
        Console.WriteLine($"{GetCell(6)}|{GetCell(7)}|{GetCell(8)}");
        Console.WriteLine();
    }

    public UpdateResult UpdateBoard(string player, string position)
    {
        // Validate player
        if (player != "X" && player != "O")
        {
            return new UpdateResult(false, "Player must be either 'X' or 'O'");
        }

        // Validate position format
        if (string.IsNullOrEmpty(position) || position.Length != 2)
        {
            return new UpdateResult(false, "Position must be in format: A1, B2, etc.");
        }

        char row = char.ToUpper(position[0]);
        char col = position[1];

        // Validate row
        if (row < 'A' || row > 'C')
        {
            return new UpdateResult(false, "Row must be A, B, or C");
        }

        // Validate column
        if (col < '1' || col > '3')
        {
            return new UpdateResult(false, "Column must be 1, 2, or 3");
        }

        // Convert position to array index
        int index = (row - 'A') * 3 + (col - '1');

        // Check if cell is already taken
        if (_cells[index] != " ")
        {
            return new UpdateResult(false, "This position is already taken");
        }

        // Update the board and store last position
        _cells[index] = player;
        _lastPosition = index;
        return new UpdateResult(true);
    }

    public bool CheckWin(string player)
    {
        // Check rows
        for (int i = 0; i < 9; i += 3)
        {
            if (_cells[i] == player && _cells[i + 1] == player && _cells[i + 2] == player)
                return true;
        }

        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (_cells[i] == player && _cells[i + 3] == player && _cells[i + 6] == player)
                return true;
        }

        // Check diagonals
        if (_cells[0] == player && _cells[4] == player && _cells[8] == player)
            return true;
        if (_cells[2] == player && _cells[4] == player && _cells[6] == player)
            return true;

        return false;
    }

    public bool IsDraw()
    {
        // This assumes check for win happend already
        return !_cells.Contains(" ");
    }

    public List<string> GetTakenPositions()
    {
        var takenPositions = new List<string>();
        
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i] != " ")
            {
                // Convert index to board position (A1-C3 format)
                char row = (char)('A' + (i / 3));
                char col = (char)('1' + (i % 3));
                takenPositions.Add($"{row}{col}");
            }
        }
        
        return takenPositions;
    }
}