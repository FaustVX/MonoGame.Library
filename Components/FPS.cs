using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Library.Components
{
    public class FPS : Drawable
    {
        public FPS()
            : base(null)
        { }

        public int FramesPerSecond { get; private set; } = 0;
        
        public override void Draw(GameTime gameTime)
            => FramesPerSecond = (int)Math.Round((gameTime.ToFrequency()));
    }
}