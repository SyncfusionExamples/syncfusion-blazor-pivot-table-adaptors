# Binding-data-from-remote-service-to-blazor-pivot-table

## Overview

This repository demonstrates how to bind data from various remote services to a **Syncfusion Blazor Pivot Table**. It provides examples of integrating data using different adaptors and showcases techniques for handling aggregation, filtering, sorting, paging, drill-through, and CRUD operations within Blazor applications. By exploring this repository, you will learn how to efficiently manage and analyze large volumes of data in your applications.

## Features

**Remote Data Integration:** Seamlessly bind data from various remote services to the Syncfusion Blazor Pivot Table.

**Server-Side Data Operations:** Handle aggregation, filtering, sorting, and paging operations on the server for scalable performance.

**CRUD Operations:** Implement Create, Read, Update, and Delete operations directly from the pivot view, including through the drill-through grid.

**Drill-Through Navigation:** Double-click aggregated cells to view and edit the underlying source records.

**Field List and Grouping Bar:** Configure rows, columns, values, and filters at runtime.

**Adaptors:** Examples of different adaptors to integrate remote data.

* UrlAdaptor
* ODataV4Adaptor
* WebApiAdaptor
* GraphQLAdaptor
* CustomAdaptor

## Prerequisites

Before you begin, ensure you have met the following requirements:

* .NET SDK installed (Latest Available Version)
* Visual Studio 2022 or later with the ASP.NET and web development workload
* Basic knowledge of Blazor and web development
* Syncfusion Blazor packages (Latest Available Syncfusion Package)
* For the OData sample: Microsoft.AspNetCore.OData (Latest Available Version)
* For the GraphQL sample: HotChocolate.AspNetCore (Latest Available Version)

## Getting Started

**1. Clone the Repository**

Use `git clone` to fetch the repository from GitHub.

```bash
git clone https://github.com/SyncfusionExamples/syncfusion-blazor-pivot-table-remote-data-binding.git
```

**2. Choose a sample**

Each adaptor sample is self-contained and lives in its own folder. Open a terminal in the folder of the sample you want to run. For example:

```bash
cd WebApiAdaptor
```

The available sample folders are:

* `WebApiAdaptor`
* `UrlAdaptor`
* `ODataV4Adaptor`
* `GraphQLAdaptor`
* `CustomAdaptor`

**3. Open and Build the Project**

* Open the project in Visual Studio.
* Build the project to restore dependencies and compile it.
* Run the project.

## Running the Application

After building the project, run it from the sample folder:

```powershell
dotnet restore
dotnet build
dotnet run
```

Once the host is ready, the terminal prints the listening URLs. Open the HTTPS or HTTP URL in your browser to interact with the Pivot Table.

To confirm successful data binding:

1. Verify the pivot view populates with aggregated values on load.
2. Drag fields between **Rows**, **Columns**, **Values**, and **Filters** in the field list.
3. Double-click an aggregated cell to open the drill-through editor and confirm the underlying records load.
4. Exercise **Add**, **Edit**, and **Delete** from the cell editor to confirm CRUD operations round-trip to the configured endpoint.

## Resources

You can also refer the below resources to know more details about Syncfusion Blazor Pivot Table.

* [Demo](https://blazor.syncfusion.com/demos/pivot-table/overview?theme=fluent2)
* [Documentation](https://blazor.syncfusion.com/documentation/pivot-table/getting-started)
* [Connecting to adaptors](https://blazor.syncfusion.com/documentation/pivot-table/connecting-to-adaptors)
