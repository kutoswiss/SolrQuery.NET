using SolrQueryNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolrQueryNET_Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize the query
            SolrQuery solrQuery = new SolrQuery("cordra", "doav01.itu.int", 8983);

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
    }
}
