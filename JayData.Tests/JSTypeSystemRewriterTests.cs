using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CoreLib.Plugin;
using ICSharpCode.NRefactory.TypeSystem;
using Knockout.Tests;
using Moq;
using NUnit.Framework;
using Saltarelle.Compiler;
using Saltarelle.Compiler.Compiler;
using Saltarelle.Compiler.JSModel;
using Saltarelle.Compiler.JSModel.TypeSystem;
using MetadataImporter = JayData.Plugin.MetadataImporter;

namespace JayData.Tests {
    [TestFixture]
    public class JSTypeSystemRewriterTests
    {
        private MockErrorReporter _errorReporter;
        private IMetadataImporter _metadata;
        private ReadOnlyCollection<Message> _allErrors;
        private ICompilation _compilation;

        private void AssertEqual(string actual, string expected)
        {
            Assert.That(actual.Replace("\r\n", "\n"), Is.EqualTo(expected.Replace("\r\n", "\n")),
                        "Expected:" + Environment.NewLine + expected + Environment.NewLine + Environment.NewLine +
                        "Actual:" + actual);
        }

        private JsClass Compile(string source, IMetadataImporter prev = null, IRuntimeLibrary runtimeLibrary = null,
                                bool expectErrors = false)
        {
            var pc = PreparedCompilation.CreateCompilation("Test", new[] {new MockSourceFile("File1.cs", source)},
                                                           new[] {Files.Mscorlib, Files.Web, Files.JayData},
                                                           new List<string>());
            _compilation = pc.Compilation;

            _errorReporter = new MockErrorReporter(!expectErrors);
            prev = prev ?? new CoreLib.Plugin.MetadataImporter(_errorReporter, _compilation, new CompilerOptions());
            var namer = new Namer();
            runtimeLibrary = runtimeLibrary ?? new RuntimeLibrary(prev, _errorReporter, _compilation, namer);

            _metadata = new MetadataImporter(prev, _errorReporter, runtimeLibrary, new Mock<INamer>().Object,
                                             new CompilerOptions());

            _metadata.Prepare(_compilation.GetAllTypeDefinitions());

            _allErrors = _errorReporter.AllMessages.ToList().AsReadOnly();
            if (expectErrors)
            {
                Assert.That(_allErrors, Is.Not.Empty, "Compile should have generated errors");
            }
            else
            {
                Assert.That(_allErrors, Is.Empty, "Compile should not generate errors");
            }

            var c = new Compiler(_metadata, namer, runtimeLibrary, _errorReporter);
            var types = ((IJSTypeSystemRewriter) _metadata).Rewrite(c.Compile(pc)).OfType<JsClass>().ToList();
            return types.Single(t => t.CSharpTypeDefinition.Name == "C");
        }

        [Test]
        public void StaticInitStatement()
        {
            var c = Compile(
                @"using JayDataApi;
using System.Collections.Generic;

[Entity]
public class D {
}

[Entity]
public class C {
    [Key]
    [Computed]
    public int Id { get; set; }
    [InverseProperty(""P1"")]
    public IList<D> P2 {get; set;}
    D P3 {get;set;}
}");
            AssertEqual
                (OutputFormatter.Format(c.StaticInitStatements[0]),
                 @"$data.Entity.extend('C', { Id: { type: 'int', key: true, computed: true }, P2: { type: 'Array', elementType: 'D', inverseProperty: 'P1' }, P3: { type: 'D' } });" +"\n");
        }

        [Test, Ignore]
        public void KnockoutPropertyBackingFieldsAreInitializedWhenInvokingBaseConstructor()
        {
            var c = Compile(
                @"using KnockoutApi;
public class B {
	public B(int i) {}
}

public class C : B {
	[KnockoutProperty] public int P1 { get; set; }
	[KnockoutProperty(true)] public string P2 { get; set; }
	public int P3 { get; set; }

	public C() : base(0) {
		int i = 0;
	}
}");

            AssertEqual(OutputFormatter.Format(c.UnnamedConstructor.Body, allowIntermediates: true),
                        @"{
	this.p1 = ko.observable(0);
	this.p2 = ko.observable(null);
	this.$2$P3Field = 0;
	{B}.call(this, 0);
	var i = 0;
}
");
        }
    }
}
