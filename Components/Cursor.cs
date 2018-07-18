using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library.Components
{
    public class Cursor : CompleteComponent
    {
        private Vector2 _mousePosition;

        public AssetLoader<Texture2D> Texture { get; set; }
        public Point Size { get; set; }
        public Point Offset { get; set; }

        private bool _isNative;
        public bool IsNative
        {
            get => _isNative;
            set
            {
                if (IsNative == value)
                    return;
                _isNative = value;
                SetMouseVisibility();
            }
        }

        public override bool Visible
        {
            get => base.Visible;
            set
            {
                if(Visible == value)
                    return;
                base.Visible = value;
                SetMouseVisibility();
            }
        }

        private void SetMouseVisibility()
        {
            if(!Visible || !IsNative)
                Game.IsMouseVisible = false;
            else if(IsNative)
                Game.IsMouseVisible = true;
        }

        public Cursor(Game game, string textureName, Point size, int drawOrder = 0)
            : this(game, textureName, drawOrder)
        {
            Size = size;
        }
        public Cursor(Game game, string textureName, int drawOrder = 0)
            : base(game)
        {
            Visible = true;
            DrawOrder = drawOrder;
            Texture = new AssetLoader<Texture2D>(textureName, game);
        }

        public override void Initialize()
        {
            if (Size == Point.Zero)
                Size = Texture.Asset.Bounds.Size;

            System.Diagnostics.Debug.WriteLine(Size, Texture.Name);
        }

        public override void Draw(GameTime gameTime)
        {
            if(Visible && !IsNative)
                using (var spriteBatch = Game.SpriteBatch.Begin())
                    spriteBatch.Draw(Texture, new Rectangle(_mousePosition.ToPoint() - Offset, Size), Color.White);
        }

        public override void Update(GameTime gameTime, GameInputs gameInputs)
        {
            _mousePosition += gameInputs.MouseDisplacement;
        }
    }
}