namespace Arena.Custom.Cccev.WebUtils.Search
{
    public enum IncludeInResults { Yes, No };
    public enum SortDirection { Ascending, Descending };

    public class QueryProperty
    {
        private string _propertyName;
        private IncludeInResults _includeInResults;
        private SortDirection? _direction;
        private int? _sortorder;

        /// <summary>
        /// The name of the property
        /// </summary>
        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        /// <summary>
        /// Should the property be returned in the results
        /// </summary>
        public IncludeInResults IncludeInResults
        {
            get { return _includeInResults; }
            set { _includeInResults = value; }
        }

        /// <summary>
        /// The sort direction
        /// </summary>
        public SortDirection? Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        /// <summary>
        /// The sort order priority
        /// </summary>
        public int? SortOrder
        {
            get { return _sortorder; }
            set { _sortorder = value; }
        }
    }
}
