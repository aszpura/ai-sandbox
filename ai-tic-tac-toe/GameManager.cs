namespace ai_tic_tac_toe;

public class GameManager(IAIPlayerService aiPlayerService)
{
    private readonly Board _board = new();
    private readonly Random _random = new();
    private readonly IAIPlayerService _aiPlayerService = aiPlayerService;
    private const string HumanPlayer = "X";
    private const string AIPlayer = "O";

    public async Task RunGame()
    {
        // Randomly decide who starts (X or O)
        string currentPlayer = _random.Next(2) == 0 ? HumanPlayer : AIPlayer;
        
        Console.WriteLine("Welcome to Tic Tac Toe!");
        Console.WriteLine($"You are playing as {HumanPlayer}, AI is playing as {AIPlayer}");
        Console.WriteLine($"Player {currentPlayer} starts the game.");
        Console.WriteLine();
        
        // Display initial empty board
        _board.Display();

        // Game loop
        bool gameInProgress = true;
        while (gameInProgress)
        {
            if (currentPlayer == HumanPlayer)
            {
                HandleHumanTurn();
            }
            else
            {
                await HandleAITurn();
            }

            // Check for win
            if (_board.CheckWin(currentPlayer))
            {
                Console.WriteLine($"\nPlayer {currentPlayer} wins!");
                gameInProgress = false;
                continue;
            }

            // Check for draw
            if (_board.IsDraw())
            {
                Console.WriteLine("\nGame ends in a draw!");
                gameInProgress = false;
                continue;
            }

            // Switch players
            currentPlayer = currentPlayer == HumanPlayer ? AIPlayer : HumanPlayer;
        }

        Console.WriteLine("\nGame Over!");
    }

    private void HandleHumanTurn()
    {
        bool validMove = false;
        while (!validMove)
        {
            Console.WriteLine("\nEnter your move (e.g., A1, B2, C3): ");
            string? move = (Console.ReadLine() ?? string.Empty).Trim().ToUpper();

            if (string.IsNullOrEmpty(move))
            {
                Console.WriteLine("Invalid input. Please try again.");
                continue;
            }

            var result = _board.UpdateBoard(HumanPlayer, move);
            if (result.IsSuccess)
            {
                validMove = true;
                _board.Display();
            }
            else
            {
                Console.WriteLine($"Error: {result.ErrorMessage}");
            }
        }
    }

    private async Task HandleAITurn()
    {
        Console.WriteLine("\nAI is thinking...");
        string aiMove = await _aiPlayerService.GetNextMoveAsync(_board, AIPlayer);
        var result = _board.UpdateBoard(AIPlayer, aiMove);
        
        if (result.IsSuccess)
        {
            Console.WriteLine($"AI plays: {aiMove}");
            _board.Display();
        }
        else
        {
            // This shouldn't happen if AI is implemented correctly
            Console.WriteLine($"AI made an invalid move: {result.ErrorMessage}");
        }
    }
}