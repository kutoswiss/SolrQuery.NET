# SolrQuery.NET [![Build Status](https://travis-ci.org/kutoswiss/solr-query-dotnet.svg?branch=master)](https://travis-ci.org/kutoswiss/solr-query-dotnet)

A simple library written in C# to perform queries on Solr search engine

## Installation
Coming soon

## Simple usage

```csharp
static void Main(string[] args)
{
    const String SOLR_DB_NAME = "collection_name";
    const String SOLR_URL = "mysolr.com";
    const int SOLR_PORT = 1234;
    
    SolrQuery solrQuery = new SolrQuery(SOLR_DB_NAME, SOLR_URL, SOLR_PORT);

    // Set the query
    solrQuery.Common.SetResponseWriter(SolrCommon.ResponseWriter.json); // Json format 
    solrQuery.Common.BindValue(SolrCommon.Parameter.q, "*", "*"); // Set the following query: q=*:*
    solrQuery.Common.BindValue(SolrCommon.Parameter.fl, QueryOperator.AND, "id", "/name"); // Set filters

    // Print results
    SolrResults solrResults = solrQuery.Execute();

    foreach (SolrItemResult i in solrResults.Response.docs)
    {
        Console.WriteLine(i.ToString()); // Print all the results
        Console.WriteLine(i["id"]); // Print the "id" value
        Console.WriteLine(i["/name"]); // Print the "/name" field
    }

    Console.ReadKey();
}
```

## Dependencies
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
