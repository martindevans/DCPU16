namespace Assembler.Grammar.AST
{
    public class AsmFile
    {
        public IReadOnlyList<Line> Lines { get; }

        public AsmFile(IReadOnlyList<Line> lines)
        {
            Lines = lines;
        }
    }
}
