//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using CoreLib.Plugin;
//using ICSharpCode.NRefactory.TypeSystem;
//using Knockout.Tests;
//using Moq;
//using NUnit.Framework;
//using Saltarelle.Compiler;
//using Saltarelle.Compiler.Compiler;
//using Saltarelle.Compiler.JSModel;
//using Saltarelle.Compiler.JSModel.TypeSystem;
//using MetadataImporter = JayData.Plugin.MetadataImporter;

//namespace JayData.Tests {
//    [TestFixture]
//    public class JSTypeSystemRewriterTests
//    {
//        private MockErrorReporter _errorReporter;
//        private IMetadataImporter _metadata;
//        private ReadOnlyCollection<Message> _allErrors;
//        private ICompilation _compilation;

//        private void AssertEqual(string actual, string expected)
//        {
//            Assert.That(actual.Replace("\r\n", "\n"), Is.EqualTo(expected.Replace("\r\n", "\n")),
//                        "Expected:" + Environment.NewLine + expected + Environment.NewLine + Environment.NewLine +
//                        "Actual:" + actual);
//        }

//        private JsClass Compile(string source, IMetadataImporter prev = null, IRuntimeLibrary runtimeLibrary = null,
//                                bool expectErrors = false)
//        {
//            var pc = PreparedCompilation.CreateCompilation("Test", new[] {new MockSourceFile("File1.cs", source)},
//                                                           new[] {Files.Mscorlib, Files.JayData},
//                                                           new List<string>());
//            _compilation = pc.Compilation;

//            _errorReporter = new MockErrorReporter(!expectErrors);
//            prev = prev ?? new CoreLib.Plugin.MetadataImporter(_errorReporter, _compilation, new CompilerOptions());
//            var namer = new Namer();
//            runtimeLibrary = runtimeLibrary ?? new RuntimeLibrary(prev, _errorReporter, _compilation, namer);

//            _metadata = new MetadataImporter(prev, _errorReporter, runtimeLibrary, new Mock<INamer>().Object,
//                                             new CompilerOptions());

//            _metadata.Prepare(_compilation.GetAllTypeDefinitions());

//            _allErrors = _errorReporter.AllMessages.ToList().AsReadOnly();
//            if (expectErrors)
//            {
//                Assert.That(_allErrors, Is.Not.Empty, "Compile should have generated errors");
//            }
//            else
//            {
//                Assert.That(_allErrors, Is.Empty, "Compile should not generate errors");
//            }

//            var c = new Compiler(_metadata, namer, runtimeLibrary, _errorReporter);
//            var types = ((IJSTypeSystemRewriter) _metadata).Rewrite(c.Compile(pc)).OfType<JsClass>().ToList();
//            return types.Single(t => t.CSharpTypeDefinition.Name == "C");
//        }

//        [Test]
//        public void EnityConstructor()
//        {
//            var c = Compile(
//@"using JayDataApi;
//using System.Collections.Generic;
//
//[Entity]
//public class D {
//}
//
//[Entity]
//public class C {
//    [Key]
//    [Computed]
//    public int Id { get; set; }
//    [InverseProperty(""P1"")]
//    public IList<D> P2 {get; set;}
//    D P3 {get;set;}
//}");
//            AssertEqual
//                (OutputFormatter.Format(c.StaticInitStatements[0]),
//                 @"$data.Entity.extend('C', { Id: { type: 'int', key: true, computed: true }, P2: { type: 'Array', elementType: 'D', inverseProperty: 'P1' }, P3: { type: 'D' } });" + "\n");
//        }

//        [Test]
//        public void EntityContextConstructor()
//        {
//            var c = Compile(
//@"
//using JayDataApi;
//using System.Collections.Generic;
//
//namespace JayDataApi
//{
//    public class EntitySet<T> 
//    {
//    }
//}
//
//[Entity]
//public class D {
//}
//
//[EntityContext]
//public class C {
//    EntitySet<D> P1 {get; set;}
//}");
//            AssertEqual
//                (OutputFormatter.Format(c.UnnamedConstructor),
//                 @"$data.EntityContext.extend({ P1: { type: '$data.EntitySet', elementType: 'D' } })");
//        }
//    }
//}
