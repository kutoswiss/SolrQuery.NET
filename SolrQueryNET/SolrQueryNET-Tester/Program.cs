using SolrQueryNET;
using SolrQueryNET.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolrQueryNET_Tester
{
    class Program
    {
        const String SOLR_DB_NAME = "cordra";
        const String SOLR_URL = "doa16.itu.int";
        const int SOLR_PORT = 8983;

        static void SimpleExample()
        {
            // Initialize the query
            SolrQuery solrQuery = new SolrQuery(SOLR_DB_NAME, SOLR_URL, SOLR_PORT);

            // Set the query
            solrQuery.Common.SetResponseWriter(SolrCommon.ResponseWriter.json); // Json format 
            solrQuery.Common.BindValue(SolrCommon.Parameter.q, "*", "*"); // Set the following query: q=*:*
            solrQuery.Common.BindValue(SolrCommon.Parameter.fl, QueryOperator.AND, "id", "/name"); // Set filters

            // Get and print results
            SolrResults solrResults = solrQuery.Execute();
            foreach (SolrItemResult i in solrResults.Response.docs)
            {
                Console.WriteLine(i.ToString()); // Print all the results
                Console.WriteLine(i["id"]); // Print the "id" value
                Console.WriteLine(i["/name"]); // Print the "/name" field
            }

            Console.ReadKey();
        }

        static void FacetExample()
        {
            // Initialize the query
            SolrQuery solrQuery = new SolrQuery(SOLR_DB_NAME, SOLR_URL, SOLR_PORT);
            
            // Set the query
            solrQuery.Common.SetResponseWriter(SolrCommon.ResponseWriter.json); // Json format 
            solrQuery.Common.ReplaceValue(SolrCommon.Parameter.rows, "0");
            solrQuery.Common.BindValue(SolrCommon.Parameter.q, "\\/keywords_s", "cloud", true);

            // Set facet parameters
            solrQuery.Facet.isEnabled = true;
            solrQuery.Facet.BindValue(SolrFacet.Parameter.field, "/keywords_s");
            solrQuery.Facet.BindValue(SolrFacet.Parameter.query, "*");

            // Get and print results
            SolrResults solrResults = solrQuery.Execute();
            foreach (SolrItemResult i in solrResults.Response.docs)
            {
                Console.WriteLine(i.ToString()); // Print all the results
            }

            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            //SimpleExample();
            FacetExample();
        }
    }
}
