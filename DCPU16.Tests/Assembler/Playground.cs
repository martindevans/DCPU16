using System.Reflection;
using Assembler;
using Assembler.Grammar;
using Pegasus.Common;

namespace DCPU16.Tests.Assembler
{
    [TestClass]
    public class Playground
    {
        [TestMethod]
        public void Parse()
        {
            using var inputStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DCPU16.Tests.Assembler.DASM.pacman.dasm16");
            var asm = new StreamReader(inputStream!).ReadToEnd();

            try
            {
                var ast = new Parser().Parse(asm);
            }
            catch (FormatException ex)
            {
                var cursor = ex.Data["cursor"] as Cursor;
                if (cursor == null)
                    throw;

                var spaces = new string(' ', Math.Max(0, cursor.Column - 2));

                Assert.Fail($"\n{cursor.Subject}\n"
                          + $"{spaces}^ {ex.Message} (Ln{cursor.Line}, Col{cursor.Column - 1})\n");
            }
        }

        [TestMethod]
        public void Assemble()
        {
            using var inputStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DCPU16.Tests.Assembler.DASM.pacman.dasm16");
            var asm = new StreamReader(inputStream!).ReadToEnd();

            var output = new MemoryStream();
            try
            {
                var assembler = new DasmAssembler(asm);
                assembler.Assemble(output);
            }
            catch (FormatException ex)
            {
                var cursor = ex.Data["cursor"] as Cursor;
                if (cursor == null)
                    throw;

                var spaces = new string(' ', Math.Max(0, cursor.Column - 2));

                Assert.Fail($"\n{cursor.Subject}\n"
                            + $"{spaces}^ {ex.Message} (Ln{cursor.Line}, Col{cursor.Column - 1})\n");
            }

            var chunks = output.ToArray()
                .Chunk(2)
                .Select(a => BitConverter.ToUInt16(a))
                .Select(a => "0x" + a.ToString("X4"))
                .Chunk(12);
            foreach (var chunk in chunks)
                Console.WriteLine(string.Join(" ", chunk));
        }
    }
}