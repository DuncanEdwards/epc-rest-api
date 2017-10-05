using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Helpers
{
    static public class OrderByHelper
    {

        static public Tuple<string,bool> ParseOrderBy(string orderByClause)
        {
            var trimmedOrderByClause = orderByClause.Trim();
            var orderDescending = trimmedOrderByClause.EndsWith(" desc");

            var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
            var propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

            return new Tuple<string, bool>(propertyName, orderDescending);
        }

    }
}
