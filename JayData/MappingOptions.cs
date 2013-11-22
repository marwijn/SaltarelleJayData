using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace JayDataApi
{
    [Imported, Serializable]
    public class MappingOptions
    {
        public List<string> Include, Ignore, Copy, Observe;
    }
}