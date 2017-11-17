# DoddleImport

DoddleImport will add automatic importing of data to and from a variety of sources.

### Objective
* Flexible importing of data via a fluid and fully extensible API to easily add support for new Import Sources and Import Destinations.

### Key Components
Importing relies on 2 distinct (and entirely extensible) mechanisms: 
* `IImportSource` - The import framework requires a set of row/field data provided via an IImportSource
* `IImportDestination` - The import framework requires a destination to write the data to via an IImportDestination

Note: Most "importable" classes implement both `IImportSource` as well as `IImportDestination`, as such, can be used as a source of import rows and a destination to import into

### Import Sources and Destinations
DoddleImport ships with some basic Import Sources, but just like all other Doddle Projects, you can easily create your own by implementing a simple interface.
* `ImportableCollection` - provides importing to and from a generic list
* `ImportableDictionary` - provides importing to and from a basic dictionary
* `ImportableSPList` - provides importing to and from a SharePoint List
* `Spreadsheet` - provides importing from an Excel Spreadsheet


### Usage

```csharp
// Basic import from one type of in-memory collection to another
ImportableDictionary source = new ImportableDictionary();
source.Fields.Add("ProductID", typeof(int));
source.Fields.Add("ProductName", typeof(string));

IDictionary row1 = source.AddRow();
row1["ProductID"] = 1;
row1["ProductName"] = "My Product";

List<Product> products = new List<Product>();
IImportDestination destination = new ImportableCollection<Product>(products);

Importer importer = new Importer();
importer.Import(source, destination);
```

Importing an Excel Spreadsheet into a SharePoint list

```csharp
// More realistic example, importing an Excel Spreadsheet into a SharePoint list
Stream uploadedFile = fileUpload.PostedFile.InputStream;
Spreadsheet spreadsheet = new Spreadsheet(uploadedFile);

SPList myList = Web.Lists["My List"];
IImportDestination destination = new ImportableSPList(myList);

Importer importer = new Importer();
importer.Import(spreadsheet, destination);
```

### Validation
DoddleImport will provide automatic validation of your Import Source against the specified destination. This ensures that the entire import contents will be successfully imported as well as provide helpful error messages to your user so they can easily correct errors.

#### Validation Rules
The validation mechanism works by automatically evaluating rules against each row being imported. This is handled through the IValidationRule interface.

The default rules with DoddleImport are as follows:
* `RequiredFieldsRule` - validates a row to make sure that data was provided for all required fields
* `MissingHeadersRule` - validates that the source contains all required fields that the destination expects
* `DataTypeValidationRule` - valdiates data type mis-match errors

### Configuration
If you want to change some of th default behavior of DoddleImport then using your application configuration file is the best place to start.

To use configuration, be sure to register the `<section>` node between `<configSections>` in your app.Config or web.Config

From there, you can add or remove validation rules that will be automatically applied to every import. You are also able to customize the default validation messages. Below is a quick sample of some of the changes that can be made. 

```xml
<configuration>
  <configSections>
    <section name="doddleImport" 
          type="Doddle.Import.Configuration.ImportSection, Doddle.Import, Version=1.0.0.0, Culture=neutral, PublicKeyToken=6f5f0fd458d019c9" />
  </configSections>

  <doddleImport>
    <validation>
      <rules>
        <remove name="MissingHeadersRule" />
        <add name="MyCustomRule" type="MyName.Importing.CustomValidationRule, MyName.Importing" />
      </rules>
      <messages>
        <remove rule="RequiredFieldsRule" />
        <add rule="RequiredFieldsRule" message="Unable to locate field '{0}'"/>
      </messages>
    </validation>
  </doddleImport>
</configuration>
```
