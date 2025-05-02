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
chat.AddUserMessage("What is captial city of Canada?");

// Get response
var reply = await chatService.GetChatMessageContentAsync(chat);
Console.WriteLine(reply?.Content);

