using System;
using System.Linq;
using System.Text.RegularExpressions;
using Diggins.Jigsaw;

namespace ImageProcessor.Shaders
{
    internal class ShaderGrammar : Grammar
    {
        public static Rule WS = SharedGrammar.WS;

        // Primitives
        public static Rule RecStatements = Recursive(() => ZeroOrMore(Statement + WS));
        public static Rule RecExpr = Recursive(() => Expr);

        public static Rule Float = Node(SharedGrammar.Float);
        public static Rule Integer = Node(SharedGrammar.Integer);
        public static Rule Number = Node(Float | Integer);
        public static Rule Boolean = Node(SharedGrammar.MatchStringSet("yes no true false"));

        public static Rule Vector = Node(CharToken('|') 
            + Node(RecExpr).SetName("r") + WS 
            + CharToken(',') + Node(RecExpr).SetName("g") + WS 
            + CharToken(',') + Node(RecExpr).SetName("b") + WS 
            + Opt(CharToken(',') + Node(RecExpr).SetName("a") + WS) + CharToken('|'));

        public static Rule NumberOrVector = Number | Vector;
        public static Rule QuotedString = Node(MatchChar('"') + AdvanceWhileNot(MatchChar('"')) + MatchChar('"'));
        public static Rule Literal = Boolean | Number | QuotedString;
        public static Rule Identifier = Node(SharedGrammar.Identifier);
        public static Rule TypeName = Node(SharedGrammar.Identifier);

        // Operators
        public static Rule AssignOp = CharToken('=');

        // Methods
        public static Rule Params = Node(SharedGrammar.Parenthesize(SharedGrammar.CommaDelimited(Identifier)) + WS);
        public static Rule Arguments = Node(SharedGrammar.Parenthesize(SharedGrammar.CommaDelimited(RecExpr)) + WS);

        public static Rule ReturnStatement = Node(StringToken("return") + RecExpr + WS);
        public static Rule FuncBody = Node(RecStatements + WS);
        public static Rule Func = Node(MatchChar('.') + Identifier + WS + Params + FuncBody + MatchString(".end"));

        // Expressions
        public static Rule Index = Node(CharToken('[') + RecExpr + CharToken(']'));
        public static Rule Field = Node(MatchChar('.') + Identifier);
        public static Rule ArgList = Node(SharedGrammar.Parenthesize(SharedGrammar.CommaDelimited(RecExpr)));
        public static Rule PrefixOp = Node(SharedGrammar.MatchStringSet("! -"));
        public static Rule BinaryOp = Node(SharedGrammar.MatchStringSet("== <= < >= > != + - ** * / %"));
        public static Rule ParenExpr = Node(SharedGrammar.Parenthesize(RecExpr));
        public static Rule PrefixExpr = Node(PrefixOp + Recursive(() => PrefixOrLeafExpr));
        public static Rule LeafExpr = ParenExpr | Boolean | Identifier | Vector | Number;
        public static Rule PrefixOrLeafExpr = PrefixExpr | LeafExpr;
        public static Rule PostfixOp = Node(Index | Field | ArgList);
        public static Rule PostfixExpr = Node(PrefixOrLeafExpr + OneOrMore(PostfixOp + WS));
        public static Rule AssignmentExpr = Node((PostfixExpr | Identifier) + WS + CharToken('=') + RecExpr);
        public static Rule UnaryExpr = PostfixExpr | PrefixOrLeafExpr;
        public static Rule BinaryExpression = Node(UnaryExpr + WS + BinaryOp + WS + RecExpr);
        public static Rule Expr = (AssignmentExpr | BinaryExpression | UnaryExpr) + WS;

        // Statements
        public static Rule MetaStatement = Node(Identifier + WS + CharToken(':') + Node(Literal).SetName("Value"));
        public static Rule Comment = CharToken('#') + AdvanceWhileNot(MatchChar('\n'));
        public static Rule VarDecl = Node(StringToken("let") + Identifier + WS + Opt(AssignOp + Expr));

        public static Rule ExprStatement = Node(Expr + AdvanceWhileNot(MatchChar('\n')));
        public static Rule Statement = VarDecl | MetaStatement | ExprStatement | ReturnStatement | Comment | Func;
        public static Rule ShaderProgram = Node(ZeroOrMore(Statement + WS) + End);

        public static Rule CharToken(char c)
        {
            return MatchChar(c) + WS;
        }

        public static Rule StringToken(string s)
        {
            return MatchString(s) + WS;
        }

        static ShaderGrammar()
        {
            InitGrammar(typeof(ShaderGrammar));
        }
    }
}