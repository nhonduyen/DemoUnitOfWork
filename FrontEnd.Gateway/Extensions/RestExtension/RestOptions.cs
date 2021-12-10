using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontEnd.Gateway.Extensions.RestExtension
{
    public class RestOptions
    {
        public IEnumerable<Service> ApiResources { get; set; }
        public class Service
        {
            public string Name { get; set; }
            public string Uri { get; set; }
        }
    }
}
