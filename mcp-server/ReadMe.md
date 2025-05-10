# MCP server addtional info

## Packages

- dotnet add package ModelContextProtocol --prerelease
- dotnet add package Microsoft.Extensions.Hosting

## Testing

- use inspector `npx @modelcontextprotocol/inspector dotnet run` from project folder and open browser with the adress
  - connect, list tools, execute tools
  - alos possible to run built exe

## VS code setup

### add to list of runing mcp servers

- open vs code settings, search for "mcp", edit settings.json for mcp, add new server:

```json

"mcp-server-dotnet": {
                "command": "dotnet",
                "args": [
                    "run",
                    "--project",
                    "C:\\path\\mcp-server"
                ],
                "env": {}
            }

```

- or if built

```json
"mcp-server-dotnet-built": {
                "command": "c:\\path\\mcp-server.exe",
                "args": [],
                "env": {}
            }


```
