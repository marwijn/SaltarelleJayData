// BindingContext.cs
// Script#/Libraries/Knockout
// This source code is subject to terms and conditions of the Apache License, Version 2.0.
//

using System;
using System.Runtime.CompilerServices;

namespace JayDataApi {
	#if !PLUGIN
	[NonScriptable]
	#endif
	[AttributeUsage(AttributeTargets.Class)]
	public class EntityAttribute : Attribute {
	}

#if !PLUGIN
	[NonScriptable]
#endif
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityContextAttribute : Attribute
    {
    }


	#if !PLUGIN
	[NonScriptable]
	#endif
	[AttributeUsage(AttributeTargets.Property)]
	public class InversePropertyAttribute : Attribute {
		public string InverseProperty { get; private set; }

        public InversePropertyAttribute(string inverseProperty)
        {
            InverseProperty = inverseProperty;
        }
	}

    #if !PLUGIN
	[NonScriptable]
    #endif
    [AttributeUsage(AttributeTargets.Property)]
    public class KeyAttribute : Attribute
    {
    }

    #if !PLUGIN
	[NonScriptable]
    #endif
    [AttributeUsage(AttributeTargets.Property)]
    public class ComputedAttribute : Attribute
    {
    }
}
