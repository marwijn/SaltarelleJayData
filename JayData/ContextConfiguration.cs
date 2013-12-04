using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace JayDataApi
{
    public class ContextConfiguration
    {
        [IntrinsicProperty]
        public string DatabaseName { get; set; }
        public string Provider { get; set; }
    }
}
