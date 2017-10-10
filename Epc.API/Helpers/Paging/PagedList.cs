using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epc.API.Helpers.Paging
{
    public class PagedList<T> : List<T>
    {

        #region Public Properties

        public int CurrentPage { get; private set; }

        public int TotalPages { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public bool HasPrevious
        {
            get
            {
                return (CurrentPage > 1);
            }
        }

        public bool HasNext
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }

        #endregion

        #region Constructor

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        #endregion

        #region Public Methods

        public string GetPaginationHeaderJson(string previousPageUri, string nextPageUri)
        {
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage = (HasPrevious) ? previousPageUri : null,
                nextPage = (HasNext) ? nextPageUri : null,
            };

            return JsonConvert.SerializeObject(paginationMetadata);
        }

        #endregion

        #region Factory Method

        public static PagedList<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }

        #endregion

    }
}
