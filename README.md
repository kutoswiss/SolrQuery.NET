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

    // ---------------------------------------------
    // Example 1: Default SOLR search 
    // ---------------------------------------------
    solrQuery.Common.BindValue(SolrCommon.param_e.q, "*", "*");


    SolrResults solrResults = solrQuery.Execute();
    foreach (SolrItemResult i in solrResults.Response.docs)
        Console.WriteLine(i.Json.ToString());
}
```

## Dependencies
* [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
