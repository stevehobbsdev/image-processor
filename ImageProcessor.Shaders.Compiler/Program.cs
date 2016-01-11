using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.Shaders.Compiler
{
    class Program
    {
        public static void Main()
        {
            var compiler = new ShaderCompiler();
            compiler.Compile(@"examples\dev.shader");
        }
    }
}
