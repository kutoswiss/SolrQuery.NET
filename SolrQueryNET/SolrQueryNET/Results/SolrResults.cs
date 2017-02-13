/*
 * Filename:    SolrResults.cs
 * Author:      C. Rejas (kutoswiss)
 * Created on:  2016-11-23
 *
 * This file is part of SolrQueryNET.
 *
 * SolrQueryNET is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * SolrQuery is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with SolrQuery. If not, see <http://www.gnu.org/licenses/>.
 */

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SolrQueryNET
{
    public class SolrResults
    {
        #region Constants
        /// <summary>
        /// Static fields name from Solr
        /// </summary>
        private const string FIELD_RESPONSE_HEADER = "responseHeader";
        private const string FIELD_STATUS = "status";
        private const string FIELD_Q_TIME = "QTime";
        private const string FIELD_RESPONSE = "response";
        private const string FIELD_START = "start";
        private const string FIELD_DOCS = "docs";
        private const string FIELD_NUM_FOUND = "numFound";
        #endregion

        #region Structures
        public struct ResponseHeader_s
        {
            public int status;
            public int QTime;
        }

        public struct Response_s
        {
            public int numFound;
            public int start;
            public List<SolrItemResult> docs;
        }
        #endregion

        #region Properties
        // Privates
        private ResponseHeader_s _responseHeader;
        private Response_s _response;

        // Publics
        public ResponseHeader_s ResponseHeader { get { return this._responseHeader; } }
        public Response_s Response { get { return this._response; } }
        public JObject Json { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="res"></param>
        public SolrResults(String res)
        {
            this.Json = JObject.Parse(res);
            this.ParseFromJson(this.Json);
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Method to parse results from Json
        /// </summary>
        /// <param name="json"></param>
        private void ParseFromJson(JObject json)
        {
            // Parse the response header node
            this._responseHeader.status = Int32.Parse(json[FIELD_RESPONSE_HEADER][FIELD_STATUS].ToString());
            this._responseHeader.QTime = Int32.Parse(json[FIELD_RESPONSE_HEADER][FIELD_Q_TIME].ToString());

            // Parse the response node
            this._response.numFound = Int32.Parse(json[FIELD_RESPONSE][FIELD_NUM_FOUND].ToString());
            this._response.start = Int32.Parse(json[FIELD_RESPONSE][FIELD_START].ToString());
            this._response.docs = new List<SolrItemResult>();

            foreach (JObject i in json[FIELD_RESPONSE][FIELD_DOCS])
                this._response.docs.Add(new SolrItemResult(i));
        }
        #endregion
    }
}
