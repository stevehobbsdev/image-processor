using System.Linq;
using FluentAssertions;
using ImageProcessor.Shaders;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageProcessor.UnitTests.ShaderCompiler
{
    [TestClass]
    public class ExpressionTests
    {
        [TestMethod]
        public void Unary_float_expression()
        {
            ShaderGrammar.UnaryExpr.Parse("1.0");
        }

        [TestMethod]
        public void Unary_identifier_expression()
        {
            ShaderGrammar.UnaryExpr.Parse("myVar");
        }

        [TestMethod]
        public void Unary_vector_expression()
        {
            ShaderGrammar.UnaryExpr.Parse("|1,2,3|");
        }

        [TestMethod]
        public void Unary_boolean_expression()
        {
            var node = ShaderGrammar.UnaryExpr.Parse("true").First();
            node.Text.Should().Be("true");
        }

        [TestMethod]
        public void Unary_input_expression()
        {
            ShaderGrammar.UnaryExpr.Parse("input:0");
        }

        [TestMethod]
        public void Unary_prefix_not_expression()
        {
            ShaderGrammar.PrefixExpr.Parse("!myvar");
        }

        [TestMethod]
        public void Unary_prefix_negative_expression()
        {
            ShaderGrammar.PrefixExpr.Parse("-1.0");
        }

        [TestMethod]
        public void Unary_property_access()
        {
            ShaderGrammar.UnaryExpr.Parse("somevar.x.y");
        }

        [TestMethod]
        public void Assignment_expression_simple()
        {
            ShaderGrammar.AssignmentExpr.Parse("myVar = 1.0");
        }

        [TestMethod]
        public void Assignment_expression_recursive()
        {
            ShaderGrammar.AssignmentExpr.Parse("myVar = myVar2 = 10");
        }

        [TestMethod]
        public void Assignment_expression_index()
        {
            ShaderGrammar.AssignmentExpr.Parse("a = input[a - 1]");
        }

        [TestMethod]
        public void Assignment_expression_to_index()
        {
            ShaderGrammar.AssignmentExpr.Parse("input[i] = 10");
        }

        [TestMethod]
        public void Binary_expression_equals()
        {
            ShaderGrammar.BinaryExpression.Parse("a == b");
        }

        [TestMethod]
        public void Binary_expression_less_than()
        {
            ShaderGrammar.BinaryExpression.Parse("a < b");
        }

        [TestMethod]
        public void Binary_expression_less_than_equals()
        {
            ShaderGrammar.BinaryExpression.Parse("a <= b");
        }

        [TestMethod]
        public void Binary_expression_greater_than()
        {
            ShaderGrammar.BinaryExpression.Parse("a > b");
        }

        [TestMethod]
        public void Binary_expression_greater_than_equals()
        {
            ShaderGrammar.BinaryExpression.Parse("a >= b");
        }

        [TestMethod]
        public void Binary_expression_addition()
        {
            ShaderGrammar.BinaryExpression.Parse("a + b");
        }

        [TestMethod]
        public void Binary_expression_subtraction()
        {
            ShaderGrammar.BinaryExpression.Parse("a - b");
        }

        [TestMethod]
        public void Binary_expression_division()
        {
            ShaderGrammar.BinaryExpression.Parse("a / b");
        }

        [TestMethod]
        public void Binary_expression_multiplication()
        {
            ShaderGrammar.BinaryExpression.Parse("a * b");
        }

        [TestMethod]
        public void Binary_expression_power()
        {
            ShaderGrammar.BinaryExpression.Parse("a ** 2");
        }

        [TestMethod]
        public void Binary_expression_modulus()
        {
            ShaderGrammar.BinaryExpression.Parse("a % 2");
        }

        [TestMethod]
        public void Binary_expression_parenthesized()
        {
            ShaderGrammar.BinaryExpression.Parse("a + (b * (1 / 2))");
        }

        [TestMethod]
        public void Binary_expression_not_equal()
        {
            ShaderGrammar.BinaryExpression.Parse("a != b");
        }

        [TestMethod]
        public void Binary_expression_add_vector_integer()
        {
            ShaderGrammar.BinaryExpression.Parse("|0,0,0| + 2");
        }

        [TestMethod]
        public void Binary_expression_add_vector_vector()
        {
            ShaderGrammar.BinaryExpression.Parse("|0,0,0| + |1,2,3|");
        }

        [TestMethod]
        public void Postfix_expression_index()
        {
            ShaderGrammar.PostfixExpr.Parse("somevar[0]");
        }

        [TestMethod]
        public void Postfix_expression_var()
        {
            ShaderGrammar.PostfixExpr.Parse("somevar[a]");
        }

        [TestMethod]
        public void Postfix_expression_binary()
        {
            ShaderGrammar.PostfixExpr.Parse("somvar[1]");
        }

        [TestMethod]
        public void Postfix_expression_field()
        {
            ShaderGrammar.PostfixExpr.Parse("somevar.x");
        }

        [TestMethod]
        public void Postfix_expression_vector_field()
        {
            ShaderGrammar.PostfixExpr.Parse("|1,2,3|.r");
        }

        [TestMethod]
        public void Postfix_Expression_argument_list()
        {
            ShaderGrammar.PostfixExpr.Parse("someFunc(1, 2, 3)");
            ShaderGrammar.PostfixExpr.Parse("anotherFunc(|0, 0, 0 |, r, g, b, 0.1)");
            ShaderGrammar.PostfixExpr.Parse("printValue(1 + 2)");
        }

        [TestMethod]
        public void Comment_expression()
        {
            ShaderGrammar.Comment.Parse("# This is a comment, where I can type anything I want");
        }
    }
}
