using cuppyzh.xtrtools.poadocumentgenerator.Exceptions;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace cuppyzh.xtrtools.poadocumentgenerator.Utilities
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
