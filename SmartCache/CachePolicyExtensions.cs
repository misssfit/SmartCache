using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCache
{
    public static class CachePolicyExtensions
    {
        public static int Minutes(this CacheLevel level)
        {
            switch (level)
            {
                case CacheLevel.L1:
                    return 1;
                case CacheLevel.L2:
                    return 10;
                case CacheLevel.L3:
                    return 60;
                case CacheLevel.L4:
                    return 1000;
                default:
                    return 1;
            }
        }
    }
}
