using System;
using System.Text.RegularExpressions;
using Diggins.Jigsaw;

namespace ImageProcessor.Shaders
{
    class ShaderGrammar : SharedGrammar
    {
        public static Rule LineTerminator = MatchChar('\n');
        public static Rule RecLine = Recursive(() => Line);

        // Primitives
        public static Rule QuotedString = MatchChar('"') + ZeroOrMore(ExceptCharSet("\"")) + MatchChar('"');
        public static Rule Bool = MatchRegex(new Regex("[Tt]true|[Ff]alse"));
        public static Rule Literal = Float | Integer | Bool;
        public static Rule ArgList = Node(Parenthesize(CommaDelimited(Node(Identifier).SetName("Argument") + WS)));

        public static Rule Version = Node(MatchRegex(new Regex(@"\d+\.\d+")));
        public static Rule Comment = MatchChar('#') + AdvanceWhileNot(MatchChar('\n'));
        public static Rule TypeName = Node(Identifier);
        public static Rule Symbol = Node(Identifier);
        public static Rule BlockEnd = Node(MatchString("end") + WS + Opt(LineTerminator));

        // Declarations
        public static Rule ShaderDecl = Node(MatchString("shader") + WS + Version);        
        public static Rule VarDecl = Node(TypeName + WS + Symbol + WS + Opt(Eq + WS + Node(Literal).SetName("InitialValue")));
        public static Rule MethodDecl = Node(Identifier.SetName("MethodName") + WS + ArgList);

        // Statements
        public static Rule Declaration = ShaderDecl | VarDecl;
        public static Rule MethodBlock = Node(MethodDecl + WS + LineTerminator + ZeroOrMore(RecLine) + WS + BlockEnd);
        public static Rule Statement = Declaration | Comment;

        public static Rule Line = Statement + WS + Opt(LineTerminator);

        public static Rule ShaderProgram = Node(ZeroOrMore(Line) + WS + End);

        static ShaderGrammar()
        {
            InitGrammar(typeof(ShaderGrammar));
        }
    }
}
