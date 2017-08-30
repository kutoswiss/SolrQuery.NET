using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SolrQueryNET.Parameters
{
    public class SolrFacet : ISolrParam
    {
        #region Enum
        public enum Parameter
        {
            query,
            field,
            prefix
        }
        #endregion

        #region Fields & Properties
        public bool isEnabled { get; set; }
        private Dictionary<Parameter, String> values;

        public Dictionary<Parameter, String> Values
        {
            get { return values; }
        }
        #endregion

        #region Constructor
        public SolrFacet()
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
            this.values[p] += String.Format("{0}%3A{2}{1}{2}", field, value, (quote) ? "%22" : String.Empty);
        }
        #endregion

        #region Implemented methods
        public string BuildQuery()
        {
            String url = String.Empty;

            if (this.isEnabled)
            {
                url += "facet=true&";
                foreach (KeyValuePair<Parameter, string> key in this.values)
                {
                    string v = key.Value;
                    url += (v != String.Empty) ? String.Format("facet.{0}={1}&",
                        ((Parameter)key.Key).ToString(),
                        HttpUtility.UrlEncode(v)) : String.Empty;
                }
            }

            return url;
        }

        public void Reset()
        {
            this.values = new Dictionary<Parameter, string>();
            this.values.Add(Parameter.query, String.Empty);
            this.values.Add(Parameter.field, String.Empty);
            this.values.Add(Parameter.prefix, String.Empty);
        }
        #endregion
    }
}
