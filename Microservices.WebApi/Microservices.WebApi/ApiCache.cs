using System;
using System.Collections.Generic;

namespace Microservices.WebApi
{
    public class ApiCache : Dictionary<string, Type>
    {
        public static readonly ApiCache Instance = new ApiCache();
        private ApiCache() { }
    }
}
