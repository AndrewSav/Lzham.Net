using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using LzhamWrapper;
using LzhamWrapper.Enums;

namespace LzhamTest
{
    [Verb("d", HelpText = "Decompress infile into outfile")]
    public class DecompressionOptions
    {
        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Decompress", new DecompressionOptions { InputFile = "infile", OutputFile = "outfile" });
            }
        }

        [Option('d', Default = 26u, HelpText = "Set log2 dictionary size, max. is 26 on x86 platforms, 29 on x64. Default is 26 (64MB)", MetaValue = "size")]
        public uint DictionarySize { get; set; }
        [Option('t', Default = TableUpdateRate.Default, HelpText = "Set Huffman table update frequency. 0=Internal def, Def=8, higher=faster. Lower settings=slower decompression, but higher ratio. Note 1=impractically slow", MetaValue = "rate")]
        public TableUpdateRate UpdateRate { get; set; }
        [Option('z', HelpText = "Write/read zlib header/adler footer", SetName = "bla3")]
        public bool Zlib { get; set; }
        [Option('u', HelpText = "Use unbuffered decompression on files that can fit into memory.Unbuffered decompression is faster, but may have more I/O overhead")]
        public bool Unbuffered { get; set; }
        [Option('c', HelpText = "Do not compute or verify adler32 checksum during decompression (faster)")]
        public bool NoAdler { get; set; }
        [Value(0, Required = true, HelpText = "File to compress", MetaName = "infile")]
        public string InputFile { get; set; }
        [Value(1, Required = true, HelpText = "File name for compression output", MetaName = "outfile")]
        public string OutputFile { get; set; }


        public DecompressionParameters GetDecompressionParameters()
        {
            DecompressionParameters result = new DecompressionParameters
            {
                DictionarySize = DictionarySize,
                UpdateRate = UpdateRate,
            };

            result.Flags |= Zlib ? DecompressionFlag.ReadZlibStream : 0;
            result.Flags |= Unbuffered ? DecompressionFlag.OutputUnbuffered : 0;
            result.Flags |= NoAdler ? 0 : DecompressionFlag.ComputeAdler32;
            return result;
        }
    }
}
