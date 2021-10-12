using System;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using CheeseCompositor.Config;
using CheeseCompositor.Config.ModifySteps;

namespace CheeseCompositor.Core
{
    internal delegate void DerivedImageModifierFunc(IImageProcessingContext context, string key);

    internal class ImageModifier
    {
        const string DerivedStepPrefix = "_";

        private IImageProcessingContext context;
        private DerivedImageModifierFunc deriveFunc;

        public ImageModifier(IImageProcessingContext context, DerivedImageModifierFunc deriveFunc = default)
        {
            this.context = context;
            this.deriveFunc = deriveFunc;
        }

        public void ApplyModifyStep(ModifyStep step)
        {
            switch (step)
            {
                case ModifyStepColor color: DoColorReplaceStep(color); break;
                case ModifyStepRotate rotate: DoImageRotateStep(rotate); break;
                case ModifyStepOpacity opacity: DoImageOpacityStep(opacity); break;
                case var modify when IsDerivedStep(modify): DoDerivedStep(modify); break;
                default: throw new ArgumentException($"unsupported modify step type: {step.Type}");
            }
        }

        private void DoColorReplaceStep(ModifyStepColor step)
        {
            var threshold = Math.Max(0.005f, step.Tolerance);
            var brush = new RecolorBrush(step.Source, step.Target, threshold);

            this.context.Fill(brush);
        }

        private void DoImageRotateStep(ModifyStepRotate step)
        {
            var (rotate, flip) = step.RotateMode switch
            {
                ModifyRotateMode.RotateLeft => (RotateMode.Rotate270, FlipMode.None),
                ModifyRotateMode.RotateRight => (RotateMode.Rotate90, FlipMode.None),
                ModifyRotateMode.Rotate180 => (RotateMode.Rotate180, FlipMode.None),
                ModifyRotateMode.FlipHorizontal => (RotateMode.None, FlipMode.Horizontal),
                ModifyRotateMode.FlipVertical => (RotateMode.None, FlipMode.Vertical),
                _ => throw new ArgumentException($"unsupported rotate mode: {step.RotateMode}"),
            };

            this.context.RotateFlip(rotate, flip);
        }

        private void DoImageOpacityStep(ModifyStepOpacity step)
        {
            var value = Math.Clamp(step.Value, 0f, 1f);

            this.context.Opacity(value);
        }

        private void DoDerivedStep(ModifyStep step)
        {
            this.deriveFunc?.Invoke(this.context, step.Type[DerivedStepPrefix.Length..]);
        }

        private static bool IsDerivedStep(ModifyStep step) => step.Type.StartsWith(DerivedStepPrefix);
    }
}