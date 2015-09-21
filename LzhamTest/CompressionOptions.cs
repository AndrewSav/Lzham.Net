using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using LzhamWrapper;
using LzhamWrapper.Enums;

namespace LzhamTest
{
    [Verb("c", HelpText = "Compress infile into outfile")]
    public class CompressionOptions
    {
        [Usage]
        public static IEnumerable<Example> Examples
        {
            get
            {
                yield return new Example("Compress", new CompressionOptions { InputFile = "infile", OutputFile = "outfile" });
            }
        }

        [Option('d', Default = 26u, HelpText = "Set log2 dictionary size, max. is 26 on x86 platforms, 29 on x64. Default is 26 (64MB)", MetaValue = "size")]
        public uint DictionarySize { get; set; }
        [Option('m', Default = CompressionLevel.Uber, HelpText = "Compression level: 0=fastest, 1=faster, 2=default, 3=better, 4=uber. Default is uber (4)", MetaValue = "level")]
        public CompressionLevel Level { get; set; }
        [Option('t', Default = TableUpdateRate.Default, HelpText = "Set Huffman table update frequency. 0=Internal def, Def=8, higher=faster. Lower settings=slower decompression, but higher ratio. Note 1=impractically slow", MetaValue = "rate")]
        public TableUpdateRate UpdateRate { get; set; }
        [Option('h', Default = 8, HelpText = "Number of extra compression helper threads. Default=# CPU's-1. Note: The total number of threads will be 1 + num_helper_threads, because the main thread is counted separately", MetaValue = "threads")]
        public int HelperThreads { get; set; }
        [Option('x', HelpText = "Extreme parsing, for slight compression gain (Uber only, MUCH slower)")]
        public bool ExtremeParsing { get; set; }
        [Option('z', HelpText = "Write/read zlib header/adler footer")]
        public bool Zlib { get; set; }
        [Option('o', HelpText = "Permit the compressor to trade off decompression rate for higher ratios. Note: This flag can drop the decompression rate by 30% or more")]
        public bool Tradeoff { get; set; }
        [Option('e', HelpText = "Enable deterministic parsing for slightly higher compression and predictable output files when enabled, but less scalability. The default is disabled, so the generated output data may slightly vary between runs when multithreaded compression is enabled")]
        public bool DeterministicParsing { get; set; }
        [Value(0,Required = true, HelpText = "File to compress", MetaName="infile")]
        public string InputFile { get; set; }
        [Value(1, Required = true, HelpText = "File name for compression output", MetaName = "outfile")]
        public string OutputFile { get; set; }

        public CompressionParameters GetCompressionParameters()
        {
            CompressionParameters result = new CompressionParameters
            {
                DictionarySize = DictionarySize,
                Level = Level,
                UpdateRate = UpdateRate,
                HelperThreads = HelperThreads
            };

            result.Flags |= Zlib ? CompressionFlag.WriteZlibStream : 0;
            result.Flags |= Tradeoff ? CompressionFlag.TradeIffDecompressionForCompressionRatio : 0;
            result.Flags |= DeterministicParsing ? CompressionFlag.DeterministicParsing : 0;
            result.Flags |= ExtremeParsing ? CompressionFlag.ExtremeParsing : 0;
            return result;
        }
    }
}
