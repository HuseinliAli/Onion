using Entities.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmloyees.Presentation.Extensions
{
    public static partial class Extension
    { 
        public static TResultType GetResult<TResultType>(this ApiBaseResponse response)
        {
            return ((ApiOkResponse<TResultType>)response).Result;
        }
    }
}
