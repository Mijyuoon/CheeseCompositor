using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using CheeseCompositor.Core;

namespace CheeseCompositor
{
    internal class Program
    {
        const string ConfigName = "cheese.json";
        const string OutputExtPng = ".png";

        private string sourceDir;
        private string outputDir;

        public Program(string sourceDir, string outputDir)
        {
            this.sourceDir = sourceDir;
            this.outputDir = outputDir;
        }

        public async Task ExecuteAsync(string configName)
        {
            var config = LoadCheeseConfig(configName);
            var processor = new CheeseProcessor(this.sourceDir, config);

            Directory.CreateDirectory(this.outputDir);

            await foreach (var (name, image) in processor.ProcessAsync())
            {
                await SaveImageAsPngAsync(name, image);
            }
        }

        private async Task SaveImageAsPngAsync(string name, Image image)
        {
            string fileName = Path.ChangeExtension(name, OutputExtPng);
            using var stream = new FileStream(Path.Combine(this.outputDir, fileName), FileMode.Create);

            await image.SaveAsPngAsync(stream, new PngEncoder
            {
                BitDepth = PngBitDepth.Bit8,
                ColorType = PngColorType.RgbWithAlpha,
                TransparentColorMode = PngTransparentColorMode.Preserve,
            });
        }

        private Config.Root LoadCheeseConfig(string configName)
        {
            var configData = File.ReadAllText(Path.Combine(this.sourceDir, configName));
            return JsonConvert.DeserializeObject<Config.Root>(configData);
        }

        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: cheesecompositor <SOURCEDIR> <OUTPUTDIR>");
                Environment.Exit(1);
            }

            try {
                var program = new Program(args[0], args[1]);
                await program.ExecuteAsync(ConfigName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("An error occurred: {0}", ex.Message);
                Environment.Exit(2);
            }
        }
    }
}
