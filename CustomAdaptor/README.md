# Blazor Pivot Table with Custom Adaptor

## Project Overview

This repository demonstrates a flexible pattern for binding data to **Syncfusion Blazor Pivot Table** using the **Custom Adaptor**. The sample application provides complete CRUD (Create, Read, Update, Delete) operations through cell editing, drill-through navigation, and the field list, all backed by an in-memory data source that is processed and transformed through a custom `DataAdaptor` implementation. The implementation follows the Syncfusion `DataAdaptor` contract, where the custom adaptor overrides the `Read`, `Insert`, `Update`, and `Remove` methods to provide full control over data retrieval, transformation, and persistence, and the Syncfusion `SfDataManager` is configured with `Adaptors.CustomAdaptor` on the client to wire the custom logic into the Pivot Table.

## Key Features

- **Custom Adaptor Implementation**: A `CustomAdaptor` class derived from `DataAdaptor` that exposes the full Pivot data pipeline through `Read`, `Insert`, `Update`, and `Remove` overrides
- **Syncfusion Blazor Pivot Table**: Field list, drill-through, and configurable rows, columns, and values for multidimensional analysis
- **Complete CRUD Operations**: Add, edit, and delete cells and rows directly from the pivot view, including updates through the drill-through grid
- **In-Memory Data Source**: Sample order data generated through the `OrderData` model and seeded once per process
- **Client-Side Operations**: Sorting, filtering, paging, and aggregation are processed inside the adaptor using `DataOperations` and `DataUtil` helpers
- **Drill-Through Primary Key Handling**: The `BeginDrillThrough` event marks the `OrderID` column as the primary key so that the drill-through grid can target the correct record for CRUD operations
- **No Backend Dependency**: All data operations are handled inside the Blazor application; no REST endpoints or controllers are required

## Prerequisites

| Component | Version | Purpose |
|-----------|---------|---------|
| Visual Studio 2022 | 17.0 or later | Development IDE with Blazor workload |
| .NET SDK | net9.0 or compatible | Runtime and build tools |
| Syncfusion.Blazor.PivotTable | 34.1.29 | Pivot Table and UI components |
| Syncfusion.Blazor.Themes | 34.1.29 | Styling for Pivot Table components |
| Syncfusion.Blazor.Data | 34.1.29 | Data adaptors including Custom Adaptor support |

## Quick Start

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd "syncfusion-blazor-pivot-table-custom-adaptor"
   
   cd "CustomAdaptor"
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
   
   Navigate to the local URL displayed in the terminal (typically `https://localhost:7180` or `http://localhost:5120`).

5. **Interact with the Pivot Table**
   
   Use the field list to configure rows, columns, and values, then double-click aggregated cells to open the drill-through editor and exercise the Insert, Update, and Delete operations provided by the Custom Adaptor.

## Configuration

### Application Port Configuration

The backend server port is configured in `Properties/launchSettings.json`. This file controls where the application runs.

**Important Notes:**
- The Custom Adaptor does not rely on any external REST endpoint; all CRUD operations are processed in-process by overriding the `DataAdaptor` methods.
- The default HTTP profile listens on `http://localhost:5120` and the HTTPS profile listens on `https://localhost:7180`.
- If you change the ports in `launchSettings.json`, no `Url` property needs to be updated in `Components/Pages/Home.razor` because the Custom Adaptor does not use a remote URL.

### Custom Adaptor Configuration

The `SfDataManager` configuration in `Home.razor` defines how the Pivot Table communicates with the custom data pipeline. This configuration specifies:

- **AdaptorInstance**: Set to `@typeof(CustomAdaptor)` so that the `SfDataManager` instantiates the `CustomAdaptor` class declared inside `Home.razor`
- **Adaptor**: Set to `Adaptors.CustomAdaptor` to activate the Custom Adaptor pipeline
- **In-Process Processing**: The `Read` override calls `OrderData.GetAllRecords()` and applies paging (skip/take) using the `DataOperations` helpers; the `Insert`, `Update`, and `Remove` overrides mutate the same in-memory list and return the affected record

The Custom Adaptor returns data in the following format when `RequiresCounts` is `true` (which the Pivot Table always requires):

```json
{
    "Result": [{ ... }, { ... }, { ... }],
    "Count": 100
}
```

For `RequiresCounts = false`, the adaptor returns the collection of records directly.

## Project Layout

| File/Folder | Purpose |
|-------------|---------|
| `/Components/Pages/OrderData.cs` | Data model representing order records, with sample data generation |
| `/Components/Pages/Home.razor` | Pivot Table page with Custom Adaptor configuration, cell editing, drill-through setup, and the `CustomAdaptor` class |
| `/Components/App.razor` | Root component that loads Syncfusion themes and scripts |
| `/Program.cs` | Service registration for ASP.NET Core and Syncfusion Blazor |
| `/Properties/launchSettings.json` | Port configuration for the local development server |
| `/appsettings.json`, `/appsettings.Development.json` | Logging and host configuration |

## Common Tasks

### Add an Order

1. Double-click an empty cell in the pivot view value area to open the editor
2. Enter a value for the new measure
3. Click **Update** to create the record via the Insert path
4. The system automatically adds the new order record to the in-memory data collection through the `CustomAdaptor.Insert` override

### Edit an Order

1. Double-click any value cell in the pivot view
2. Modify the value in the editor dialog
3. Click **Update** to save the change via the Update path
4. The corresponding order record is updated in the underlying data source through the `CustomAdaptor.Update` override

### Delete an Order

1. Right-click the row or cell to be removed
2. Select the delete option, or use the cell editing delete action
3. Confirm the deletion in the dialog
4. The record is removed via the `CustomAdaptor.Remove` override

### Drill-Through to Source Records

1. Double-click an aggregated value cell in the pivot view
2. A drill-through grid opens showing the underlying records
3. The primary key column is automatically configured (via the `BeginDrillThrough` event) so that CRUD operations from the drill-through grid target the correct record
4. Close the drill-through window to return to the pivot view

### Client-Side Sorting, Filtering, and Paging

1. Interact with the Pivot Table to apply sorting, filtering, or paging
2. The `DataManagerRequest` payload is passed to the `CustomAdaptor.Read` override
3. The override applies the skip and take operations on the in-memory collection using `DataOperations.PerformSkip` and `DataOperations.PerformTake`
4. The Pivot Table receives the processed page of data along with the total record count for accurate pagination

## Troubleshooting

### Pivot Table Not Rendering

- Verify the application is running: Check terminal for `Now listening on: http://localhost:5120`
- Confirm `app.MapRazorComponents<App>().AddInteractiveServerRenderMode()` is configured in `Program.cs`
- Ensure `builder.Services.AddSyncfusionBlazor()` is called before `var app = builder.Build()`

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

- Verify the `AdaptorInstance` in `Home.razor` `SfDataManager` points to the `CustomAdaptor` class declared in the same file: `AdaptorInstance="@typeof(CustomAdaptor)"`
- Confirm the `Adaptor` property is set to `Adaptors.CustomAdaptor`
- Check the browser console for JavaScript errors that may indicate the `syncfusion-blazor.min.js` script failed to load
- Ensure all `Read`, `Insert`, `Update`, and `Remove` overrides are implemented in the `CustomAdaptor` class
- Verify the `OrderData` model exposes the fields expected by the Pivot Table configuration
- When performing CRUD from the drill-through grid, confirm the `BeginDrillThrough` event handler marks the primary key column (`IsPrimaryKey = true`)

### Data Not Appearing in the Pivot Table

- Confirm `OrderData.GetAllRecords()` returns a non-empty list (it seeds 100 records on the first call)
- Verify the `TValue` on `SfPivotView` matches the type returned by the adaptor (`OrderData`)
- Ensure the field names configured in `PivotViewColumns`, `PivotViewRows`, and `PivotViewValues` exist on the `OrderData` model
- Check that the `CustomAdaptor.Read` override returns a `DataResult` with `Result` and `Count` populated when `RequiresCounts` is `true`

### Version Conflicts

- Align Syncfusion package versions (this sample uses `34.1.29` for all Syncfusion packages)
- Run `dotnet restore` to update NuGet packages
- Check the `CustomAdaptor.csproj` file for conflicting version constraints
- Verify all packages are compatible with .NET 9.0 or later

### Port Already in Use

- Change the port numbers in `launchSettings.json` to available ports
- Or identify the process using the port: `Get-Process | Where-Object {$_.Handles -gt 500} | Select-Object Name, Id`
- No `Url` update is required in `Components/Pages/Home.razor` because the Custom Adaptor does not use a remote endpoint

## Full Documentation

For detailed, step-by-step directions including complete code examples, adaptor implementations, and advanced configurations, refer to the official Syncfusion Blazor Pivot Table documentation. *(Documentation link will be added once published.)*
