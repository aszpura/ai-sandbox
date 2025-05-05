namespace ai_tic_tac_toe;

public class Board
{
    private readonly List<string> _cells;

    public Board()
    {
        // Initialize empty board with 9 cells
        _cells = Enumerable.Repeat(" ", 9).ToList();
    }

    public List<string> Cells => _cells;

    public void Display()
    {
        Console.WriteLine();
        Console.WriteLine($" {_cells[0]} | {_cells[1]} | {_cells[2]} ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {_cells[3]} | {_cells[4]} | {_cells[5]} ");
        Console.WriteLine("---+---+---");
        Console.WriteLine($" {_cells[6]} | {_cells[7]} | {_cells[8]} ");
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

        // Update the board
        _cells[index] = player;
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
}