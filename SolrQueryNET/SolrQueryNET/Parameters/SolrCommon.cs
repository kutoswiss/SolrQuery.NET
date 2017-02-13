/*
 * Filename:    SolrCommon.cs
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolrQueryNET
{
    public class SolrCommon : ISolrParam
    {
        #region Constants
        public const int DEFAULT_START = 0;
        public const int DEFAULT_ROWS = 10;
        #endregion

        #region Enums
        /// <summary>
        /// Common parameters names
        /// </summary>
        public enum Parameter
        {
            q,
            fq,
            sort,
            start,
            rows,
            fl,
            df,
            wt,
            indent,
            debugQuery
        }

        /// <summary>
        /// Reponse writer types
        /// </summary>
        public enum ResponseWriter
        {
            // Not yet implemented
            //xml,
            //python,
            //ruby,
            //php,
            //csv,
            json
        }
        #endregion

        #region Structures
        public struct SortValue
        {
            public String field;
            public bool desc;
        }
        #endregion

        #region Fields & Properties
        private Dictionary<Parameter, String> values;

        public Dictionary<Parameter, String> Values
        {
            get { return values; }
        }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public SolrCommon()
        {
            this.Reset();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Method to bind a value on a parameters
        /// </summary>
        /// <param name="p"></param>
        /// <param name="value"></param>
        public void BindValue(Parameter p, String value)
        {
            this.values[p] += value;
        }

        /// <summary>
        /// Method to bind a value on a parameters
        /// </summary>
        /// <param name="p">Solr Common parameter</param>
        /// <param name="field">name of the field</param>
        /// <param name="value">Value of the field</param>
        /// <param name="quote">Put double quote the value</param>
        public void BindValue(Parameter p, String field, String value, bool quote = false)
        {
            this.values[p] += String.Format("{0}:{2}{1}{2}", field, value, (quote) ? "\"" : String.Empty);
        }

        /// <summary>
        /// Method to bind multiple values on a parameters
        /// </summary>
        /// <param name="p"></param>
        /// <param name="op"></param>
        /// <param name="values"></param>
        public void BindValue(Parameter p, QueryOperator op, params String[] values)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                if(i > 0) this.BindOperator(p, op);
                this.values[p] += values[i];
            }
        }

        /// <summary>
        /// Method to replace a value on a parameter
        /// </summary>
        /// <param name="p"></param>
        /// <param name="value"></param>
        public void ReplaceValue(Parameter p, String value)
        {
            this.values[p] = value;
        }

        /// <summary>
        /// Method to bind an operator to a parameter
        /// </summary>
        /// <param name="p">Solr Common parameter</param>
        /// <param name="op">Operator value</param>
        public void BindOperator(Parameter p, QueryOperator op)
        {
            String operator_str = String.Empty;
            switch (op)
            {
                case QueryOperator.AND: operator_str = " AND ";
                    break;

                case QueryOperator.OR: operator_str = " OR ";
                    break;

                default: break;
            }

            this.values[p] += operator_str;
        }

        /// <summary>
        /// Method to bind a sign to a parameter
        /// </summary>
        /// <param name="p">Solr Common parameter</param>
        /// <param name="s">Sign value</param>
        public void BindSign(Parameter p, QuerySymbol s)
        {
            this.values[p] += SolrQuery.SignToStr(s);
        }

        /// <summary>
        /// Method to set the sort value
        /// </summary>
        /// <param name="s"></param>
        public void SetSort(SortValue s)
        {
            this.ReplaceValue(Parameter.sort, String.Format("{0} {1}", s.field, (s.desc) ? "desc" : "asc"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void SetDateRange(Parameter p, String field, DateTime from, DateTime to)
        {
            const String format = "yyyy-MM-ddTHH:mm:ssZ";
            this.values[p] += String.Format("{0}:[{1} TO {2}]", field, from.ToString(format), to.ToString(format));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wt"></param>
        public void SetResponseWriter(ResponseWriter wt)
        {
            this.ReplaceValue(Parameter.wt, wt.ToString());
        }

        #endregion

        #region Implemented methods
        public string BuildQuery()
        {
            String url = String.Empty;
            foreach (KeyValuePair<Parameter, string> key in this.values)
            {
                url += (key.Value != String.Empty) ? String.Format("{0}={1}&",
                    ((Parameter)key.Key).ToString(),
                    HttpUtility.UrlEncode(key.Value)) : String.Empty;
            }

            return url;
        }

        public void Reset()
        {
            this.values = new Dictionary<Parameter, string>();
            this.values.Add(Parameter.q, String.Empty);
            this.values.Add(Parameter.fq, String.Empty);
            this.values.Add(Parameter.sort, String.Empty);
            this.values.Add(Parameter.start, DEFAULT_START.ToString());
            this.values.Add(Parameter.rows, DEFAULT_ROWS.ToString());
            this.values.Add(Parameter.fl, String.Empty);
            this.values.Add(Parameter.df, String.Empty);
            this.values.Add(Parameter.wt, (ResponseWriter.json).ToString());
            this.values.Add(Parameter.indent, "false");
            this.values.Add(Parameter.debugQuery, String.Empty);
        }
        #endregion
    }
}
