using Assembler.Grammar;
using Pegasus.Common;

namespace DCPU16.Tests.Assembler
{
    [TestClass]
    public class Playground
    {
        private string asm = @"set pc,Start

:speed dat 500

:ticker dat 0
:randish dat 0

:delay
  set a,0
:delayloop
  ifg a,[speed]
  set pc,pop
  add a,1
  set pc,delayloop

:Start
jsr init
:MainLoop
  add [ticker],1
  jsr updatePlayer
  ife [dead],0
  jsr animate
  ife [dead],0
  jsr moveGhosts

  jsr delay
  jsr probeInput
  
  ife [pillsEaten],[pillsInMap]
  jsr completeLevel
set pc,mainloop";

        [TestMethod]
        public void Parse()
        {
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
    }
}