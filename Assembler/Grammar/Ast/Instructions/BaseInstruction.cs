namespace Assembler.Grammar.AST.Instructions
{
    public abstract class BaseInstruction
    {
        /// <summary>
        /// Get the length of this instruction once it is emitted
        /// </summary>
        public abstract uint WordLength { get; }

        /// <summary>
        /// Get the words which make up this instruction
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<ushort> Emit(IReadOnlyDictionary<string, ushort> map);
    }
}
