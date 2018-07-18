using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.Library
{
    public class SpriteBatch
    {
        public static Texture2D Pixel { get; private set; }

        public static void LoadTextures(GraphicsDevice graphicsDevice)
        {
            Pixel = new Texture2D(graphicsDevice, 1, 1);
            Pixel.SetData(new[] { Color.White });
        }

        public Microsoft.Xna.Framework.Graphics.SpriteBatch RawSpriteBatch { get; }

        private SpriteBatch(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            RawSpriteBatch = spriteBatch;
        }

        public SpriteBatchDrawer Begin(SpriteSortMode sortMode = SpriteSortMode.Deferred, BlendState blendState = null, SamplerState samplerState = null, DepthStencilState depthStencilState = null, RasterizerState rasterizerState = null, Effect effect = null, Matrix? transformMatrix = null)
            => new SpriteBatchDrawer(RawSpriteBatch, sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);

        public static implicit operator SpriteBatch(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
            => new SpriteBatch(spriteBatch);
    }
}
