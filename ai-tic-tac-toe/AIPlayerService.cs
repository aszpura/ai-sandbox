namespace ai_tic_tac_toe;

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

public interface IAIPlayerService
{
    Task<string> GetNextMoveAsync(Board board, string player);
}

public class AIPlayerService : IAIPlayerService
{
    private readonly Kernel _kernel;
    private readonly IChatCompletionService _chatService;

    private ChatHistory Chat = [];  

    private const string SystemMessage = @"You are playing Tic Tac Toe as an expert AI player. The board uses this coordinate system:
A1|A2|A3
B1|B2|B3
C1|C2|C3

Strategic guidelines:
1. Center position (B2) is often strongest - take it if available
2. Corner positions (A1,A3,C1,C3) are next best - especially if opponent has center

Rules:
- Respond ONLY with a valid move position (e.g. 'A1', 'B2', etc.).
- Winning requires 3 in a row (horizontal, vertical, or diagonal)";
    public AIPlayerService(Kernel kernel)
    {
        _kernel = kernel;
        _chatService = kernel.GetRequiredService<IChatCompletionService>();
        Chat.AddSystemMessage(SystemMessage);
    }

    public async Task<string> GetNextMoveAsync(Board board, string player)
    {
        // Create visual board representation for better context
        var visualBoard = GetBoardAsString(board.Cells);
        string prompt = $@"You are playing as {player}. Current board state (first three values represent first row, next three represent second row and last thre values represent last row) ):

{visualBoard}

Analyze the board and make your next move. Consider:
1. Can you win in this move?
2. Can you block the opponent from winning in their next move?

Respond with just the single position (e.g. 'A1').
Never choose a position that is already taken. List of already taken positions: {string.Join(",", board.GetTakenPositions())}.";
        Chat.AddUserMessage(prompt);

        try
        {
            var response = await _chatService.GetChatMessageContentAsync(Chat);
            string move = (response.Content ?? "").Trim().ToUpper();
            
            // Validate that the response is in correct format
            if (move.Length == 2 && move[0] >= 'A' && move[0] <= 'C' && move[1] >= '1' && move[1] <= '3')
            {
                return move;
            }
            
            // If invalid response, fall back to strategic default moves
            return GetStrategicFallbackMove(board.Cells);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"AI Error: {ex.Message}. Using fallback strategy.");
            return GetStrategicFallbackMove(board.Cells);
        }
    }

    private string GetBoardAsString(List<string> cells) => string.Join(",", cells);

    private string GetVisualBoard(List<string> cells)
    {
        return $@"
        {cells[0]}|{cells[1]}|{cells[2]}
        -+-+-
        {cells[3]}|{cells[4]}|{cells[5]}
        -+-+-
        {cells[6]}|{cells[7]}|{cells[8]}";
    }

    public static string GetStrategicFallbackMove(List<string> cells)
    {
        // Priority order: Center -> Corners -> Edges
        var moveOrder = new[] 
        { 
            (4, "B2"),  // Center
            (0, "A1"), (2, "A3"), (6, "C1"), (8, "C3"),  // Corners
            (1, "A2"), (3, "B1"), (5, "B3"), (7, "C2")   // Edges
        };

        foreach (var (index, position) in moveOrder)
        {
            if (cells[index] == " ")
                return position;
        }

        throw new InvalidOperationException("No valid move found");
    }
}