using System;
using System.Text.RegularExpressions;
using Diggins.Jigsaw;

namespace ImageProcessor.Shaders
{
    class ShaderGrammar : SharedGrammar
    {
        public static Rule LineTerminator = MatchChar('\n');

        public static Rule Version = Node(MatchRegex(new Regex(@"\d+\.\d+")));

        public static Rule ShaderDecl = Node(MatchString("shader") + WS + Version);
        public static Rule Comment = MatchChar('#') + AdvanceWhileNot(MatchChar('\n'));
        public static Rule Statement = ShaderDecl | Comment;

        public static Rule Line = Statement + WS + Opt(LineTerminator);
        public static Rule ShaderProgram = Node(ZeroOrMore(Line) + WS + End);

        static ShaderGrammar()
        {
            InitGrammar(typeof(ShaderGrammar));
        }
    }
}
