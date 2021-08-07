# BlazingDocs C#, .NET client
High-performance document generation API. Generate documents and reports from СSV, JSON, XML with 99,9% uptime and 24/7 monitoring.

## Installation

Run this line from Package Manager Console:

```
Install-Package BlazingDocs -Version 1.0.0
```

## Integration basics

### Setup

You can get your API Key at https://app.blazingdocs.com

```c#
var client = new BlazingClient("api-key");
```

### Getting account info

```c#
AccountModel account = await _client.GetAccountAsync();
```

### Getting merge templates list

```c#
List<TemplateModel> templates = await _client.GetTemplatesAsync();
```

### Getting usage info

```c#
UsageModel usage = await _client.GetUsageAsync();
```

### Executing merge

```c#
using (var source = File.OpenRead("PO-Template.docx"))
{
    using (var reader = new StreamReader("PO-Template.json", Encoding.UTF8))
    {
        var data = reader.ReadToEnd();

        var parameters = new MergeParameters
        {
            Sequence = false, // data is object
            DataSourceType = DataSourceType.Json, // data in json format
            Strict = true // keep json types
        };

        var template = new FormFile("PO-Template.docx", source);

        var result = await _client.MergeAsync(data, "output.pdf", parameters, template);

        foreach (var file in result.Files)
        {
            using (var stream = new FileStream(file.Name, FileMode.Create))
            {
                file.SaveToAsync(stream).Wait();
            }
        }
    }
}
```

## Documentation

See more details here https://docs.blazingdocs.com