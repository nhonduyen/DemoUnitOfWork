using Recruiter.Core.Entities.ViewModel.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Recruiter.Core.Entities.ViewModel.Requests
{
    public class BaseRequestPayload
    {
        [PagingValidation]
        public BaseRequestPaging paging { get; set; }
        public BaseRequestSorting sorting { get; set; }
    }

    public class BaseRequestPaging
    {
        public int? page { get; set; }
        public int? pageSize { get; set; }
    }

    public class BaseRequestSorting
    {
        public string column { get; set; }

        [Required]
        public string direction { get; set; }
    }
}
