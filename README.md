
# Elastic Search Project

This Project was created using ASP.NET, [Elastic Search](https://www.elastic.co/) engin and [TomTom](https://www.tomtom.com/). 
## Screenshots
### Main window of ASP program
![Main window](/ScreenShots/Elastic.png?raw=true)
### View when we open some property
![View window](/ScreenShots/Screenshot_1.png?raw=true)
### Panel to select fields for search
![Filters panel](/ScreenShots/Screenshot_2.png?raw=true)
  
  
## Run Locally
#### Step 1)
Download Elastic program - [Download](https://www.elastic.co/downloads/elasticsearch)
#### Step 2)
Run file
```bash
  ../elasticsearch-7.14.0/bin/elasticsearch.bat
```
#### Step 3)
Open solution 
#### Step 4)
If you have file to import
```bash
  curl -X PUT "localhost:9200/sample_data/_bulk?pretty" -H 'Content-Type: application/x-ndjson' --data-binary @sample.json
```
**sample_data** - specyfic index that will be used in project
more information about import you can find [here](https://kb.objectrocket.com/elasticsearch/how-to-bulk-import-into-elasticsearch-using-curl)

### You can import properties.json from folder TestData using ElasticSearchUtil project:
Enter data in file Program.cs in Main()
```bash
    var path = @"..\TestData\properties.json";
    var indexName = "property";
```
**path** - path to the json array file\
**indexName** - index that will be register in Elastic Search


#### Step 5)
In project ElasticSearchProject in file appsettings.json enter **Tom Tom Key** and **Elastic Search Index**
```bash  
  "TomTomKey": "<Key>",
  "ElasticSearchIndex": "<Index>" 
``` 
**TomTomKey** - you can get in from [Developer tomtom](https://developer.tomtom.com/)\
**Index** - index that was registered in Elastic search\
You can chek them:
```bash  
  http://localhost:9200/_aliases?pretty=true
``` 




## Usage/Examples
To filter information please create request in **ElasticsearchExtension** 
```c# 
public static ISearchResponse<T> MatchAll<T>(ElasticClient client, string query, int from = 0, int size = 1) where T : class
{
  var results = client.Search<T>(s => s
     .Query(q => q.MatchAll()).From(from).Size(size));
  return results;
}
```
Parameters to use:
**ElasticClient client**  - instance of ElasticClient\
**string query** - word to find\
**int from** - start range to return results\
**int size** - count of results\
**Expression<Func<T, object>> field** - field to Search\

#### **Example call**
```C#
  var property = ElasticSearch.Search<Property>(_client, id.ToString(), f => f.propertyID, 10, 5); 
```

