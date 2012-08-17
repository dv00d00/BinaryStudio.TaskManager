namespace BinaryStudio.TaskManager.Extensions.Logic
{
    using System;

    using BinaryStudio.TaskManager.Extensions.Extentions;

    public class StringExtensions : IStringExtensions
    {
        public string Truncate(string source, int count)
        {
            var truncatedString = "";
            if (source != null)
            {
                truncatedString = source.Substring(0, Math.Min(count, source.Length));
                if (count < source.Length) truncatedString += "...";
            }
            return truncatedString;
        }

        public string ToSingleLine(string source)
        {
            return source.Replace(Environment.NewLine, " ");
        }
    }
}