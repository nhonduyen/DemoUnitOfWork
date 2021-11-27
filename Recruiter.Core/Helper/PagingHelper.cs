using Recruiter.Core.Entities.ViewModel.Requests;
using Recruiter.Core.Entities.ViewModel.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Recruiter.Core.Helper
{
    public static class PagingHelper
    {
        public static async Task<List<T>> ToListPagedAsync<T>(this IQueryable<T> query, BaseRequestPayload requestPayload, BaseResultData resultData) where T : class
        {
            if (requestPayload.paging == null)
            {
                resultData.paging = null;
                return await query.ToListAsync();
            }
            else
            {
                resultData.paging = new ResultPaging
                {
                    page = requestPayload.paging.page ?? 0,
                    pageSize = requestPayload.paging.pageSize ?? 1,
                    rowCount = await query.CountAsync()
                };

                var pageCount = (double)resultData.paging.rowCount / (requestPayload.paging.pageSize ?? 1);
                resultData.paging.pageCount = (int)Math.Ceiling(pageCount);

                var skip = ((requestPayload.paging.page ?? 0) - 1) * (requestPayload.paging.pageSize ?? 1);
                return await query.Skip(skip)
                    .Take(requestPayload.paging.pageSize ?? 1)
                    .ToListAsync();
            }
        }
    }
}
