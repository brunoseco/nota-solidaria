using System;
using System.Collections.Generic;
using System.Text;

namespace Util.ListGeneric
{
    public struct Sort
    {
        #region .: Properties :.
        private string strName;
        public string Name
        {
            get { return strName; }
            set
            {
                if (value == null || value.Trim().Length == 0)
                    throw new ArgumentException("A property cannot have an empty name.");

                strName = value.Trim();
            }
        }

        private bool blnDescending;
        public bool Descending
        {
            get { return blnDescending; }
            set { blnDescending = value; }
        }
        #endregion

        #region .: Constructors :.
        public Sort(string propertyName)
        {
            if (propertyName == null || propertyName.Trim().Length == 0)
                throw new ArgumentException("A property cannot have an empty name.");

            this.strName = propertyName.Trim();
            this.blnDescending = false;
        }

        public Sort(string propertyName, bool sortDescending)
        {
            if (propertyName == null || propertyName.Trim().Length == 0)
                throw new ArgumentException("A property cannot have an empty name.");

            this.strName = propertyName.Trim();
            this.blnDescending = sortDescending;
        }
        #endregion
    }
}
