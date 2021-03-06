﻿/*
 * Filename:    SolrQuery.cs
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


using SolrQueryNET.Parameters;
using System;
using System.Collections.Generic;
using System.Net;

namespace SolrQueryNET
{
    public class SolrQuery
    {
        #region Enums
        public enum RequestHandler_e
        {
            select,
            spell
        }

        private enum InputParameter_e
        {
            COMMON,
            FACET,
            SPELLCHECK
        }
        #endregion

        #region Properties & Fields
        // Properties
        public RequestHandler_e RequestHandler { get; set; }
        public Uri Uri { get; private set; }
        private Dictionary<InputParameter_e, ISolrParam> Parameters { get; set; }

        public SolrCommon Common
        {
            get { return (SolrCommon) this.Parameters[InputParameter_e.COMMON]; }
            set { this.Parameters[InputParameter_e.COMMON] = value; }
        }

        public SolrFacet Facet
        {
            get { return (SolrFacet)this.Parameters[InputParameter_e.FACET]; }
            set { this.Parameters[InputParameter_e.FACET] = value; }
        }

        public SolrSpellCheck Spellcheck
        {
            get { return (SolrSpellCheck) this.Parameters[InputParameter_e.SPELLCHECK]; }
            set { this.Parameters[InputParameter_e.SPELLCHECK] = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializer
        /// </summary>
        private SolrQuery()
        {
            this.RequestHandler = RequestHandler_e.select;
            this.Parameters = new Dictionary<InputParameter_e, ISolrParam>();
            this.Parameters.Add(InputParameter_e.COMMON, new SolrCommon());
            this.Parameters.Add(InputParameter_e.FACET, new SolrFacet());
            this.Parameters.Add(InputParameter_e.SPELLCHECK, new SolrSpellCheck());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collectionName">Solr collection name</param>
        /// <param name="url">Url of the Solr Engine</param>
        public SolrQuery(String collectionName, String url) : this()
        {
            this.Uri = new Uri(String.Format("{0}/solr/{1}", url, collectionName));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="collectionName">Solr collection name</param>
        /// <param name="url">Url of the Solr Engine</param>
        /// <param name="port">Port number</param>
        public SolrQuery(String collectionName, String url, UInt16 port) : this()
        {
            this.Uri = new Uri(String.Format("{0}:{1}/solr/{2}", url, port.ToString(), collectionName));
        }
        #endregion

        #region Static methods
        /// <summary>
        /// Method to get the string value of a sign_e (enum)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String SignToStr(QuerySymbol s)
        {
            String res = String.Empty;
            switch (s)
            {
                case QuerySymbol.OPEN_P: res = "(";
                    break;

                case QuerySymbol.CLOSE_P: res = ")";
                    break;

                case QuerySymbol.AMPERSAND: res = "&";
                    break;

                default: break;
            }
            return res;
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Method to build the "final" url
        /// </summary>
        /// <returns></returns>
        private String BuildUrl()
        {
            String url = String.Format("http://{0}/{1}?", this.Uri.AbsoluteUri, this.RequestHandler.ToString());

            foreach (KeyValuePair<InputParameter_e, ISolrParam> p in this.Parameters)
                url += p.Value.BuildQuery();

            return url;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Method to execute the query
        /// </summary>
        /// <returns>SolrResults object</returns>
        public SolrResults Execute()
        {
            SolrResults sr = null;
            String response = String.Empty;

            using (WebClient wc = new WebClient())
            {
                String url = this.BuildUrl();
                wc.Encoding = System.Text.Encoding.UTF8;
                response = wc.DownloadString(url);
                sr = new SolrResults(response);
            }

            return sr;
        }

        /// <summary>
        /// Method to reset all the parameters
        /// </summary>
        public void Reset()
        {
            foreach (KeyValuePair<InputParameter_e, ISolrParam> p in this.Parameters)
                p.Value.Reset();
        }
        #endregion
    }
}
