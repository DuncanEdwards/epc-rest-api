using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epc.API.Helpers.Paging
{
    public class UsersResourceParameters : PagingAndSearchResourceParameters
    {

        public string Type{ get; set; }
       
        public UsersResourceParameters()
        {
            ValidOrderByFields.AddRange(new[] { "Surname", "FirstName", "Email" });
            //Set default
            OrderBy = "Surname";
        }
    }
}
