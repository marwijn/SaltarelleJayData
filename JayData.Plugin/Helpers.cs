using ICSharpCode.NRefactory.TypeSystem;
using JayDataApi;
using Saltarelle.Compiler;

namespace JayData.Plugin
{
    public static class Helpers
    {
        public static bool IsEntityType(ITypeDefinition type)
        {
            return
                AttributeReader.HasAttribute<EntityAttribute>(type.Attributes);
        }

        public static bool IsEnityContextType(ITypeDefinition type)
        {
            return AttributeReader.HasAttribute<EntityContextAttribute>(type.Attributes);
        }

        public static bool IsEntityProperty(IProperty property)
        {
            return IsAutoProperty(property);
        }

        public static bool IsEntityContextProperty(IProperty property)
        {
            return IsAutoProperty(property) && property.ReturnType.FullName == "JayDataApi.EntitySet";
        }

        private static bool IsAutoProperty(IProperty property)
        {
            if (property.Region == default(DomRegion))
                return false;
            return property.Getter != null && property.Setter != null && property.Getter.BodyRegion == default(DomRegion) && property.Setter.BodyRegion == default(DomRegion);
        }
    }
}
