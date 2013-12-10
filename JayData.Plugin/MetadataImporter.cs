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


        private bool? IsAutoProperty(IProperty property)
        {
            if (property.Region == default(DomRegion))
                return null;
            return property.Getter != null && property.Setter != null && property.Getter.BodyRegion == default(DomRegion) && property.Setter.BodyRegion == default(DomRegion);
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

                foreach (var p in type.Properties.Where(p => IsAutoProperty(p) == true && p.FullName == "JayData.EntitySet"))
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
            var clazz = types.First() as JsClass;
        }
    }
}

