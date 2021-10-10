using System;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using CheeseCompositor.Config;
using CheeseCompositor.Config.ModifySteps;

namespace CheeseCompositor.Core
{
    internal class ImageModifier
    {
        private IImageProcessingContext context;

        public ImageModifier(IImageProcessingContext context)
        {
            this.context = context;
        }

        public void ApplyModifyStep(ModifyStep step)
        {
            switch (step)
            {
                case ModifyStepColor color: DoColorReplaceStep(color); break;
                default: throw new ArgumentException("unsupported modify step type");
            }
        }

        private void DoColorReplaceStep(ModifyStepColor step)
        {
            const float MinTolerance = 0.005f;

            var brush = new RecolorBrush(step.Source, step.Target, Math.Max(MinTolerance, step.Tolerance));
            this.context.Fill(brush);
        }
    }
}