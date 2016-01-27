using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diggins.Jigsaw;
using FluentAssertions;
using ImageProcessor.Shaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessor.UnitTests.ShaderCompiler
{
    [TestClass]
    public class StatementTests
    {
        [TestMethod]
        public void Can_create_meta_statement_from_literal()
        {
            var node = ShaderGrammar.MetaStatement.Parse("someIdentifier: \"Hello\"").First();

            node.Nodes.FindByLabel("Identifier").Text.Should().Be("someIdentifier");
            node.Nodes.FindByLabel("Value").Text.Should().Be("\"Hello\"");
            node.Nodes.FindByLabel("Value").Nodes.Should().Contain(n => n.Label == "QuotedString");                
        }

        [TestMethod]
        public void Can_create_meta_statement_from_number()
        {
            var node = ShaderGrammar.MetaStatement.Parse("shader: 1.0").First();

            node.Nodes.FindByLabel("Identifier").Text.Should().Be("shader");
            node.Nodes.FindByLabel("Value").Text.Should().Be("1.0");
            node.Nodes.FindByLabel("Value").Nodes.Should().Contain(n => n.Label == "Number");
        }

        [TestMethod]
        public void Can_create_meta_statement_from_boolean()
        {
            var node = ShaderGrammar.MetaStatement.Parse("optimize: yes").First();

            node.Nodes.FindByLabel("Identifier").Text.Should().Be("optimize");
            node.Nodes.FindByLabel("Value").Text.Should().Be("yes");
            node.Nodes.FindByLabel("Value").Nodes.Should().Contain(n => n.Label == "Boolean");
        }

        [TestMethod]
        public void Can_declare_an_empty_variable()
        {
            var node = ShaderGrammar.VarDecl.Parse("def myVar").First();
            node.Nodes.FindByLabel("Identifier").Text.Should().Be("myVar");
        }

        [TestMethod]
        public void Can_declare_a_variable_with_initial_float_value()
        {
            var node = ShaderGrammar.VarDecl.Parse("def myVar = 1.0").First();

            node.Nodes.FindByLabel("Identifier").Text.Should().Be("myVar");
            node.Nodes[1].Text.Should().Be("1.0");
            node.Nodes[1].Label.Should().Be("Number");
        }
    }
}
