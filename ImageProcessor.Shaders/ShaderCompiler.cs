using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessor.Shaders
{
    public class ShaderCompiler
    {
        public void Compile(string path)
        {
            var contents = File.ReadAllText(path);

            try
            {
                var nodes = ShaderGrammar.ShaderProgram.Parse(contents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }               
        }
    }   
}
