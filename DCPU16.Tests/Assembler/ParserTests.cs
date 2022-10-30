using Assembler.Grammar;
using Assembler.Grammar.AST.Instructions;
using Assembler.Grammar.AST.Operands;

namespace DCPU16.Tests.Assembler
{
    [TestClass]
    public class ParserTests
    {
        private static BasicInstruction CheckBasicInstruction(string assembly, BasicOpcode opcode, string? label = null)
        {
            var ast = new Parser().Parse(assembly);

            Assert.AreEqual(1, ast.Lines.Count);

            var li = ast.Lines[0];
            Assert.IsNotNull(li);
            Assert.AreEqual(label, li.Label);

            var bi = li.Instruction as BasicInstruction;
            Assert.IsNotNull(bi);

            Assert.AreEqual(BasicOpcode.SET, opcode);

            return bi;
        }

        [TestMethod]
        public void ParseSimpleInstruction()
        {
            var instr = CheckBasicInstruction("     SET A, B", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as RegisterValue;
            Assert.IsNotNull(ar);
            Assert.AreEqual(Register.B, ar.Register);

            var br = ob as RegisterValue;
            Assert.IsNotNull(br);
            Assert.AreEqual(Register.A, br.Register);
        }

        [TestMethod]
        public void ParseLabelledSimpleInstruction()
        {
            var instr = CheckBasicInstruction(":hello SET A, B", BasicOpcode.SET, "hello");

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as RegisterValue;
            Assert.IsNotNull(ar);
            Assert.AreEqual(Register.B, ar.Register);

            var br = ob as RegisterValue;
            Assert.IsNotNull(br);
            Assert.AreEqual(Register.A, br.Register);
        }

        [TestMethod]
        public void ParseComment()
        {
            var ast = new Parser().Parse("; hello");

            Assert.AreEqual(1, ast.Lines.Count);
        }
    }
}
