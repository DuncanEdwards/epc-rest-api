using System.Collections.Generic;

namespace Epc.API.Helpers.Paging
{
    abstract public class PagingAndSearchResourceParameters
    {

        #region Protected Fields

        protected int maxPageSize = 20;

        protected int _pageSize = 10;

        #endregion

        #region Public Properties

        virtual public int PageNumber { get; set; } = 1;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        //Includes support 
        public string SearchQuery { get; set; }

        public string OrderBy { get; set; }

        public List<string> ValidOrderByFields { get; set; } = new List<string>();

        #endregion

        #region Public Methods

        public bool IsOrderByValid()
        {
            foreach (var orderByClause in OrderBy.Split(','))
            {
                var orderByAsTuple = OrderByHelper.ParseOrderBy(orderByClause);
                var propertyName = orderByAsTuple.Item1;
                if (!ValidOrderByFields.Contains(propertyName))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion


    }
}
