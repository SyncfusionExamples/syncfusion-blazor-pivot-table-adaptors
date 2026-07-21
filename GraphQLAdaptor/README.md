# Blazor Pivot Table with Hot Chocolate GraphQL

## Project Overview

This repository demonstrates a production-ready pattern for binding **GraphQL** data to **Syncfusion Blazor Pivot Table** using **Hot Chocolate GraphQL Server**. The sample application provides complete CRUD (Create, Read, Update, Delete) operations through cell editing, drill-through navigation, and the field list, all backed by a GraphQL adaptor that gives the pivot view full control over how data is read and written. The implementation follows industry best practices using GraphQL queries, mutations, resolvers, and a structured `DataManagerRequestInput` for handling pivot data requests.

## Key Features

- **Hot Chocolate GraphQL Server Integration**: Query resolvers for read operations and mutation resolvers for write operations
- **Syncfusion Blazor Pivot Table**: Field list, drill-through, and configurable rows, columns, and values for multidimensional analysis
- **Complete CRUD Operations**: Add, edit, and delete cells and rows directly from the pivot view, including batch updates
- **GraphQL Adaptor**: Full control over pivot data operations (read and write) via GraphQL queries and mutations
- **DataManagerRequestInput**: Structured input type for handling complex pivot data requests, including aggregation parameters
- **Configurable GraphQL Endpoint**: Backend port and GraphQL endpoint managed via `launchSettings.json`

## Prerequisites

| Component | Version | Purpose |
|-----------|---------|---------|
| Visual Studio 2022 | 17.0 or later | Development IDE with Blazor workload |
| .NET SDK | net8.0 or compatible | Runtime and build tools |
| HotChocolate.AspNetCore | 15.1 or later | GraphQL server framework |
| Syncfusion.Blazor.PivotTable | Latest Version | Pivot Table and UI components |
| Syncfusion.Blazor.Themes | Latest Version | Styling for Pivot Table components |
| Syncfusion.Blazor.Data | Latest Version | Data adaptors including GraphQL support |

## Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd "syncfusion-blazor-pivot-table-remote-data-binding"
   
   cd "GraphQLAdaptor/Pivot_GraphQLAdaptor"
   ```

2. **Restore packages and build**
   ```powershell
   dotnet restore; dotnet build
   ```

3. **Run the application**
   ```powershell
   dotnet run
   ```

4. **Open the application**
   
   Navigate to the local URL displayed in the terminal (typically `https://localhost:7009` or `http://localhost:5272`).

5. **Access GraphQL Playground (Optional)**
   
   Navigate to `https://localhost:7009/graphql` to explore the GraphQL schema and test queries/mutations manually.

## Configuration

### GraphQL Endpoint Port Configuration

The GraphQL endpoint port is configured in `Properties/launchSettings.json`. This file controls where the application runs and where the GraphQL endpoint is exposed.

**Instructions to Change the Port:**

1. Open `Properties/launchSettings.json` in the project root.
2. Locate the `https` profile section:

```json
"https": {
  "commandName": "Project",
  "dotnetRunMessages": true,
  "launchBrowser": true,
  "applicationUrl": "https://localhost:7009;http://localhost:5272",
  "environmentVariables": {
    "ASPNETCORE_ENVIRONMENT": "Development"
  }
}
```

3. Modify the `applicationUrl` property to change port numbers:
   - `https://localhost:7009` - HTTPS port for GraphQL endpoint
   - `http://localhost:5272` - HTTP port for GraphQL endpoint

4. Update the Pivot Table connection URL in `Components/Pages/Home.razor` to match the configured port:
   ```html
   <SfDataManager Url="https://localhost:7009/graphql" GraphQLAdaptorOptions="@adaptorOptions" Adaptor="Adaptors.GraphQLAdaptor"></SfDataManager>
   ```

**Important Notes:**
- Port numbers must be between 1024 and 65535
- Avoid using ports already in use by other applications
- The GraphQL endpoint is always accessible at `/graphql` path (configured via `app.MapGraphQL()` in Program.cs)

### GraphQL Adaptor Configuration

The `GraphQLAdaptorOptions` in `Home.razor` defines how the Pivot Table communicates with the GraphQL backend. This configuration specifies:

- **Query**: GraphQL query for reading expense records with data operation support
- **ResolverName**: Backend resolver method name (e.g., `expenseRecordData`)
- **Mutation.Insert**: GraphQL mutation for creating new records (including row insertion)
- **Mutation.Update**: GraphQL mutation for updating existing records (including cell edits)
- **Mutation.Delete**: GraphQL mutation for deleting records (including row removal)
- **Mutation.Batch**: GraphQL mutation for batch operations (multiple add/edit/delete in a single request)

Refer to the [Full Documentation](#full-documentation) for complete GraphQL adaptor configuration examples.

## Project Layout

| File/Folder | Purpose |
|-------------|---------|
| `/Models/ExpenseRecord.cs` | Data model representing expense records |
| `/Models/GraphQLQuery.cs` | Query resolvers for read operations (GetExpenseRecordData) |
| `/Models/GraphQLMutation.cs` | Mutation resolvers for write operations (CreateExpense, UpdateExpense, DeleteExpense, BatchUpdate) |
| `/Models/DataMangerRequest.cs` | Input type for data operation parameters (skip, take, aggregates, paging, and pivot-specific request data) |
| `/Components/Pages/Home.razor` | Pivot Table page with GraphQL adaptor configuration, cell editing, and drill-through setup |
| `/Program.cs` | Service registration for Hot Chocolate GraphQL server and Syncfusion Blazor |
| `/Properties/launchSettings.json` | Port configuration for GraphQL endpoint |

## Common Tasks

### Add an Expense

1. Double-click an empty cell in the pivot view value area to open the editor
2. Enter a value for the new measure
3. Click **Update** to create the record via GraphQL mutation
4. The system automatically generates a unique ExpenseId (e.g., EXP1003)

### Edit an Expense

1. Double-click any value cell in the pivot view
2. Modify the value in the editor dialog
3. Click **Update** to save the change via GraphQL mutation
4. The corresponding expense record is updated in the underlying data source

### Delete an Expense

1. Right-click the row or cell to be removed
2. Select the delete option, or use the cell editing delete action
3. Confirm the deletion in the dialog
4. The record is removed via GraphQL mutation

### Drill-Through to Source Records

1. Double-click an aggregated value cell in the pivot view
2. A drill-through grid opens showing the underlying records
3. The primary key column is automatically configured (via the `BeginDrillThrough` event) so that CRUD operations from the drill-through grid target the correct record
4. Close the drill-through window to return to the pivot view

### Batch Operations

1. Make multiple cell-level changes to the pivot view (add, edit, delete)
2. Click **Update** to apply all changes in a single request
3. The batch mutation sends all changed, added, and deleted records to the backend in one request

## Troubleshooting

### GraphQL Endpoint Not Responding

- Verify the application is running: Check terminal for `Now listening on: https://localhost:xxxx`
- Confirm the URL in `SfDataManager` matches the running port
- Ensure `app.MapGraphQL()` is configured in `Program.cs`
- Check firewall settings if accessing from a different machine

### Static Files Not Loading (CSS/Scripts)

- Verify Syncfusion stylesheet is referenced in `Components/App.razor`:
  ```html
  <link href="_content/Syncfusion.Blazor.Themes/tailwind3.css" rel="stylesheet" />
  ```
- Verify Syncfusion scripts are referenced in `Components/App.razor`:
  ```html
  <script src="_content/Syncfusion.Blazor.Core/scripts/syncfusion-blazor.min.js" type="text/javascript"></script>
  ```
- Check browser developer tools (F12) for 404 errors on static resources

### CRUD Operations Not Working

- Verify the GraphQL endpoint URL in `Home.razor` `SfDataManager` matches the backend port
- Check browser console for GraphQL errors (Network tab → GraphQL requests)
- Ensure all GraphQL mutation resolvers are implemented in `GraphQLMutation.cs`
- Verify the `DataManagerRequestInput` class includes all required properties for pivot data operations
- When performing CRUD from the drill-through grid, confirm the `BeginDrillThrough` event handler marks the primary key column (`IsPrimaryKey = true`)

### Version Conflicts

- Align HotChocolate.AspNetCore and Syncfusion package versions
- Run `dotnet restore` to update NuGet packages
- Check the `Pivot_GraphQLAdaptor.csproj` file for conflicting version constraints
- Verify all packages are compatible with .NET 8.0 or later

### Port Already in Use

- Change the port numbers in `launchSettings.json` to available ports
- Or identify the process using the port: `Get-Process | Where-Object {$_.Handles -gt 500} | Select-Object Name, Id`
- Update the Pivot Table connection URL to match the new port

## Full Documentation

For detailed, step-by-step directions including complete code examples, GraphQL query structures, mutation implementations, and advanced configurations, refer to the official Syncfusion Blazor Pivot Table documentation. *(Documentation link will be added once published.)*
