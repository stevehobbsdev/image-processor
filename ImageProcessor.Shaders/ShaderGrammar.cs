using System;
using System.Text.RegularExpressions;
using Diggins.Jigsaw;

namespace ImageProcessor.Shaders
{
    internal class ShaderGrammar : Grammar
    {
        public static Rule WS = SharedGrammar.WS;

        // Primitives
        public static Rule Float = Node(Opt(MatchChar('-')) + SharedGrammar.Float);
        public static Rule Integer = Node(Opt(MatchChar('-')) + SharedGrammar.Integer);
        public static Rule Number = Node(Float | Integer);

        public static Rule Vector = Node(CharToken('|') 
            + Node(Number).SetName("r") + WS 
            + CharToken(',') + Node(Number).SetName("g") + WS 
            + CharToken(',') + Node(Number).SetName("b") + WS 
            + Opt(CharToken(',') + Node(Number).SetName("a") + WS) + CharToken('|'));

        public static Rule NumberOrVector = Number | Vector;
        public static Rule InputReference = Node(StringToken("input") + MatchChar(':') + Integer);
        public static Rule QuotedString = Node(MatchChar('"') + AdvanceWhileNot(MatchChar('"')) + MatchChar('"'));
        public static Rule Literal = Number | QuotedString;
        public static Rule Identifier = Node(SharedGrammar.Identifier);
        public static Rule TypeName = Node(SharedGrammar.Identifier);

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
        public static Rule PrefixOp = Node(SharedGrammar.MatchStringSet("! -"));
        public static Rule BinaryOp = Node(SharedGrammar.MatchStringSet("== < <= >= > + - * /"));
        public static Rule PrefixExpr = Node(PrefixOp + Recursive(() => SimpleExpr));
        public static Rule SimpleExpr = InputReference | Identifier | Vector | Number;        
        public static Rule AssignmentExpr = Node(Identifier + WS + CharToken('=') + RecExpr);
        public static Rule UnaryExpr = PrefixExpr | SimpleExpr;
        public static Rule BinaryExpression = Node(UnaryExpr + WS + BinaryOp + WS + RecExpr);
        public static Rule Expr = (AssignmentExpr | BinaryExpression | UnaryExpr) + WS;

        // Statements
        public static Rule MetaStatement = Node(Identifier + WS + Node(Literal).SetName("Value"));
        public static Rule Comment = CharToken('#') + AdvanceWhileNot(MatchChar('\n'));
        public static Rule VarDecl = Node(StringToken("def") + Identifier + WS + Opt(AssignOp + Expr));

        public static Rule ExprStatement = Node(Expr + AdvanceWhileNot(MatchChar('\n')));
        public static Rule FuncStatement = VarDecl | ReturnStatement | ExprStatement | Comment;
        public static Rule Statement = VarDecl | ExprStatement | MetaStatement  | Comment | Func;
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