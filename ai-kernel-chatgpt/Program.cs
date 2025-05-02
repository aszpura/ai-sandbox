using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.Configuration;


IConfigurationRoot config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();

var builder = Kernel.CreateBuilder();


builder.AddOpenAIChatCompletion(
    modelId: "gpt-3.5-turbo", // or "gpt-4"
    apiKey: config["API_KEY"] ?? throw new InvalidOperationException("API_KEY is not set in the environment variables.")
);


var kernel = builder.Build();

// Get the chat completion service
var chatService = kernel.GetRequiredService<IChatCompletionService>();

// Create chat history (you can add system prompts, user messages, etc.)
ChatHistory chat = [];
chat.AddSystemMessage("You are a helpful assistant.");

Console.WriteLine("Simple AI Chat Agent (type 'exit' to quit)");
Console.WriteLine("----------------------------------------");

while (true)
{
    // Get user input
    Console.Write("You: ");
    var userInput = Console.ReadLine();

    if (string.Equals(userInput, "exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (string.IsNullOrEmpty(userInput))
    {
        continue;
    }

    chat.AddUserMessage(userInput);

    try
    {
        var response = await chatService.GetChatMessageContentAsync(chat,  new PromptExecutionSettings());

        Console.WriteLine($"AI: {response.Content}");
        chat.AddAssistantMessage(response.Content ?? string.Empty);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
    }
}

