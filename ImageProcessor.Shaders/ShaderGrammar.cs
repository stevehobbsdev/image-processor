using System;
using System.Text.RegularExpressions;
using Diggins.Jigsaw;

namespace ImageProcessor.Shaders
{
    class ShaderGrammar : Grammar
    {
        public static Rule WS = SharedGrammar.WS;

        // Primitives
        public static Rule Float = Node(SharedGrammar.Float);
        public static Rule Integer = Node(SharedGrammar.Integer);
        public static Rule Number = Node(Float | Integer);
        public static Rule Vector = Node(SharedGrammar.Parenthesize(Pattern(@"(\d*(\.\d+)?)\s*,\s*(\d*(\.\d+)?)\s*,\s*(\d*(\.\d+)?)\s*(,\s*(\d*(\.\d+)?))?")));
        public static Rule NumberOrVector = Number | Vector;
        public static Rule ImageReference = Node(StringToken("image") + SharedGrammar.Parenthesize(Integer));
        public static Rule QuotedString = Node(MatchChar('"') + AdvanceWhileNot(MatchChar('"')) + MatchChar('"'));
        public static Rule Literal = Number | QuotedString;
        public static Rule Identifier = Node(SharedGrammar.Identifier);
        public static Rule TypeName = Node(SharedGrammar.Identifier);
        public static Rule Symbol = Node(Identifier);

        public static Rule RecStatements = Recursive(() => ZeroOrMore(Statement + WS));
        public static Rule RecFuncStatements = Recursive(() => ZeroOrMore(FuncStatement + WS));
        public static Rule RecExpr = Recursive(() => Expr);

        // Operators
        public static Rule AssignOp = CharToken('=');

        // Methods
        public static Rule Parameter = Node(Identifier);
        public static Rule Arguments = Node(SharedGrammar.Parenthesize(SharedGrammar.CommaDelimited(Parameter)) + WS);
        public static Rule ReturnStatement = Node(StringToken("return") + (Identifier | Vector | Number) + WS);
        public static Rule FuncBody = Node(RecFuncStatements + WS);
        public static Rule Func = Node(MatchChar('.') + Identifier + WS + Arguments + FuncBody + MatchString(".end"));

        // Expressions
        public static Rule LeafExpr = ImageReference | Identifier | NumberOrVector;
        public static Rule AssignmentExpr = Node(Identifier + WS + CharToken('=') + RecExpr);
        public static Rule Expr = (AssignmentExpr | LeafExpr) + WS;

        // Statements
        public static Rule MetaStatement = Node(Identifier + WS + Node(Literal).SetName("Value"));
        public static Rule Comment = CharToken('#') + AdvanceWhileNot(MatchChar('\n'));
        public static Rule VarDecl = Node(TypeName + WS + Identifier + WS + Opt(AssignOp + Expr));

        public static Rule FuncStatement = ReturnStatement | VarDecl | Comment;
        public static Rule Statement = VarDecl | MetaStatement | Comment | Func;
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