using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace CheeseCompositor.Core
{
    internal class CheeseProcessor
    {
        private string assetPath;
        private Config.Root config;

        private IReadOnlyDictionary<string, Config.Anchor> anchors;
        private IReadOnlyDictionary<string, Config.Modify> modifies;

        public CheeseProcessor(string assetPath, Config.Root config)
        {
            this.assetPath = assetPath;
            this.config = config;

            this.anchors = config.Anchors.ToDictionary(k => k.Key);
            this.modifies = config.Modifies.ToDictionary(k => k.Key);
        }

        public async IAsyncEnumerable<(string, Image)> ProcessAsync()
        {
            using var baseImage = await LoadImageAsync<Rgba32>(this.config.Base.Image);

            foreach (var output in this.config.Outputs)
            {
                var cheeseName = this.config.Base.Name + output.Name;
                var cheeseImage = new Image<Rgba32>(baseImage.Width, baseImage.Height, Color.Transparent);

                cheeseImage.Mutate(async context => {
                    DrawImageAtOffset(context, baseImage, output.BaseOffsetX, output.BaseOffsetY);

                    foreach (var part in output.Parts)
                    {
                        using var partImage = await LoadImageAsync<Rgba32>(part.Image);

                        if (this.modifies.TryGetValue(part.Modify ?? "", out var modify))
                        {
                            partImage.Mutate(context => {
                                ApplyImageModifier(context, modify);
                            });
                        }

                        int offsetX = output.BaseOffsetX + part.PositionX;
                        int offsetY = output.BaseOffsetY + part.PositionY;

                        if (this.anchors.TryGetValue(part.Anchor ?? "", out var anchor))
                        {
                            offsetX += anchor.PositionX;
                            offsetY += anchor.PositionY;
                        }

                        DrawImageAtOffset(context, partImage, offsetX, offsetY);
                    }
                });

                yield return (cheeseName, cheeseImage);
            }
        }

        private void DrawImageAtOffset(IImageProcessingContext context, Image image, int offsetX, int offsetY)
        {
            var offset = new Point(offsetX, offsetY) * Math.Max(1, this.config.Base.Scale);

            context.DrawImage(image, offset, new GraphicsOptions
            {
                ColorBlendingMode = PixelColorBlendingMode.Normal,
                AlphaCompositionMode = PixelAlphaCompositionMode.SrcOver,
                Antialias = false,
            });
        }

        private void ApplyImageModifier(IImageProcessingContext context, Config.Modify modify)
        {
            var modifier = new ImageModifier(context);

            foreach (var step in modify.Steps)
            {
                modifier.ApplyModifyStep(step);
            }
        }

        private async Task<Image<T>> LoadImageAsync<T>(string fileName) where T : unmanaged, IPixel<T>
        {
            using var stream = new FileStream(Path.Combine(this.assetPath, fileName), FileMode.Open);

            return await Image.LoadAsync<T>(stream);
        }
    }
}