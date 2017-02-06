using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Gen
{
    public class TableInfo
    {
        public TableInfo()
        {
            this.CodeCustomImplemented = false;
            this.MakeCrud = false;
            this.MakeApp = false;
            this.MakeApi = false;
            this.MakeDomain = false;
            this.MakeTest = false;
        }

        public bool InheritQuery { get; set; }

        public bool ModelBase { get; set; }

        public bool ModelBaseWithoutGets { get; set; }

        private string _InheritClassName;
        public string InheritClassName
        {
            get
            {
                return _InheritClassName.IsSent() ? _InheritClassName : this.ClassName;
            }
            set { _InheritClassName = value; }

        }

        public string BoundedContext { get; set; }

        public string TableName { get; set; }

        public string ClassName { get; set; }

        public string ToolsName { get; set; }

        public bool IsCompositeKey { get { return Keys != null ? Keys.Count() > 1 : false; } }

        public IEnumerable<string> Keys { get; set; }

        public IEnumerable<string> KeysTypes { get; set; }

        public bool MakeCrud { get; set; }

        public bool MakeTest { get; set; }

        public bool MakeApp { get; set; }

        public bool MakeApi { get; set; }

        public bool MakeDomain { get; set; }

        public bool MakeSummary { get; set; }

        public bool MakeDto { get; set; }


        public bool CodeCustomImplemented { get; set; }

        public IEnumerable<Info> ReletedClasss { get; set; }

        #region Obsolet

        public string ClassNameRigth { get; set; }
        public string TableHelper { get; set; }
        public string LeftKey { get; set; }
        public string RightKey { get; set; }


        #endregion

    }
}
