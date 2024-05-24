using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Common.ViewModels
{

    ////<summary>
    ////API Response
    ///</summary>
    public class APIResponse
    {
        ////<summary>
        ////Status Code
        ///</summary>
        ///<example>404</example>
       public int StatusCode {  get; set; }

        ////<summary>
        ////Status Message
        ///</summary>
        ///<example>Not Found</example>
        public string Message{ get; set; }

        ////<summary>
        ////API Response Data
        ///</summary>
       
        public object Data { get; set; }


    }
}
