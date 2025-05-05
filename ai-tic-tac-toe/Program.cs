using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ai_tic_tac_toe;

// Configuration setup
IConfigurationRoot config = new ConfigurationBuilder()
    .AddEnvironmentVariables()
    .Build();

// Setup services
var services = new ServiceCollection();

// Configure Semantic Kernel
var kernelBuilder = Kernel.CreateBuilder();
kernelBuilder.AddOpenAIChatCompletion(
    modelId: "gpt-3.5-turbo",
    apiKey: config["OPEN_API_KEY"] ?? throw new InvalidOperationException("OPEN_API_KEY is not set in the environment variables.")
);
var kernel = kernelBuilder.Build();

// Register game services
services.AddSingleton(kernel);
services.AddSingleton<IAIPlayerService, AIPlayerService>();
services.AddSingleton<GameManager>();

// Build service provider
var serviceProvider = services.BuildServiceProvider();

// Start the game
var gameManager = serviceProvider.GetRequiredService<GameManager>();
await gameManager.RunGame();