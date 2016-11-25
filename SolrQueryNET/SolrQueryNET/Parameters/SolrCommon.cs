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
        public enum param_e
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
        #endregion

        #region Structures
        public struct sort_param_st
        {
            public String field;
            public bool desc;
        }
        #endregion

        #region Fields & Properties
        private Dictionary<param_e, String> values;

        public Dictionary<param_e, String> Values
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
        public void BindValue(param_e p, String value)
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
        public void BindValue(param_e p, String field, String value, bool quote = false)
        {
            this.values[p] += String.Format("{0}:{2}{1}{2}", field, value, (quote) ? "\"" : String.Empty);
        }

        /// <summary>
        /// Method to bind multiple values on a parameters
        /// </summary>
        /// <param name="p"></param>
        /// <param name="op"></param>
        /// <param name="values"></param>
        public void BindValue(param_e p, operator_e op, params String[] values)
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
        public void ReplaceValue(param_e p, String value)
        {
            this.values[p] = value;
        }

        /// <summary>
        /// Method to bind an operator to a parameter
        /// </summary>
        /// <param name="p">Solr Common parameter</param>
        /// <param name="op">Operator value</param>
        public void BindOperator(param_e p, operator_e op)
        {
            String operator_str = String.Empty;
            switch (op)
            {
                case operator_e.AND: operator_str = " AND ";
                    break;

                case operator_e.OR: operator_str = " OR ";
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
        public void BindSign(param_e p, sign_e s)
        {
            this.values[p] += SolrQuery.SignToStr(s);
        }

        /// <summary>
        /// Method to set the sort value
        /// </summary>
        /// <param name="s"></param>
        public void SetSort(sort_param_st s)
        {
            this.ReplaceValue(param_e.sort, String.Format("{0} {1}", s.field, (s.desc) ? "desc" : "asc"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void SetDateRange(param_e p, String field, DateTime from, DateTime to)
        {
            const String format = "yyyy-MM-ddTHH:mm:ssZ";
            this.values[p] += String.Format("{0}:[{1} TO {2}]", field, from.ToString(format), to.ToString(format));
        }
        #endregion

        #region Implemented methods
        public string BuildQuery()
        {
            String url = String.Empty;
            foreach (KeyValuePair<param_e, string> key in this.values)
            {
                url += (key.Value != String.Empty) ? String.Format("{0}={1}&",
                    ((param_e)key.Key).ToString(),
                    HttpUtility.UrlEncode(key.Value)) : String.Empty;
            }

            return url;
        }

        public void Reset()
        {
            this.values = new Dictionary<param_e, string>();
            this.values.Add(param_e.q, String.Empty);
            this.values.Add(param_e.fq, String.Empty);
            this.values.Add(param_e.sort, String.Empty);
            this.values.Add(param_e.start, DEFAULT_START.ToString());
            this.values.Add(param_e.rows, DEFAULT_ROWS.ToString());
            this.values.Add(param_e.fl, String.Empty);
            this.values.Add(param_e.df, String.Empty);
            this.values.Add(param_e.wt, "json");
            this.values.Add(param_e.indent, "false");
            this.values.Add(param_e.debugQuery, String.Empty);
        }
        #endregion
    }
}
