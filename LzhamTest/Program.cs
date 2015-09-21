using System;
using System.IO;
using CommandLine;
using LzhamWrapper;

namespace LzhamTest
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                return Parser.Default.ParseArguments<CompressionOptions, DecompressionOptions>(args)
                    .MapResult(
                        (CompressionOptions opts) => Compress(opts),
                        (DecompressionOptions opts) => Decompress(opts),
                        errs => 1);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error:");
                Console.WriteLine(ex.ToString());
            }
            return 2;
        }

        private static int Compress(CompressionOptions opts)
        {
            Console.WriteLine("Compressing...");
            File.Delete(opts.OutputFile);
            Stream basestream = File.OpenWrite(opts.OutputFile);
            Stream input = File.OpenRead(opts.InputFile);
            CompressionParameters p = opts.GetCompressionParameters();
            LzhamStream s = new LzhamStream(basestream, p);
            input.CopyTo(s);
            s.Close();
            input.Close();
            basestream.Close();
            return 0;
        }

        private static int Decompress(DecompressionOptions opts)
        {
            Console.WriteLine("Decompressing...");
            Stream input = File.OpenRead(opts.InputFile);
            File.Delete(opts.OutputFile);
            Stream output = File.OpenWrite(opts.OutputFile);
            DecompressionParameters p = opts.GetDecompressionParameters();
            LzhamStream s = new LzhamStream(input, p);
            s.CopyTo(output);
            s.Close();
            input.Close();
            output.Close();
            return 0;
        }
    }
}
