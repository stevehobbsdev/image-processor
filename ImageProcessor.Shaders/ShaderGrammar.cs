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
        public static Rule Vector = Node(SharedGrammar.Parenthesize(Pattern(@"(\d*(\.\d+))\s*,\s*(\d*(\.\d+))\s*,\s*(\d*(\.\d+))\s*(,\s*(\d*(\.\d+))\s*)?")));
        public static Rule NumberOrVector = Number | Vector;
        public static Rule ImageReference = Node(StringToken("image") + SharedGrammar.Parenthesize(Integer));
        public static Rule QuotedString = Node(MatchChar('"') + AdvanceWhileNot(MatchChar('"')) + MatchChar('"'));
        public static Rule Literal = Number | QuotedString;
        public static Rule Identifier = Node(SharedGrammar.Identifier);
        public static Rule TypeName = Node(SharedGrammar.Identifier);

        // Expressions
        public static Rule AssignmentExpr = Node(Identifier + WS + CharToken('=') + (NumberOrVector | ImageReference));

        public static Rule MetaStatement = Node(Identifier + WS + Node(Literal).SetName("Value"));
        public static Rule Comment = CharToken('#') + AdvanceWhileNot(MatchChar('\n'));
        public static Rule VarDecl = Node(TypeName + WS + (AssignmentExpr | Identifier));

        public static Rule Statement = VarDecl | MetaStatement | Comment;
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
