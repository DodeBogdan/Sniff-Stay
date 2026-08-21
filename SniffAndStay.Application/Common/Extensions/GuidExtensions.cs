using System;
using System.Collections.Generic;
using System.Text;

namespace SniffAndStay.Application.Common.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(this Guid? guid)
        {
            return !guid.HasValue || guid == Guid.Empty;
        }
    }
}
