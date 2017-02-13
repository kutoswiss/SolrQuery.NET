/*
 * Filename:    SolrItemResult.cs
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
using System.Collections.Generic;


namespace SolrQueryNET
{
    public class SolrItemResult
    {
        #region Properties
        private Dictionary<string, string> _values;
        public JObject Json { get; set; }
        
        /// <summary>
        /// Self indexer to access dictionnary values
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this [string key]
        {
            get 
            {
                if (this._values.ContainsKey(key))
                    return this._values[key];
                else
                    return string.Empty;
            }
        }
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="json"></param>
        public SolrItemResult(JObject json)
        {
            this._values = new Dictionary<string, string>();
            this.Json = json;

            foreach(JProperty p in this.Json.Children())
            {
                this._values.Add(p.Name, p.Value.ToString());
            }
        }

        /// <summary>
        /// Method to print all the parameters values
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = string.Empty;

            foreach (KeyValuePair<string, string> kv in this._values)
                str += string.Format("{0}: {1}\n", kv.Key, kv.Value);

            return str;
        }
    }
}
