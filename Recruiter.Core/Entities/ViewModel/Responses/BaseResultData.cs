using Newtonsoft.Json;
using Recruiter.Core.Common.Constants;

namespace Recruiter.Core.Entities.ViewModel.Responses
{
    public class BaseResultData
    {
        public virtual ResultPaging paging { get; set; }
        public virtual ResultSorting sorting { get; set; }
    }

    public class ResultPaging
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int rowCount { get; set; }
        public int pageCount { get; set; }

    }

    public class ResultSorting
    {
        public string column { get; set; }

        public string direction { get; set; }
    }

    public class ResultStatus
    {
        public ResultStatus()
        {
            code = StatusCode.SUCCESS;
            msg = "";
            data = null;
            keyParam = null;
        }

        [JsonIgnore]
        public bool IsSuccess => this.code == StatusCode.SUCCESS;

        public string code { get; set; }
        public string msg { get; set; }
        public object data { get; set; }
        public string[] keyParam { get; set; }
    }
}
