using System;

namespace BinaryStudio.TaskManager.Web.Extentions
{
    public static class StringExtensions
    {
        public static string Truncate(this string source, int count)
        {
            if (source == null)
            {
                return "";
            }
            return source.Substring(0, Math.Min(count, source.Length));
        }
    }
}