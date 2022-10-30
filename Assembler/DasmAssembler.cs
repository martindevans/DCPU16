using Assembler.Grammar;

namespace Assembler
{
    //test case: http://fingswotidun.com/dcpu16/pac.html

    public class DasmAssembler
    {
        private readonly string _input;
        private readonly Func<string, string> _loadFile;

        public DasmAssembler(string input, Func<string, string> loadFile)
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