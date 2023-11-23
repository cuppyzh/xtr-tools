using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace a_no_da.xtools.core.Exceptions
{
    public class XToolsException : Exception
    {
        public XToolsException(string message) : base(message) { }
    }
}
