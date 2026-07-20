# Blazor Pivot Table with Web API Adaptor

## Project Overview

This repository demonstrates a production-ready pattern for binding remote data to **Syncfusion Blazor Pivot Table** using the **Web API Adaptor**. The sample application provides complete CRUD (Create, Read, Update, Delete) operations through cell editing, drill-through navigation, and the field list, all backed by a RESTful API controller that handles data operations through standard HTTP endpoints. The implementation follows industry best practices using ASP.NET Core Web API controllers, OData-style query parameters for handling pivot data operations, and the Syncfusion `SfDataManager` with `WebApiAdaptor` configuration on the client.

## Key Features

- **ASP.NET Core Web API Integration**: RESTful endpoints for read and write operations exposed through `OrdersController`
- **Syncfusion Blazor Pivot Table**: Field list, drill-through, and configurable rows, columns, and values for multidimensional analysis
- **Complete CRUD Operations**: Add, edit, and delete cells and rows directly from the pivot view, including batch updates
- **Web API Adaptor**: OData-style query support for read operations (`$orderby`, `$filter`, `$skip`, `$top`) processed server-side, with standard HTTP verbs (`GET`, `POST`, `PUT`, `DELETE`) for write operations
- **Server-Side Operations**: Sorting, filtering, and paging are pushed to the server, enabling efficient handling of large datasets
- **Configurable Backend Endpoint**: Server port managed via `launchSettings.json`

## Prerequisites

| Component | Version | Purpose |
|-----------|---------|---------|
| Visual Studio 2022 | 17.0 or later | Development IDE with Blazor workload |
| .NET SDK | net9.0 or compatible | Runtime and build tools |
| Syncfusion.Blazor.PivotTable | Latest | Pivot Table and UI components |
| Syncfusion.Blazor.Themes | Latest | Styling for Pivot Table components |
| Syncfusion.Blazor.Data | Latest | Data adaptors including Web API Adaptor support |

## Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd "syncfusion-blazor-pivot-table-remote-data-binding"
   
   cd "WebApiAdaptor"
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
   
   Navigate to the local URL displayed in the terminal (typically `https://localhost:7167` or `http://localhost:5198`).

5. **Access the Web API (Optional)**
   
   Navigate to `http://localhost:5198/api/orders` to test the read endpoint, or use tools like Postman or Swagger to explore the CRUD endpoints manually.

## Configuration

### Web API Adaptor Endpoint Configuration

The backend server port is configured in `Properties/launchSettings.json`. This file controls where the application runs and where the REST API endpoints are exposed.

**Important Notes:**
- The Web API Adaptor endpoints are automatically wired up by the controller routes defined in `Controllers/OrdersController.cs`:
  - `GET /api/orders` — Read endpoint that accepts OData query parameters (`$orderby`, `$filter`, `$skip`, `$top`) for server-side sorting, filtering, and paging
  - `POST /api/orders` — Create a new record
  - `PUT /api/orders` — Update an existing record
  - `DELETE /api/orders/{id}` — Delete a record by primary key
- The Pivot Table connection URL in `Components/Pages/Home.razor` is set to `http://localhost:5198/api/orders`, which matches the default HTTP profile. No port change is required for the default configuration.
- If you change the port in `launchSettings.json`, update the `Url` property of `SfDataManager` in `Components/Pages/Home.razor` to match the new port.

### Web API Adaptor Configuration

The `SfDataManager` configuration in `Home.razor` defines how the Pivot Table communicates with the REST backend. This configuration specifies:

- **Url**: REST endpoint for reading order records; the Web API Adaptor automatically appends OData query parameters (`$orderby`, `$filter`, `$skip`, `$top`) based on the user's interactions with the Pivot Table
- **Adaptor**: Set to `Adaptors.WebApiAdaptor` to enable OData-style query support for remote data binding
- **Server-Side Processing**: Sorting, filtering, and paging are applied on the server using the OData query string parameters, reducing payload size and improving performance

The Web API Adaptor is an extension of the OData Adaptor. It expects the server to return data in the following format:

```json
{
    "Items": [{ ... }, { ... }, { ... }],
    "Count": 45
}
```

## Project Layout

| File/Folder | Purpose |
|-------------|---------|
| `/Models/OrdersDetails.cs` | Data model representing order records, with sample data generation |
| `/Controllers/OrdersController.cs` | REST API controller exposing read and CRUD endpoints for the Pivot Table, including OData query parsing |
| `/Components/Pages/Home.razor` | Pivot Table page with Web API Adaptor configuration, cell editing, and drill-through setup |
| `/Program.cs` | Service registration for ASP.NET Core, Syncfusion Blazor, and MVC controllers |
| `/Properties/launchSettings.json` | Port configuration for the local development server |

## Common Tasks

### Add an Order

1. Double-click an empty cell in the pivot view value area to open the editor
2. Enter a value for the new measure
3. Click **Update** to create the record via the Insert endpoint
4. The system automatically adds the new order record to the in-memory data collection through the `POST` action

### Edit an Order

1. Double-click any value cell in the pivot view
2. Modify the value in the editor dialog
3. Click **Update** to save the change via the Update endpoint
4. The corresponding order record is updated in the underlying data source through the `PUT` action

### Delete an Order

1. Right-click the row or cell to be removed
2. Select the delete option, or use the cell editing delete action
3. Confirm the deletion in the dialog
4. The record is removed via the `DELETE` endpoint

### Drill-Through to Source Records

1. Double-click an aggregated value cell in the pivot view
2. A drill-through grid opens showing the underlying records
3. The primary key column is automatically configured (via the `BeginDrillThrough` event) so that CRUD operations from the drill-through grid target the correct record
4. Close the drill-through window to return to the pivot view

### Server-Side Sorting, Filtering, and Paging

1. Interact with the Pivot Table to apply sorting, filtering, or paging
2. The Web API Adaptor automatically appends OData query parameters (`$orderby`, `$filter`, `$skip`, `$top`) to the request URL
3. The `OrdersController` parses these parameters and applies the corresponding operations on the server before returning the data
4. The Pivot Table receives only the processed page of data along with the total record count for accurate pagination

## Troubleshooting

### API Endpoint Not Responding

- Verify the application is running: Check terminal for `Now listening on: http://localhost:5198`
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

- Verify the endpoint URL in `Home.razor` `SfDataManager` matches the backend port
- Check browser console for HTTP errors (Network tab → fetch/XHR requests)
- Ensure all controller action methods are implemented in `OrdersController.cs`
- Verify the `OrdersDetails` model is correctly bound for POST and PUT operations
- When performing CRUD from the drill-through grid, confirm the `BeginDrillThrough` event handler marks the primary key column (`IsPrimaryKey = true`)

### OData Query Parameters Not Being Applied

- Confirm the request URL includes `$orderby`, `$filter`, `$skip`, or `$top` parameters when sorting, filtering, or paging
- Verify the `OrdersController` `GetOrderData` action reads the query string via `Request.Query`
- Check the response includes both `Items` and `Count` properties as required by the Web API Adaptor
- Ensure the `Adaptor` property in `SfDataManager` is set to `Adaptors.WebApiAdaptor`

### Version Conflicts

- Align Syncfusion package versions
- Run `dotnet restore` to update NuGet packages
- Check the `WebApiAdaptor.csproj` file for conflicting version constraints
- Verify all packages are compatible with .NET 9.0 or later

### Port Already in Use

- Change the port numbers in `launchSettings.json` to available ports
- Or identify the process using the port: `Get-Process | Where-Object {$_.Handles -gt 500} | Select-Object Name, Id`
- Update the `Url` property in `Components/Pages/Home.razor` to match the new port

## Full Documentation

For detailed, step-by-step directions including complete code examples, controller implementations, and advanced configurations, refer to the official Syncfusion Blazor Pivot Table documentation. *(Documentation link will be added once published.)*
