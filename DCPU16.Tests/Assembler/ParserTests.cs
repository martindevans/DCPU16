using Assembler.Grammar;
using Assembler.Grammar.AST.Instructions;
using Assembler.Grammar.AST.Operands;
using Pegasus.Common;

namespace DCPU16.Tests.Assembler
{
    [TestClass]
    public class ParserTests
    {
        private static BasicInstruction CheckBasicInstruction(string assembly, BasicOpcode opcode, string? label = null)
        {
            try
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
            catch (FormatException ex)
            {
                var cursor = ex.Data["cursor"] as Cursor;
                if (cursor == null)
                    throw;

                var spaces = new string(' ', Math.Max(0, cursor.Column - 2));

                Assert.Fail($"\n{cursor.Subject}\n"
                          + $"{spaces}^ {ex.Message} (Ln{cursor.Line}, Col{cursor.Column - 1})\n");

                return null!;
            }
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
            var instr = CheckBasicInstruction(":hello set A, B", BasicOpcode.SET, "hello");

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
        public void ParseSetRegisterInstruction()
        {
            var instr = CheckBasicInstruction("     SET A, 55", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as NextWordLiteral;
            Assert.IsNotNull(ar);
            Assert.AreEqual(55, ar.Value);

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

        [TestMethod]
        public void ParseIndirectRegister()
        {
            var instr = CheckBasicInstruction("set [a+1],label", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as LabelValue;
            Assert.IsNotNull(ar);
            Assert.AreEqual("label", ar.Value);

            var br = ob as IndirectRegisterNextWord;
            Assert.IsNotNull(br);
            Assert.AreEqual(Register.A, br.Register);
            Assert.AreEqual(1, br.Value);
        }
    }
}
