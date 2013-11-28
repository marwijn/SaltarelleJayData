using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using JayDataApi;
using Saltarelle.Compiler;
using Saltarelle.Compiler.Compiler;
using Saltarelle.Compiler.Decorators;
using Saltarelle.Compiler.JSModel.Expressions;
using Saltarelle.Compiler.JSModel.TypeSystem;
using Saltarelle.Compiler.ScriptSemantics;

namespace JayData.Plugin {
	public class MetadataImporter : MetadataImporterDecoratorBase, IJSTypeSystemRewriter, IRuntimeContext
	{

	    private readonly Dictionary<string, string> _typeMapping = new Dictionary<string, string>(); 

		private readonly IErrorReporter _errorReporter;
		private readonly IRuntimeLibrary _runtimeLibrary;
		private readonly INamer _namer;
		private readonly bool _minimizeNames;

		public MetadataImporter(IMetadataImporter prev, IErrorReporter errorReporter, IRuntimeLibrary runtimeLibrary, INamer namer, CompilerOptions options) : base(prev) {
			_errorReporter  = errorReporter;
			_runtimeLibrary = runtimeLibrary;
			_namer          = namer;
			_minimizeNames  = options.MinimizeScript;
            _typeMapping.Add("System.Int32", "int");
            _typeMapping.Add("System.String", "string");
            _typeMapping.Add("System.Collections.Generic.IList", "Array");
		}

        //private void PrepareKnockoutProperty(IProperty p) {
        //    if (p.IsStatic) {
        //        if (AttributeReader.HasAttribute<KnockoutPropertyAttribute>(p)) {
        //            _errorReporter.Region = p.Region;
        //            _errorReporter.Message(MessageSeverity.Error, 8000, "The property {0} cannot have a [KnockoutPropertyAttribute] because it is static", p.FullName);
        //        }
        //    }

        private bool? IsAutoProperty(IProperty property) {
            if (property.Region == default(DomRegion))
                return null;
            return property.Getter != null && property.Setter != null && property.Getter.BodyRegion == default(DomRegion) && property.Setter.BodyRegion == default(DomRegion);
        }
	    
        private JsType Rewrite (JsType type)
        {
            var clazz = type as JsClass;
            if (clazz == null) return type;
            var attributes = clazz.CSharpTypeDefinition.GetAttributes().ToList();

            if (AttributeReader.HasAttribute<EntityAttribute>(attributes)) return RewriteEntityClass(clazz);
            if (AttributeReader.HasAttribute<EntityContextAttribute>(attributes)) return RewriteEntityContextClass(clazz);

            return type;
        }

        private JsType RewriteEntityContextClass(JsClass clazz)
        {
            clazz = clazz.Clone();

            var propertyInitializerList = new List<JsObjectLiteralProperty>();

            foreach (var property in clazz.CSharpTypeDefinition.Properties)
            {
                var initializers = new List<JsObjectLiteralProperty>();
                var typeInitializer = new JsObjectLiteralProperty("type", JsExpression.String("$data.EntitySet"));
                initializers.Add(typeInitializer);
                var elementName = property.ReturnType.TypeArguments[0].FullName;
                initializers.Add(new JsObjectLiteralProperty("elementType", JsExpression.String(elementName)));

                propertyInitializerList.Add(new JsObjectLiteralProperty(property.Name,
                                                                        JsExpression.ObjectLiteral(initializers.ToArray())));
            }

            clazz.UnnamedConstructor = JsExpression.Invocation(
                    JsExpression.Member(JsExpression.Member(JsExpression.Identifier("$data"), "EntityContext"), "extend"), JsExpression.ObjectLiteral(propertyInitializerList));

            return clazz;
        }

	    private JsType RewriteEntityClass(JsClass clazz)
	    {
	        clazz = clazz.Clone();

	        var parameters = new List<JsExpression>();
	        var propertyDefinitions = JsExpression.String(clazz.CSharpTypeDefinition.FullName);
	        var propertyInitializerList = new List<JsObjectLiteralProperty>();

	        foreach (var property in clazz.CSharpTypeDefinition.Properties)
	        {
	            var mappedType = _typeMapping.ContainsKey(property.ReturnType.FullName)
	                                 ? _typeMapping[property.ReturnType.FullName]
	                                 : property.ReturnType.FullName;
	            var initializers = new List<JsObjectLiteralProperty>();
	            var typeInitializer = new JsObjectLiteralProperty("type", JsExpression.String(mappedType));
	            initializers.Add(typeInitializer);
	            if (mappedType == "Array")
	            {
	                var elementName = property.ReturnType.TypeArguments[0].FullName;
	                initializers.Add(new JsObjectLiteralProperty("elementType", JsExpression.String(elementName)));
	            }
	            if (AttributeReader.HasAttribute<InversePropertyAttribute>(property.Attributes))
	            {
	                var inverse = AttributeReader.ReadAttribute<InversePropertyAttribute>(property.Attributes).InverseProperty;
	                initializers.Add(new JsObjectLiteralProperty("inverseProperty", JsExpression.String(inverse)));
	            }
	            if (AttributeReader.HasAttribute<KeyAttribute>(property.Attributes))
	            {
	                initializers.Add(new JsObjectLiteralProperty("key", JsExpression.Boolean(true)));
	            }
	            if (AttributeReader.HasAttribute<ComputedAttribute>(property.Attributes))
	            {
	                initializers.Add(new JsObjectLiteralProperty("computed", JsExpression.Boolean(true)));
	            }
	            propertyInitializerList.Add(new JsObjectLiteralProperty(property.Name,
	                                                                    JsExpression.ObjectLiteral(initializers.ToArray())));
	        }

	        var param2 = JsExpression.ObjectLiteral(propertyInitializerList);

	        parameters.Add(propertyDefinitions);
	        parameters.Add(param2);

	        var constructorBody =
	            JsExpression.Invocation(
	                JsExpression.Member(JsExpression.Member(JsExpression.Identifier("$data"), "Entity"), "extend"), parameters);

	        clazz.UnnamedConstructor = constructorBody;

	        return clazz;
	    }

	    public override void Prepare(ITypeDefinition type)
        {
            if (AttributeReader.HasAttribute<EntityAttribute>(type))
            {

                foreach (var p in type.Properties.Where(p => IsAutoProperty(p) == true))
                {
                    base.ReserveMemberName(p.DeclaringTypeDefinition, p.Name, false);
                    base.SetPropertySemantics(p, PropertyScriptSemantics.Field(p.Name));
                }
            }
            
            if (AttributeReader.HasAttribute<EntityAttribute>(type))
            {

                foreach (var p in type.Properties.Where(p => IsAutoProperty(p) == true && p.FullName=="JayData.EntitySet"))
                {
                    base.ReserveMemberName(p.DeclaringTypeDefinition, p.Name, false);
                    base.SetPropertySemantics(p, PropertyScriptSemantics.Field(p.Name));
                }
            }
            base.Prepare(type);
        }

	    public IEnumerable<JsType> Rewrite(IEnumerable<JsType> types)
        {
            return types.Select(Rewrite);
        }

	    public JsExpression ResolveTypeParameter(ITypeParameter tp)
	    {
            var typeName =  JsExpression.Identifier(_namer.GetTypeParameterName(tp));
	        return typeName;
	    }

	    public JsExpression EnsureCanBeEvaluatedMultipleTimes(JsExpression expression, IList<JsExpression> expressionsThatMustBeEvaluatedBefore)
	    {
	        throw new NotImplementedException();
	    }
	}
}
