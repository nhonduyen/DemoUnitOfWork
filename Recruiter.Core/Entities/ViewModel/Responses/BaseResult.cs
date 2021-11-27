using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiter.Core.Entities.ViewModel.Responses
{
    public class BaseResult
    {
        public BaseResult()
        {
            status = new ResultStatus();
        }
        public virtual ResultStatus status { get; set; }
    }

    public class BaseResponse<T> where T : new()
    {
        public BaseResponse()
        {
            Status = new ResultStatus();
            Data = new T();
        }

        public virtual ResultStatus Status { get; set; }
        public T Data { get; set; }
    }

    public class Data
    {
        public Data()
        {
            paging = new Paging();
        }

        public virtual Paging paging { get; set; }

    }

    public class Paging
    {
        public int? total { get; set; }
    }
}
