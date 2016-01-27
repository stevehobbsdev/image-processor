using System.Linq;
using Diggins.Jigsaw;
using FluentAssertions;
using ImageProcessor.Shaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessor.UnitTests.ShaderCompiler
{
    [TestClass]
    public class LiteralTests
    {
        [TestMethod]
        public void Can_parse_float()
        {
            ShaderGrammar.Float.Parse("1.0");
        }

        [TestMethod]
        public void Can_parse_integer()
        {
            ShaderGrammar.Integer.Parse("1");
        }

        [TestMethod]
        public void Can_parse_float_as_number()
        {
            var nodes = ShaderGrammar.Number.Parse("20.46");

            nodes.First().Nodes.Find(n => n.Label == "Float")
                .Should().NotBeNull()
                .And.Subject.As<Node>().Text.Should().Be("20.46");
        }

        [TestMethod]
        public void Can_parse_integer_as_number()
        {
            var nodes = ShaderGrammar.Number.Parse("3230");

            nodes.First().Nodes.Find(n => n.Label == "Integer")
                .Should().NotBeNull()
                .And.Subject.As<Node>().Text.Should().Be("3230");
        }

        [TestMethod]
        public void Can_parse_rgb_vector()
        {
            var node = ShaderGrammar.Vector.Parse("|2, 3, 4|").First();

            node.Nodes.FindByLabel("r").Text.Should().Be("2");
            node.Nodes.FindByLabel("g").Text.Should().Be("3");
            node.Nodes.FindByLabel("b").Text.Should().Be("4");
        }

        [TestMethod]
        public void Can_parse_rgba_vector()
        {
            var node = ShaderGrammar.Vector.Parse("|1.0 , 2 , 0.5 ,0|").First();

            node.Nodes.FindByLabel("r").FindLeaf().Text.Should().Be("1.0");
            node.Nodes.FindByLabel("g").FindLeaf().Text.Should().Be("2");
            node.Nodes.FindByLabel("b").FindLeaf().Text.Should().Be("0.5");
            node.Nodes.FindByLabel("a").FindLeaf().Text.Should().Be("0");
        }

        [TestMethod]
        public void Can_create_vector_from_expression()
        {
            ShaderGrammar.Vector.Parse("|a,b,c|");
        }

        [TestMethod]
        public void Can_create_vector_from_arithmetic_expression()
        {
            ShaderGrammar.Vector.Parse("|1 + 1, b, (10 * 3.4)|");
        }

        [TestMethod]
        public void Can_parse_input_reference()
        {
            var node = ShaderGrammar.Expr.Parse("input[0]").First();

            node.Nodes.Find(n => n.Label == "Number", true).Text.Should().Be("0");
        }        

        [TestMethod]
        public void Can_parse_boolean()
        {
            ShaderGrammar.Boolean.Parse("yes");
            ShaderGrammar.Boolean.Parse("no");
            ShaderGrammar.Boolean.Parse("true");
            ShaderGrammar.Boolean.Parse("false");
        }

        [TestMethod]
        public void Can_parse_argument_list()
        {
            ShaderGrammar.Params.Parse("(a, b, c)");
        }

        [TestMethod]
        public void Can_parse_return_expression_number()
        {
            ShaderGrammar.ReturnStatement.Parse("return 1.0");
        }

        [TestMethod]
        public void Can_parse_return_expression_identifier()
        {
            ShaderGrammar.ReturnStatement.Parse("return a");
        }

        [TestMethod]
        public void Can_parse_return_expression_vector()
        {
            ShaderGrammar.ReturnStatement.Parse("return |0,0,0|");
        }

        [TestMethod]
        public void Can_parse_return_expression_arithmetic()
        {
            ShaderGrammar.ReturnStatement.Parse("return (a + 1.5)");
        }

        [TestMethod]
        public void Can_parse_function_statement_no_arguments()
        {
            ShaderGrammar.Func.Parse(
                @".myfunc()
                  .end");
        }

        [TestMethod]
        public void Can_parse_function_statement_with_arguments()
        {
            ShaderGrammar.Func.Parse(
                @".myfunc(color, tu, tv)

                  .end");
        }
    }
}
