using Assembler;
using CommandLine;
using Pegasus.Common;

var result = Parser.Default.ParseArguments<Options>(args)
    .WithNotParsed(errors =>
    {
        foreach (var error in errors)
            Console.WriteLine(error);
    });

if (result.Value == null)
    return;
var options = result.Value;

var asm = File.ReadAllText(options.Input.FullName);

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

    Console.Error.WriteLine($"\n{cursor.Subject}\n"
                          + $"{spaces}^ {ex.Message} (Ln{cursor.Line}, Col{cursor.Column - 1})\n");
}

if (options.Output != null)
{
    File.WriteAllBytes(options.Output.FullName, output.ToArray());
    Console.WriteLine($"Written {output.Length} bytes");
}
else
{
    var chunks = output.ToArray()
        .Chunk(2)
        .Select(a => BitConverter.ToUInt16(a))
        .Select(a => "0x" + a.ToString("X4"))
        .Chunk(12);
    foreach (var chunk in chunks)
        Console.WriteLine(string.Join(" ", chunk));
}


internal class Options
{
    [Option('i', "input", Required = true, HelpText = "Input file.")]
    public FileInfo Input { get; set; } = null!;

    [Option('o', "output", Required = false, HelpText = "Output file.")]
    public FileInfo? Output { get; set; } = null!;
}