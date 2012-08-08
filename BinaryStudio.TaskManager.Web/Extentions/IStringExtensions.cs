using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinaryStudio.TaskManager.Web.Extentions
{
    public interface IStringExtensions
    {
        string Truncate(string source, int count);
    }
}