using System;
using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.TypeSystem;
using JayDataApi;
using Saltarelle.Compiler;
using Saltarelle.Compiler.JSModel.Expressions;
using Saltarelle.Compiler.JSModel.Statements;
using Saltarelle.Compiler.JSModel.TypeSystem;
using Saltarelle.Compiler.OOPEmulation;

namespace JayData.Plugin
{
    public class JayDataOOPEmulator : Saltarelle.Compiler.Decorators.OOPEmulatorDecoratorBase
    {
        private readonly Dictionary<string, string> _typeMapping = new Dictionary<string, string>(); 

        public JayDataOOPEmulator(IOOPEmulator prev)
            : base(prev)
        {
            _typeMapping.Add("System.Int32", "int");
            _typeMapping.Add("System.String", "string");
            _typeMapping.Add("System.Collections.Generic.IList", "Array");
        }

        public override TypeOOPEmulation EmulateType(JsType type)
        {
            if (Helpers.IsEntityType(type.CSharpTypeDefinition)) return EmulateJayDataType(type, GenerateJayEntityInitCall);
            if (Helpers.IsEnityContextType(type.CSharpTypeDefinition)) return EmulateJayDataType(type, GenerateJayEntityContextInitCall);
            return base.EmulateType(type);
        }

        private TypeOOPEmulation EmulateJayDataType(JsType type, Func<JsType, JsExpression> initCallGenerator)
        {
            var originalOOPEmulation = base.EmulateType(type);

            var phases = new List<TypeOOPEmulationPhase>(originalOOPEmulation.Phases);

            var jayDataConstructor = JsExpression.Assign(
                JsExpression.Member(new JsTypeReferenceExpression(type.CSharpTypeDefinition),"$jayDataConstructor"),
                initCallGenerator(type));


            var statements = new List<JsStatement>(phases[0].Statements);
            statements.Insert(2, jayDataConstructor);

            phases.Insert(1, new TypeOOPEmulationPhase(GetDependentTypes(type), statements));
            phases.RemoveAt(0);

            return new TypeOOPEmulation(phases);
        }

        IEnumerable<ITypeDefinition> GetDependentTypes(JsType type)
        {
            var dependencies = type.CSharpTypeDefinition.Properties.Where(Helpers.IsEntityContextProperty).Select(property => property.ReturnType.TypeArguments[0].GetDefinition()).ToList();
            dependencies.AddRange(type.CSharpTypeDefinition.GetAllBaseTypeDefinitions().Where(x => !x.Equals(type.CSharpTypeDefinition)));
            return dependencies;
        }

        private JsExpression GenerateJayEntityInitCall(JsType type)
        {
            var clazz = type as JsClass;
            if (clazz == null) return null;

	        var parameters = new List<JsExpression>();
	        var propertyDefinitions = JsExpression.String(clazz.CSharpTypeDefinition.FullName);
	        var propertyInitializerList = new List<JsObjectLiteralProperty>();

	        foreach (var property in clazz.CSharpTypeDefinition.Properties.Where(Helpers.IsEntityProperty))
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


            return JsExpression.Invocation(
                    JsExpression.Member(JsExpression.Member(JsExpression.Identifier("$data"), "Entity"), "extend"), parameters);
        }

        private JsExpression GenerateJayEntityContextInitCall(JsType type)
        {
            var clazz = type as JsClass;
            if (clazz == null) return null;

            var propertyInitializerList = new List<JsObjectLiteralProperty>();

            foreach (var property in clazz.CSharpTypeDefinition.Properties.Where(Helpers.IsEntityContextProperty))
            {
                var initializers = new List<JsObjectLiteralProperty>();
                var typeInitializer = new JsObjectLiteralProperty("type", JsExpression.String("$data.EntitySet"));
                initializers.Add(typeInitializer);
                var elementName = property.ReturnType.TypeArguments[0].FullName;
                initializers.Add(new JsObjectLiteralProperty("elementType", JsExpression.String(elementName)));

                propertyInitializerList.Add(new JsObjectLiteralProperty(property.Name,
                                                                        JsExpression.ObjectLiteral(initializers.ToArray())));
            }

            return JsExpression.Invocation(
                    JsExpression.Member(JsExpression.Member(JsExpression.Identifier("$data"), "EntityContext"), "extend"),JsExpression.String(type.CSharpTypeDefinition.FullName) ,  JsExpression.ObjectLiteral(propertyInitializerList));
        }

    }
}
