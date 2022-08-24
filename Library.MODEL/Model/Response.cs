using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryAPI.MODEL.Model
{
   public class Response
    {
        public string result { get; set; }
        public bool status { get; set; }
         
    }
    public class ResultModel<T>
    {
        public bool respStatus { get; set; }
        public string respMsg { get; set; }
        public T model { get; set; }
        public List<T> lstModel { get; set; }

        public ResultModel()
        {
            respStatus = true;
            respMsg = "Query Executed Successfully.";

        }

    }
}
