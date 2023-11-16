using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class ApplicationAssembly
    {
        public static readonly Assembly Assembly = Assembly.GetExecutingAssembly();
    }
}
