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
        private ResponseHeader_s _responseHeader;
        private Response_s _response;

        public ResponseHeader_s ResponseHeader { get { return this._responseHeader; } }
        public Response_s Response { get { return this._response; } }
        public JObject Json { get; private set; }
        #endregion

        #region Constructor
        public SolrResults(String res)
        {
            this.Json = JObject.Parse(res);
            this.ParseFromJson(this.Json);
        }
        #endregion

        #region Private methods
        private void ParseFromJson(JObject json)
        {
            // Parse the response header node
            this._responseHeader.status = Int32.Parse(json["responseHeader"]["status"].ToString());
            this._responseHeader.QTime = Int32.Parse(json["responseHeader"]["QTime"].ToString());

            // Parse the response node
            this._response.numFound = Int32.Parse(json["response"]["numFound"].ToString());
            this._response.start = Int32.Parse(json["response"]["start"].ToString());
            this._response.docs = new List<SolrItemResult>();

            foreach(JObject i in json["response"]["docs"])
                this._response.docs.Add(new SolrItemResult(i));
        }
        #endregion
    }
}
