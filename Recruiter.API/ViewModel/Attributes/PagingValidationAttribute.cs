using Recruiter.API.ViewModel.Requests;
using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Recruiter.API.ViewModel.Attributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class PagingValidationAttribute : ValidationAttribute
    {
        public PagingValidationAttribute() : base()
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format(CultureInfo.CurrentUICulture, ErrorMessageString, name);
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;
            if (!(value is BaseRequestPaging paging)) return false;
            if (!paging.page.HasValue || paging.page.Value < 0)
            {
                return false;
            }

            return paging.pageSize.HasValue && paging.pageSize.Value >= 0;
        }
    }
}
