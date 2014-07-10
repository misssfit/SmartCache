using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartCache
{
    public interface ICategorisedCacheItem : ICacheItem
    {
         string Category { get; set; }
    }
}
