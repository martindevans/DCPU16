using System.Text;
using Assembler.Grammar;

namespace Assembler
{
    //test case: http://fingswotidun.com/dcpu16/pac.html

    public class DasmAssembler
    {
        private readonly string _input;

        public DasmAssembler(string input)
        {
            _input = input;
        }

        public void Assemble(Stream output)
        {
            var ast = new Parser().Parse(_input);

            // Map out label positions
            var offset = (ushort)0;
            var map = new LabelMap();
            foreach (var line in ast.Lines)
            {
                if (line.Label != null)
                    map.Add(line.Label, offset);

                if (line.Instruction != null)
                    offset += checked((ushort)line.Instruction.WordLength);
            }

            // Emit code
            var expectedOffset = (ushort)0;
            var actualOffset = (ushort)0;
            using (var writer = new BinaryWriter(output, Encoding.Unicode, true))
            {
                foreach (var line in ast.Lines)
                {
                    if (line.Instruction == null)
                        continue;

                    foreach (var word in line.Instruction.Emit(map))
                    {
                        actualOffset++;
                        writer.Write(word);
                    }

                    expectedOffset += checked((ushort)line.Instruction.WordLength);

                    if (expectedOffset != actualOffset)
                        throw new InvalidOperationException("Instruction length mismatch");
                }
            }
        }
    }
}

/*


set pc,Start

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
set pc,mainloop



 */