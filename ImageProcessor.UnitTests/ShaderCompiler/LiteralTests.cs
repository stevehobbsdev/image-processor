using System;
using ImageProcessor.Shaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using System.Linq;
using Diggins.Jigsaw;

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
        public void Can_parse_negative_float()
        {
            ShaderGrammar.Float.Parse("-1.3");
        }

        [TestMethod]
        public void Can_parse_integer()
        {
            ShaderGrammar.Integer.Parse("1");
        }

        [TestMethod]
        public void Can_parse_negative_integer()
        {
            ShaderGrammar.Integer.Parse("-1");
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
            var node = ShaderGrammar.Vector.Parse("|2,3,4|").First();

            node.Nodes.FindByLabel("r").Text.Should().Be("2");
            node.Nodes.FindByLabel("g").Text.Should().Be("3");
            node.Nodes.FindByLabel("b").Text.Should().Be("4");
        }

        [TestMethod]
        public void Can_parse_rgba_vector()
        {
            var node = ShaderGrammar.Vector.Parse("|1.0 , 2 , 0.5 ,0|").First();

            node.Nodes.FindByLabel("r").Text.Should().Be("1.0");
            node.Nodes.FindByLabel("g").Text.Should().Be("2");
            node.Nodes.FindByLabel("b").Text.Should().Be("0.5");
            node.Nodes.FindByLabel("a").Text.Should().Be("0");
        }

        [TestMethod]
        public void Can_parse_input_reference()
        {
            var node = ShaderGrammar.InputReference.Parse("input:0").First();

            node.Nodes.FindByLabel("Integer").Text.Should().Be("0");
        }
    }
}
