using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

var builder = Host.CreateApplicationBuilder(args);

builder.Logging.AddConsole(consoleLogOptions =>
{
    consoleLogOptions.LogToStandardErrorThreshold = LogLevel.Trace;
});

builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();
await builder.Build().RunAsync();

[McpServerToolType]
public static class SqlQueryTool
{
    [McpServerTool, Description("Returns a sample SQL query for demonstration.")]
    public static string GetSqlExample(string context)
    {
        var query = new
        {
            Query = "SELECT * FROM Users WHERE Age > 18 ORDER BY Name ASC",
            Description = "Retrieves all columns from the Users table for users over 18, sorted by name"
        };
        return JsonSerializer.Serialize(query);
    }
}
