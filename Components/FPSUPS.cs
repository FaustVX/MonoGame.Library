using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Components
{
    public class FpsUps : Drawable
    {
        private readonly FPS _fps;
        private readonly UPS _ups;

        public AssetLoader<SpriteFont> Font { get; set; }
        public Color Color { get; set; }

        public bool Enabled
        {
            get => base.Visible;
            set => _fps.Visible = _ups.Enabled = base.Visible = value;
        }

        public FpsUps(Game game, string fontName, Color color)
            : base(game)
        {
            Color = color;
            Game.Components.Add(_fps = new FPS());
            Game.Components.Add(_ups = new UPS());
            Font = new AssetLoader<SpriteFont>(fontName, Game);
        }

        public override void Draw(GameTime gameTime)
        {
            using (var spriteBatch = Game.SpriteBatch.Begin())
                spriteBatch.DrawString(Font, $"FPS:{_fps.FramesPerSecond}{Environment.NewLine}UPS:{_ups.UpdatesPerSecond}", Vector2.One, Color);
        }

        public int? TargetFPS
        {
            get => Game.IsFixedTimeStep ? (int)Game.TargetElapsedTime.ToFrequency() : (int?)null;
            set => (Game.IsFixedTimeStep, Game.TargetElapsedTime) = value is int i ? (true, ((decimal)i).FromFrequency()) : (false, Game.TargetElapsedTime);
        }

        public bool IsVSync
        {
            get => Game.Graphics.SynchronizeWithVerticalRetrace;
            set => Game.Graphics.SynchronizeWithVerticalRetrace = value;
        }
    }
}