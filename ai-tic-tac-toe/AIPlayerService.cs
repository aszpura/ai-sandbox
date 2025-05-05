namespace ai_tic_tac_toe;

public interface IAIPlayerService
{
    Task<string> GetNextMoveAsync(Board board, string player);
}

public class AIPlayerService : IAIPlayerService
{
    public async Task<string> GetNextMoveAsync(Board board, string player)
    {
        // TODO: Implement AI logic
        // For now, return a placeholder implementation
        await Task.Delay(200); // Simulate thinking
        return board.Cells.IndexOf(" ") switch
        {
            0 => "A1",
            1 => "A2",
            2 => "A3",
            3 => "B1",
            4 => "B2",
            5 => "B3",
            6 => "C1",
            7 => "C2",
            8 => "C3",
            _ => throw new InvalidOperationException("No valid move found")
        };
        
       // var chatService = kernel.GetRequiredService<IChatCompletionService>();

// test chat completion
// ChatHistory chat = [];
// chat.AddSystemMessage("You are a helpful assistant.");
// chat.AddUserMessage("Hello, name a random polish king and when he was governing");
// var response = await chatService.GetChatMessageContentAsync(chat, new PromptExecutionSettings());
// Console.WriteLine($"AI: {response.Content}");
    }
}