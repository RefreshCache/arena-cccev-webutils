using System.Data;

namespace Arena.Custom.Cccev.WebUtils.Search
{
    public class SearchMetadata
    {
        private DataTable _properties;
        private DataTable _scopes;

        public DataTable Properties
        {
            get { return _properties; }
            set { _properties = value; }
        }

        public DataTable Scopes
        {
            get { return _scopes; }
            set { _scopes = value; }
        }
    }
}
