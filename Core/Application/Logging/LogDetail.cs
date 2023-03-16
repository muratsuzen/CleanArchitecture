using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Logging
{
    public class LogDetail
    {
        public string MethodName { get; set; }
        public string RequestName { get; set; }
        public string ResponseName { get; set; }
        public string? Exceptions { get; set; }
        public List<LogParameter> Parameters { get; set; }
    }
}
