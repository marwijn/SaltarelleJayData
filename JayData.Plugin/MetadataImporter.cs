using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ICSharpCode.NRefactory.TypeSystem;
using JayDataApi;
using Saltarelle.Compiler;
using Saltarelle.Compiler.Compiler;
using Saltarelle.Compiler.Decorators;
using Saltarelle.Compiler.JSModel.Expressions;
using Saltarelle.Compiler.JSModel.TypeSystem;
using Saltarelle.Compiler.ScriptSemantics;

namespace JayData.Plugin
{
    public class MetadataImporter : MetadataImporterDecoratorBase, IRuntimeContext, IJSTypeSystemRewriter
    {
        private readonly IErrorReporter _errorReporter;
        private readonly IRuntimeLibrary _runtimeLibrary;
        private readonly INamer _namer;
        private readonly bool _minimizeNames;

        public MetadataImporter(IMetadataImporter prev, IErrorReporter errorReporter, IRuntimeLibrary runtimeLibrary, INamer namer, CompilerOptions options)
            : base(prev)
        {
            _errorReporter = errorReporter;
            _runtimeLibrary = runtimeLibrary;
            _namer = namer;
            _minimizeNames = options.MinimizeScript;
        }

        public override void Prepare(ITypeDefinition type)
        {
            if (AttributeReader.HasAttribute<EntityAttribute>(type))
            {
                foreach (var p in type.Properties.Where(Helpers.IsEntityProperty))
                {
                    base.ReserveMemberName(p.DeclaringTypeDefinition, p.Name, false);
                    base.SetPropertySemantics(p, PropertyScriptSemantics.Field(p.Name));
                }
            }

            if (AttributeReader.HasAttribute<EntityContextAttribute>(type))
            {

                foreach (var p in type.Properties.Where(Helpers.IsEntityContextProperty))
                {
                    base.ReserveMemberName(p.DeclaringTypeDefinition, p.Name, false);
                    base.SetPropertySemantics(p, PropertyScriptSemantics.Field(p.Name));
                }
            }
            base.Prepare(type);
        }

        public JsExpression ResolveTypeParameter(ITypeParameter tp)
        {
            var typeName = JsExpression.Identifier(_namer.GetTypeParameterName(tp));
            return typeName;
        }

        public JsExpression EnsureCanBeEvaluatedMultipleTimes(JsExpression expression, IList<JsExpression> expressionsThatMustBeEvaluatedBefore)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<JsType> Rewrite(IEnumerable<JsType> types)
        {
            return types;
            // return types.Select(Rewrite);
        }

        //private JsType Rewrite(JsType type)
        //{
        //    var clazz = type as JsClass;
        //    if (clazz == null) return type;

        //    if (AttributeReader.HasAttribute<EntityAttribute>(clazz.CSharpTypeDefinition.Attributes))
        //    {
        //        var newClazz = clazz.Clone();

        //        var statements = new List<JsStatement> (clazz.UnnamedConstructor.Body.Statements);

        //       var constructorMethodName = "$" + type.CSharpTypeDefinition.FullName.Replace('.', '_') + "$JayDataConstructor";

        //        var callJayDataConstructor = JsExpression.Invocation(JsExpression.Member(JsExpression.Identifier(constructorMethodName), "call"), JsExpression.This);

        //        statements.Add(callJayDataConstructor);

        //        newClazz.UnnamedConstructor = JsExpression.FunctionDefinition(
        //            clazz.UnnamedConstructor.ParameterNames, JsStatement.Block(statements), clazz.UnnamedConstructor.Name);
                
        //        return newClazz;
        //    }

        //    if (AttributeReader.HasAttribute<EntityContextAttribute>(clazz.CSharpTypeDefinition.Attributes))
        //    {
        //        var newClazz = clazz.Clone();

        //        var statements = new List<JsStatement>(clazz.UnnamedConstructor.Body.Statements);

        //        var constructorMethodName = "$" + type.CSharpTypeDefinition.FullName.Replace('.', '_') + "$JayDataConstructor";

        //        var callJayDataConstructor = JsExpression.Invocation(JsExpression.Member(JsExpression.Identifier(constructorMethodName), "call"), JsExpression.This, JsExpression.Member(JsExpression.This, "$JayDataConstructorArgument"));

        //        statements.Add(callJayDataConstructor);

        //        newClazz.UnnamedConstructor = JsExpression.FunctionDefinition(
        //            clazz.UnnamedConstructor.ParameterNames, JsStatement.Block(statements), clazz.UnnamedConstructor.Name);

        //        return newClazz;
        //    }

        //    return clazz;
        //}
    }
}

