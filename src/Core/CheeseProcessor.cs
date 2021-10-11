using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using CheeseCompositor.Config;

namespace CheeseCompositor.Core
{
    internal class CheeseProcessor
    {
        const string CommonPathPrefix = "@";
        const string BaseAnchorKey = "_base";
        
        private string assetPath;
        private string commonPath;
        private Root config;

        private IReadOnlyDictionary<string, Anchor> anchors;
        private IReadOnlyDictionary<string, Modify> modifies;

        public CheeseProcessor(string assetPath, string commonPath, Root config)
        {
            this.assetPath = assetPath;
            this.commonPath = commonPath;
            this.config = config;

            this.anchors = config.Anchors.ToDictionary(k => k.Key);
            this.modifies = config.Modifies.ToDictionary(k => k.Key);
        }

        public IEnumerable<(string, Image)> Process()
        {
            foreach (var output in this.config.Outputs)
            {
                var cheeseName = this.config.Props.Name + output.Name;
                var cheeseImage = new Image<Rgba32>(config.Props.Size, config.Props.Size, Color.Transparent);
                
                var baseParts = this.config.BaseParts
                    .Select(part => new OutputPart
                    {
                        Image = part.Image,
                        PositionX = part.PositionX,
                        PositionY = part.PositionY,
                        Modify = output.BaseModify,
                        Anchor = BaseAnchorKey,
                        Order = 0,
                    });

                var parts = baseParts
                    .Concat(output.Parts)
                    .OrderBy(p => p.Order);
                
                cheeseImage.Mutate(cheeseCtx =>
                {
                    foreach (var part in parts)
                    {
                        using var partImage = LoadImage<Rgba32>(part.Image);
                        var (offsetX, offsetY) = ResolveAnchor(part.Anchor ?? "", output);
                    
                        partImage.Mutate(partCtx =>
                        {
                            var inScale = part.InputScale > 0 ? part.InputScale : config.Props.InputScale;
                            
                            ApplyImageRescale(partCtx, inScale: inScale);
                            ApplyImageModifier(partCtx, part.Modify);
                        });
                        
                        DrawImageAtOffset(cheeseCtx, partImage, offsetX, offsetY);
                    }
                    
                    ApplyImageModifier(cheeseCtx, output.OutputModify);
                    ApplyImageRescale(cheeseCtx, outScale: config.Props.OutputScale);
                });

                yield return (cheeseName, cheeseImage);
            }
        }

        private (int, int) ResolveAnchor(string key, Output output)
        {
            var (offsetX, offsetY) = (0, 0);

            while (key is not null)
            {
                if (key == BaseAnchorKey)
                {
                    offsetX += output.BaseOffsetX;
                    offsetY += output.BaseOffsetY;
                    break;
                }

                var anchor = this.anchors.GetValueOrDefault(key);

                offsetX += anchor?.PositionX ?? 0;
                offsetY += anchor?.PositionY ?? 0;
                key = anchor?.Parent;
            }
            
            return (offsetX, offsetY);
        }

        private void DrawImageAtOffset(IImageProcessingContext context, Image image, int offsetX, int offsetY)
        {
            var offset = new Point(offsetX, offsetY);
            
            context.DrawImage(image, offset, new GraphicsOptions
            {
                ColorBlendingMode = PixelColorBlendingMode.Normal,
                AlphaCompositionMode = PixelAlphaCompositionMode.SrcOver,
                Antialias = false,
            });
        }

        private void ApplyImageRescale(IImageProcessingContext context, int inScale = 1, int outScale = 1)
        {
            if (outScale > 1)
            {
                var currentSize = context.GetCurrentSize();
                var resampler = new NearestNeighborResampler();
                
                context.Resize(currentSize.Width * outScale, currentSize.Height * outScale, resampler);
            }

            if (inScale > 1)
            {
                var currentSize = context.GetCurrentSize();
                var resampler = new NearestNeighborResampler();
                
                context.Resize(currentSize.Width / inScale, currentSize.Height / inScale, resampler);
            }
        }

        private void ApplyImageModifier(IImageProcessingContext context, string key)
        {
            if (this.modifies.TryGetValue(key ?? "", out var modify))
            {
                var modifier = new ImageModifier(context, this.ApplyImageModifier);

                foreach (var step in modify.Steps)
                {
                    modifier.ApplyModifyStep(step);
                }          
            }
        }

        private Image<T> LoadImage<T>(string fileName) where T : unmanaged, IPixel<T>
        {
            var imagePath = fileName.StartsWith(CommonPathPrefix)
                ? Path.Combine(this.commonPath, fileName[CommonPathPrefix.Length..])
                : Path.Combine(this.assetPath, fileName);

            using var stream = new FileStream(imagePath, FileMode.Open);
            
            return Image.Load<T>(stream);
        }
    }
}