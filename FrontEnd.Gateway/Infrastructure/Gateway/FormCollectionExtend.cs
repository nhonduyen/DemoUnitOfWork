using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Gateway.Infrastructure.Gateway
{
    public static class FormCollectionExtend
    {
        public static IEnumerable<KeyValuePair<string, string>> Transform(this IFormCollection form)
        {
            foreach (var item in form)
            {
                yield return new KeyValuePair<string, string>(item.Key, item.Value.ToString());
            }
        }
    }
}
