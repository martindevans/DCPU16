using Assembler.Grammar;
using Assembler.Grammar.AST.Instructions;
using Assembler.Grammar.AST.Operands;
using Pegasus.Common;

namespace DCPU16.Tests.Assembler
{
    [TestClass]
    public class ParserTests
    {
        private static BaseInstruction? Parse(string assembly, string? label = null)
        {
            try
            {
                var ast = new Parser().Parse(assembly);

                Assert.AreEqual(1, ast.Lines.Count);

                var li = ast.Lines[0];
                Assert.IsNotNull(li);
                Assert.AreEqual(label, li.Label);

                return li.Instruction;
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

        private static BasicInstruction CheckBasicInstruction(string assembly, BasicOpcode opcode, string? label = null)
        {
            var instr = (BasicInstruction?)Parse(assembly, label);
            Assert.IsNotNull(instr!);
            Assert.AreEqual(opcode, instr.Opcode);
            return instr;
        }

        private static BaseInstruction CheckSpecialInstruction(string assembly, SpecialOpcode opcode, string? label = null)
        {
            var instr = Parse(assembly, label);
            Assert.IsNotNull(instr!);

            if (instr is SpecialInstruction si)
            {
                Assert.AreEqual(opcode, si.Opcode);
                return si;
            }

            var psi = instr as ParameterlessSpecialInstruction;
            Assert.IsNotNull(psi!);
            Assert.AreEqual(opcode, psi.Opcode);
            return psi;
        }

        [TestMethod]
        public void ParseBasicInstruction()
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
        public void ParseSpecialInstruction()
        {
            var instr = CheckSpecialInstruction("RFI [A]", SpecialOpcode.RFI) as SpecialInstruction;
            Assert.IsNotNull(instr);

            var oa = instr.A;
            Assert.IsNotNull(oa);

            var ar = oa as IndirectRegister;
            Assert.IsNotNull(ar);
            Assert.AreEqual(Register.A, ar.Register);
        }

        [TestMethod]
        public void ParseParameterlessSpecialInstruction()
        {
            var instr = CheckSpecialInstruction("HCF", SpecialOpcode.HCF);

            Assert.IsInstanceOfType(instr, typeof(ParameterlessSpecialInstruction));
        }

        [TestMethod]
        public void ParseData()
        {
            var instr = Parse("dat 1,2,0xFF") as DatInstruction;
            Assert.IsNotNull(instr!);

            Assert.IsNotNull(instr.Numbers.Count);
            Assert.AreEqual(3, instr.Numbers.Count);
            Assert.AreEqual(1, instr.Numbers[0].Resolve(null!));
            Assert.AreEqual(2, instr.Numbers[1].Resolve(null!));
            Assert.AreEqual(255, instr.Numbers[2].Resolve(null!));
        }

        [TestMethod]
        public void ParseLabelledSimpleInstruction()
        {
            var instr = CheckBasicInstruction(":scroll_loop set A, B", BasicOpcode.SET, "scroll_loop");

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
        public void ParseLabelledLine()
        {
            Parse("    :scroll_loop", "scroll_loop");
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
            Assert.AreEqual(55, ar.Value.Resolve(null!));

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
            var instr = CheckBasicInstruction("set [a],label", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as NextWordLiteral;
            Assert.IsNotNull(ar);
            Assert.AreEqual("label", ar.Value.Label);

            var br = ob as IndirectRegister;
            Assert.IsNotNull(br);
            Assert.AreEqual(Register.A, br.Register);
        }

        [TestMethod]
        public void ParseIndirectRegisterNextWord()
        {
            var instr = CheckBasicInstruction("set [a+1],label", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as NextWordLiteral;
            Assert.IsNotNull(ar);
            Assert.AreEqual("label", ar.Value.Label);

            var br = ob as IndirectRegisterNextWord;
            Assert.IsNotNull(br);
            Assert.AreEqual(Register.A, br.Register);
            Assert.AreEqual(1, br.Value.Resolve(null!));
        }

        [TestMethod]
        public void ParseIndirectRegisterNextWordLabelled()
        {
            var instr = CheckBasicInstruction("set [a+label2],label", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as NextWordLiteral;
            Assert.IsNotNull(ar);
            Assert.AreEqual("label", ar.Value.Label);

            var br = ob as IndirectRegisterNextWord;
            Assert.IsNotNull(br);
            Assert.AreEqual(Register.A, br.Register);
            Assert.AreEqual("label2", br.Value.Label);
        }

        [TestMethod]
        public void ParseIndirectWord()
        {
            var instr = CheckBasicInstruction("set [111],POP", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as PushPop;
            Assert.IsNotNull(ar);
            Assert.IsFalse(ar.Push);

            var br = ob as IndirectNextWord;
            Assert.IsNotNull(br);
            Assert.AreEqual(111, br.Value.Resolve(null!));
        }

        [TestMethod]
        public void ParseIndirectLabel()
        {
            var instr = CheckBasicInstruction("set [hello_world],POP", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as PushPop;
            Assert.IsNotNull(ar);
            Assert.IsFalse(ar.Push);

            var br = ob as IndirectNextWord;
            Assert.IsNotNull(br);
            Assert.AreEqual("hello_world", br.Value.Label);
        }

        [TestMethod]
        public void ParseSmallLiteral()
        {
            var instr = CheckBasicInstruction("set PUSH,3", BasicOpcode.SET);

            var oa = instr.A;
            Assert.IsNotNull(oa);
            var ob = instr.B;
            Assert.IsNotNull(ob);

            var ar = oa as SmallLiteral;
            Assert.IsNotNull(ar);
            Assert.AreEqual(3, ar.Value);

            var br = ob as PushPop;
            Assert.IsNotNull(br);
            Assert.IsTrue(br.Push);
        }
    }
}
