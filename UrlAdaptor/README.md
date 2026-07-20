# Blazor Pivot Table with URL Adaptor

## Project Overview

This repository demonstrates a production-ready pattern for binding remote data to **Syncfusion Blazor Pivot Table** using the **URL Adaptor**. The sample application provides complete CRUD (Create, Read, Update, Delete) operations through cell editing, drill-through navigation, and the field list, all backed by a RESTful API controller that handles data operations through standard HTTP endpoints. The implementation follows industry best practices using ASP.NET Core Web API controllers, `DataManagerRequest` for handling pivot data operations, and the Syncfusion `SfDataManager` with `UrlAdaptor` configuration on the client.

## Key Features

- **ASP.NET Core Web API Integration**: RESTful endpoints for read and write operations exposed through `OrdersController`
- **Syncfusion Blazor Pivot Table**: Field list, drill-through, and configurable rows, columns, and values for multidimensional analysis
- **Complete CRUD Operations**: Add, edit, and delete cells and rows directly from the pivot view, including batch updates
- **URL Adaptor**: Full control over pivot data operations (read and write) via dedicated Insert, Update, Remove, and Batch endpoints
- **DataManagerRequest**: Structured request object for handling complex pivot data operations, including search, filter, sort, and paging parameters
- **Configurable Backend Endpoint**: Server port managed via `launchSettings.json`

## Prerequisites

| Component | Version | Purpose |
|-----------|---------|---------|
| Visual Studio 2022 | 17.0 or later | Development IDE with Blazor workload |
| .NET SDK | net9.0 or compatible | Runtime and build tools |
| Syncfusion.Blazor.PivotTable | Latest | Pivot Table and UI components |
| Syncfusion.Blazor.Themes | Latest | Styling for Pivot Table components |
| Syncfusion.Blazor.Data | Latest | Data adaptors including URL Adaptor support |

## Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd "syncfusion-blazor-pivot-table-remote-data-binding"
   
   cd "URLAdaptor"
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
   
   Navigate to the local URL displayed in the terminal (typically `https://localhost:7169` or `http://localhost:5145`).

5. **Access the Web API (Optional)**
   
   Navigate to `http://localhost:5145/api/orders` to test the read endpoint, or use tools like Postman or Swagger to explore the CRUD endpoints manually.

## Configuration

### URL Adaptor Endpoint Configuration

The backend server port is configured in `Properties/launchSettings.json`. This file controls where the application runs and where the REST API endpoints are exposed.

**Important Notes:**
- The URL Adaptor endpoints are automatically wired up by the controller routes defined in `Controllers/OrdersController.cs`:
  - `GET/POST /api/orders` — Read endpoint for fetching and processing pivot data
  - `POST /api/orders/Insert` — Create a new record
  - `POST /api/orders/Update` — Update an existing record
  - `POST /api/orders/Remove` — Delete a record
  - `POST /api/orders/CrudUpdate` — Single-request CRUD dispatcher
  - `POST /api/orders/BatchUpdate` — Batch operations (multiple add/edit/delete in one request)
- The Pivot Table connection URL in `Components/Pages/Index.razor` is set to `http://localhost:5145/api/orders`, which matches the default HTTP profile. No port change is required for the default configuration.
- If you change the port in `launchSettings.json`, update the `Url`, `InsertUrl`, `UpdateUrl`, and `RemoveUrl` properties of `SfDataManager` in `Components/Pages/Index.razor` to match the new port.

### URL Adaptor Configuration

The `SfDataManager` configuration in `Index.razor` defines how the Pivot Table communicates with the REST backend. This configuration specifies:

- **Url**: REST endpoint for reading order records with data operation support (search, filter, sort, paging)
- **InsertUrl**: Endpoint for creating new records (including row insertion)
- **UpdateUrl**: Endpoint for updating existing records (including cell edits)
- **RemoveUrl**: Endpoint for deleting records (including row removal)
- **Adaptor**: Set to `Adaptors.UrlAdaptor` to enable URL-based data binding
- **CrudUrl**: Optional single endpoint that dispatches CRUD actions (`insert`, `update`, `remove`) based on the `action` property
- **BatchUrl**: Endpoint for batch operations that processes added, changed, and deleted records in a single request

Refer to the [Full Documentation](#full-documentation) for complete URL Adaptor configuration examples.

## Project Layout

| File/Folder | Purpose |
|-------------|---------|
| `/Models/OrderDetails.cs` | Data model representing order records, with sample data generation |
| `/Controllers/OrdersController.cs` | REST API controller exposing read and CRUD endpoints for the Pivot Table |
| `/Components/Pages/Index.razor` | Pivot Table page with URL Adaptor configuration, cell editing, and drill-through setup |
| `/Program.cs` | Service registration for ASP.NET Core, Syncfusion Blazor, and MVC controllers |
| `/Properties/launchSettings.json` | Port configuration for the local development server |

## Common Tasks

### Add an Order

1. Double-click an empty cell in the pivot view value area to open the editor
2. Enter a value for the new measure
3. Click **Update** to create the record via the Insert endpoint
4. The system automatically adds the new order record to the in-memory data collection

### Edit an Order

1. Double-click any value cell in the pivot view
2. Modify the value in the editor dialog
3. Click **Update** to save the change via the Update endpoint
4. The corresponding order record is updated in the underlying data source

### Delete an Order

1. Right-click the row or cell to be removed
2. Select the delete option, or use the cell editing delete action
3. Confirm the deletion in the dialog
4. The record is removed via the Remove endpoint

### Drill-Through to Source Records

1. Double-click an aggregated value cell in the pivot view
2. A drill-through grid opens showing the underlying records
3. The primary key column is automatically configured (via the `BeginDrillThrough` event) so that CRUD operations from the drill-through grid target the correct record
4. Close the drill-through window to return to the pivot view

### Batch Operations

1. Make multiple cell-level changes to the pivot view (add, edit, delete)
2. Click **Update** to apply all changes in a single request
3. The batch endpoint processes all added, changed, and deleted records from the batch model in one request

## Troubleshooting

### API Endpoint Not Responding

- Verify the application is running: Check terminal for `Now listening on: http://localhost:5145`
- Confirm the URLs in `SfDataManager` match the running port
- Ensure `app.MapControllers()` is configured in `Program.cs`
- Check firewall settings if accessing from a different machine

### Static Files Not Loading (CSS/Scripts)

- Verify Syncfusion stylesheet is referenced in `Components/App.razor`:
  ```html
  <link href="_content/Syncfusion.Blazor.Themes/bootstrap5.css" rel="stylesheet" />
  ```
- Verify Syncfusion scripts are referenced in `Components/App.razor`:
  ```html
  <script src="_content/Syncfusion.Blazor.Core/scripts/syncfusion-blazor.min.js" type="text/javascript"></script>
  ```
- Check browser developer tools (F12) for 404 errors on static resources

### CRUD Operations Not Working

- Verify the endpoint URLs in `Index.razor` `SfDataManager` match the backend port
- Check browser console for HTTP errors (Network tab → fetch/XHR requests)
- Ensure all controller action methods are implemented in `OrdersController.cs`
- Verify the `DataManagerRequest` parameter is correctly bound for read operations
- When performing CRUD from the drill-through grid, confirm the `BeginDrillThrough` event handler marks the primary key column (`IsPrimaryKey = true`)

### Version Conflicts

- Align Syncfusion package versions
- Run `dotnet restore` to update NuGet packages
- Check the `URLAdaptor.csproj` file for conflicting version constraints
- Verify all packages are compatible with .NET 9.0 or later

### Port Already in Use

- Change the port numbers in `launchSettings.json` to available ports
- Or identify the process using the port: `Get-Process | Where-Object {$_.Handles -gt 500} | Select-Object Name, Id`
- Update the `Url`, `InsertUrl`, `UpdateUrl`, and `RemoveUrl` properties in `Components/Pages/Index.razor` to match the new port

## Full Documentation

For detailed, step-by-step directions including complete code examples, controller implementations, and advanced configurations, refer to the official Syncfusion Blazor Pivot Table documentation. *(Documentation link will be added once published.)*
