using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Library.Components
{
    public class UPS : Updateable
    {
        public UPS()
            : base(null)
        { }

        public int UpdatesPerSecond { get; private set; } = 0;

        public override void Update(GameTime gameTime, GameInputs gameInputs)
        { }

        public override void Update(GameTime gameTime)
            => UpdatesPerSecond = (int)Math.Round((gameTime.ToFrequency()));
    }
}