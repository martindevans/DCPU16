using Assembler.Grammar;

namespace Assembler
{
    //test case: http://fingswotidun.com/dcpu16/pac.html

    public class Assembler
    {
        private readonly string _input;
        private readonly Func<string, string> _loadFile;

        public Assembler(string input, Func<string, string> loadFile)
        {
            _input = input;
            _loadFile = loadFile;
        }

        public void Assemble(Stream output)
        {
            var ast = new Parser().Parse(_input);
        }
    }
}