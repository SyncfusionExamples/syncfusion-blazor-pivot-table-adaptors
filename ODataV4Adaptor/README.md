# 🟦 Blazor Pivot Table with OData V4 Adaptor

## Project Overview

This repository demonstrates a standards-based pattern for binding data to the **Syncfusion Blazor Pivot Table** using the built-in **OData V4 Adaptor**. The sample application provides complete CRUD (Create, Read, Update, Delete) operations through cell editing, drill-through navigation, and the field list, all backed by an **ASP.NET Core OData V4** service that exposes a queryable endpoint over an in-memory data source. The implementation follows the OData V4 specification, where the server registers an EDM model and enables `$filter`, `$orderby`, `$count`, and `$top` query options, and the Syncfusion `SfDataManager` is configured with `Adaptor="Adaptors.ODataV4Adaptor"` on the client to wire the pivot engine directly to the OData endpoint.

## Key Features

- **OData V4 Service**: A queryable endpoint built with `Microsoft.AspNetCore.OData` that supports `$filter`, `$orderby`, `$count`, and `$top` query options
- **Syncfusion Blazor Pivot Table**: Field list, drill-through, and configurable rows, columns, and values for multidimensional analysis
- **Complete CRUD Operations**: `GET`, `POST`, `PATCH`, and `DELETE` endpoints on the `Orders` entity set; add, edit, and delete cells and rows directly from the pivot view, including updates through the drill-through grid
- **In-Memory Data Source**: Sample order data generated through the `OrdersDetails` model and seeded once per process
- **Server-Side Query Composition**: Sorting, filtering, paging, and aggregation are processed on the server through the `[EnableQuery]` attribute and translated to LINQ
- **Drill-Through Primary Key Handling**: The `BeginDrillThrough` event marks the `OrderID` column as the primary key so that the drill-through grid can target the correct record for CRUD operations
- **Standards-Based Integration**: All data operations are routed through the OData endpoint; no custom adaptor logic is required

## Prerequisites

| Component | Version | Purpose |
|-----------|---------|---------|
| Visual Studio 2022 | 17.0 or later | Development IDE with ASP.NET and Blazor workloads |
| .NET SDK | net9.0 or compatible | Runtime and build tools |
| Syncfusion.Blazor.PivotTable | Latest available version | Pivot Table and UI components |
| Syncfusion.Blazor.Themes | Latest available version | Styling for Pivot Table components |
| Microsoft.AspNetCore.OData | Latest available version | OData V4 server-side framework |

## Quick Start

1. **Clone the repository**
   ```bash
   git clone https://github.com/SyncfusionExamples/syncfusion-blazor-pivot-table-remote-data-binding.git
   cd "ODataV4Adaptor"
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

   Navigate to the local URL displayed in the terminal (typically `https://localhost:7078` or `http://localhost:5217`).

5. **Interact with the Pivot Table**

   Use the field list to configure rows, columns, and values, then double-click aggregated cells to open the drill-through editor and exercise the Insert, Update, and Delete operations provided by the OData V4 service.

## Configuration

### Application Port Configuration

The backend server port is configured in `Properties/launchSettings.json`. This file controls where the application runs.

**Important Notes:**
- The OData V4 Adaptor relies on the OData endpoint exposed by the server; CRUD operations are processed on the server through the `OrdersController` endpoints.
- The default HTTP profile listens on `http://localhost:5217` and the HTTPS profile listens on `https://localhost:7078` (with HTTP fallback to `http://localhost:5217`).
- If you change the ports in `launchSettings.json`, update the `Url` property on the `SfDataManager` in `ODataV4Adaptor.Client/Pages/Index.razor` so the OData V4 Adaptor points to the new endpoint.

### OData V4 Adaptor Configuration

The `SfDataManager` configuration in `Index.razor` defines how the Pivot Table communicates with the OData service. This configuration specifies:

- **Url**: Set to `http://localhost:5217/odata/orders` so that the `ODataV4Adaptor` targets the `Orders` entity set registered in `Program.cs`
- **Adaptor**: Set to `Adaptors.ODataV4Adaptor` to activate the built-in OData V4 pipeline that translates Pivot operations into OData query options (`$filter`, `$orderby`, `$top`, `$skip`, `$count`) and HTTP methods

The OData V4 service registration in `Program.cs` enables the following query options:

- `Count()` — enables the `$count` query option
- `Filter()` — enables the `$filter` query option
- `OrderBy()` — enables the `$orderby` query option
- `SetMaxTop(recordCount)` — limits the maximum number of records returned using `$top`

The service is registered through `ODataConventionModelBuilder` and mounted under the `odata` route prefix:

```csharp
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EntitySet<OrdersDetails>("Orders");

builder.Services.AddControllers().AddOData(
    options => options
        .Count()
        .Filter()
        .OrderBy()
        .SetMaxTop(recordCount)
        .AddRouteComponents("odata", modelBuilder.GetEdmModel())
);
```

## Project Layout

| File/Folder | Purpose |
|-------------|---------|
| `/ODataV4Adaptor/Program.cs` | Service registration for ASP.NET Core, OData, and Syncfusion Blazor |
| `/ODataV4Adaptor/ODataV4Adaptor.csproj` | Server project file (net9.0) with OData package reference |
| `/ODataV4Adaptor/Controllers/OrdersController.cs` | OData endpoint with `[EnableQuery]` GET and POST/PATCH/DELETE CRUD operations |
| `/ODataV4Adaptor/Components/App.razor` | Root component that loads Syncfusion themes and scripts |
| `/ODataV4Adaptor/Properties/launchSettings.json` | Port configuration for the local development server |
| `/ODataV4Adaptor/appsettings.json`, `/ODataV4Adaptor/appsettings.Development.json` | Logging and host configuration |
| `/ODataV4Adaptor.Client/Models/OrdersDetails.cs` | Data model representing order records, with sample data generation |
| `/ODataV4Adaptor.Client/Pages/Index.razor` | Pivot Table page with OData V4 Adaptor configuration, cell editing, and drill-through setup |
| `/ODataV4Adaptor.Client/Program.cs` | Client host and Syncfusion Blazor service registration |

## OData Service Setup

### Entity Model

The `OrdersDetails` model defines the shape of the entity exposed by the OData endpoint:

- `OrderID` (int, primary key)
- `CustomerID` (string)
- `EmployeeID` (int?)
- `ShipCountry` (string)

A static seed method populates the in-memory list on first access and returns the same collection on subsequent calls.

### Endpoint Routes

The `OrdersController` exposes the following routes under the `odata` prefix:

| Method | Route | Purpose |
|--------|-------|---------|
| `GET` | `/odata/orders` | Returns the entity set with OData query options applied (`$filter`, `$orderby`, `$top`, `$skip`, `$count`) |
| `POST` | `/odata/orders` | Inserts a new order at the beginning of the in-memory list |
| `PATCH` | `/odata/orders/{key}` | Updates the matching order's `CustomerID`, `EmployeeID`, or `ShipCountry` |
| `DELETE` | `/odata/orders/{key}` | Removes the order with the matching `OrderID` |

The `GET` endpoint is decorated with `[EnableQuery]`, which allows the OData middleware to interpret query options from the request and apply them to the in-memory data source.

## Data Binding with Pivot Table

The Pivot Table is configured in `ODataV4Adaptor.Client/Pages/Index.razor` with the following structure:

- **Columns**: `OrderID`
- **Rows**: `ShipCountry`
- **Values**: `EmployeeID`

The `SfDataManager` is wired to the OData service through the built-in `ODataV4Adaptor`:

```razor
<SfDataManager Url="http://localhost:5217/odata/orders"
               Adaptor="Adaptors.ODataV4Adaptor">
</SfDataManager>
```

Cell editing is enabled to allow inline creation, editing, and deletion of pivot cells:

```razor
<PivotViewCellEditSettings AllowEditing="true"
                           AllowAdding="true"
                           AllowDeleting="true"
                           Mode="Syncfusion.Blazor.PivotView.EditMode.Normal">
</PivotViewCellEditSettings>
```

The `BeginDrillThrough` event marks `OrderID` as the primary key column so that the drill-through grid can target the correct record for `PATCH` and `DELETE` operations.

## Common Tasks

### Add an Order

1. Double-click an empty cell in the pivot view value area to open the editor
2. Enter a value for the new measure
3. Click **Update** to create the record via the Insert path
4. The system issues a `POST` request to `/odata/orders`; the server inserts the record and returns the new entity

### Edit an Order

1. Double-click any value cell in the pivot view
2. Modify the value in the editor dialog
3. Click **Update** to save the change via the Update path
4. The Pivot Table issues a `PATCH` request to `/odata/orders/{OrderID}`; the corresponding order record is updated in the underlying data source

### Delete an Order

1. Right-click the row or cell to be removed
2. Select the delete option, or use the cell editing delete action
3. Confirm the deletion in the dialog
4. The Pivot Table issues a `DELETE` request to `/odata/orders/{OrderID}`; the record is removed via the controller endpoint

### Drill-Through to Source Records

1. Double-click an aggregated value cell in the pivot view
2. A drill-through grid opens showing the underlying records
3. The primary key column is automatically configured (via the `BeginDrillThrough` event) so that CRUD operations from the drill-through grid target the correct record
4. Close the drill-through window to return to the pivot view

### Server-Side Sorting, Filtering, and Paging

1. Interact with the Pivot Table to apply sorting, filtering, or paging
2. The OData V4 Adaptor translates the request into `$orderby`, `$filter`, `$top`, and `$skip` query options
3. The server applies the query options to the in-memory collection through the `[EnableQuery]` attribute
4. The Pivot Table receives the processed page of data along with the total record count for accurate pagination

## Troubleshooting

### Pivot Table Not Rendering

- Verify the application is running: Check terminal for `Now listening on: http://localhost:5217`
- Confirm `app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode()` is configured in `Program.cs`
- Ensure `builder.Services.AddSyncfusionBlazor()` is called before `var app = builder.Build()`
- Confirm `AddControllers().AddOData(...)` is registered in the service collection

### OData Endpoint Not Reachable (404)

- Verify the entity set is registered in `Program.cs` with `modelBuilder.EntitySet<OrdersDetails>("Orders")`
- Confirm the route components are added under the `odata` prefix: `AddRouteComponents("odata", modelBuilder.GetEdmModel())`
- Check the `Url` on the `SfDataManager` in `Index.razor` matches the OData route (for example, `http://localhost:5217/odata/orders`)
- Test the endpoint directly in a browser or REST client: `http://localhost:5217/odata/orders?$count=true`

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

- Verify the `Adaptor` on the `SfDataManager` in `Index.razor` is set to `Adaptors.ODataV4Adaptor`
- Confirm the `Url` points to the `Orders` entity set route (for example, `http://localhost:5217/odata/orders`)
- Check the browser console for JavaScript errors that may indicate the `syncfusion-blazor.min.js` script failed to load
- Verify the `OrdersController` exposes `GET`, `POST`, `PATCH`, and `DELETE` actions and is mapped through `app.MapControllers()` in `Program.cs`
- Verify the `OrdersDetails` model exposes the fields expected by the Pivot Table configuration
- When performing CRUD from the drill-through grid, confirm the `BeginDrillThrough` event handler marks the primary key column (`IsPrimaryKey = true`)

### Data Not Appearing in the Pivot Table

- Confirm `OrdersDetails.GetAllRecords()` returns a non-empty list (it seeds 45 records on the first call)
- Verify the `TValue` on `SfPivotView` matches the type returned by the OData endpoint (`OrdersDetails`)
- Ensure the field names configured in `PivotViewColumns`, `PivotViewRows`, and `PivotViewValues` exist on the `OrdersDetails` model
- Test the OData endpoint directly: `http://localhost:5217/odata/orders` should return a JSON array of records
- Check that `$count` returns the expected total record count: `http://localhost:5217/odata/orders/$count`

### Version Conflicts

- Align Syncfusion package versions across the server and client projects (use the latest available stable release for all Syncfusion packages)
- Run `dotnet restore` to update NuGet packages
- Check the `ODataV4Adaptor.csproj` and `ODataV4Adaptor.Client.csproj` files for conflicting version constraints
- Verify all packages are compatible with .NET 9.0 or later

### Port Already in Use

- Change the port numbers in `launchSettings.json` to available ports
- Or identify the process using the port: `Get-Process | Where-Object {$_.Handles -gt 500} | Select-Object Name, Id`
- Update the `Url` on the `SfDataManager` in `ODataV4Adaptor.Client/Pages/Index.razor` to match the new HTTP port, since the OData V4 Adaptor points to a remote endpoint

## Full Documentation

For detailed, step-by-step directions including complete code examples, OData service configurations, and advanced Pivot Table setups, refer to the official Syncfusion Blazor Pivot Table documentation. *(Documentation link will be added once published.)*
