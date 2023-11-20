using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.core.Extensions
{
    public static class LoggerExtension
    {
        public static void LogException(this ILogger logger, Exception ex)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Error occurred: {ex.Message}");

            if (ex.InnerException != null)
            {
                stringBuilder.Append($"Inner Exception: {ex.InnerException.Message}");
            }

            stringBuilder.Append($"{ex.StackTrace}");

            logger.LogError($"Error occurred: {stringBuilder.ToString()}");
        }
    }
}
