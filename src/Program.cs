using System;
using System.IO;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using CheeseCompositor.Config;
using CheeseCompositor.Core;

namespace CheeseCompositor
{
    internal class Program
    {
        const string CommonDir = "../_common";
        const string ConfigName = "cheese.json";
        const string OutputExtPng = ".png";

        private string sourceDir;
        private string outputDir;

        public Program(string sourceDir, string outputDir)
        {
            this.sourceDir = sourceDir;
            this.outputDir = outputDir;
        }

        public void Execute(string configName)
        {
            var config = LoadCheeseConfig(configName);

            var assetPath = Path.GetFullPath(this.sourceDir);
            var commonPath = Path.GetFullPath(Path.Combine(this.sourceDir, CommonDir));

            var processor = new CheeseProcessor(assetPath, commonPath, config);

            Directory.CreateDirectory(this.outputDir);

            foreach (var (name, image) in processor.Process())
            {
                SaveImageAsPng(name, image);
            }
        }

        private void SaveImageAsPng(string name, Image image)
        {
            string fileName = Path.ChangeExtension(name, OutputExtPng);
            using var stream = new FileStream(Path.Combine(this.outputDir, fileName), FileMode.Create);

            image.SaveAsPng(stream, new PngEncoder
            {
                BitDepth = PngBitDepth.Bit8,
                ColorType = PngColorType.RgbWithAlpha,
                TransparentColorMode = PngTransparentColorMode.Preserve,
            });
        }

        private Root LoadCheeseConfig(string configName)
        {
            var configData = File.ReadAllText(Path.Combine(this.sourceDir, configName));
            return JsonConvert.DeserializeObject<Root>(configData);
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: cheesecompositor <SOURCEDIR> <OUTPUTDIR>");
                Environment.Exit(1);
            }

            try
            {
                var program = new Program(args[0], args[1]);
                program.Execute(ConfigName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: {0}", ex.Message);
                Environment.Exit(2);
            }
        }
    }
}
