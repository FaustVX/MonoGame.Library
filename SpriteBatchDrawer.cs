using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MonoGame.Library
{
    public struct SpriteBatchDrawer : IDisposable
    {
        private Rectangle AddOffset(Rectangle rectangle)
        {
            return Transform.Apply(rectangle);
            //return new Rectangle(rectangle.Location + _offset.ToPoint(), rectangle.Size);
        }

        private Point AddOffset(Point point)
        {
            return Transform.Apply(point);
            //return point + _offset.ToPoint();
        }

        private Vector2 AddOffset(Vector2 vector2)
        {
            return Transform.Apply(vector2);
            //return vector2 + _offset;
        }

        public readonly Microsoft.Xna.Framework.Graphics.SpriteBatch SpriteBatch;
        //private readonly Vector2 _offset;
        private readonly bool _isChild;
        public Transform Transform { get; }

        public SpriteBatchDrawer(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, SpriteSortMode sortMode, BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix? transformMatrix)
        {
            Transform = Transform.None();
            SpriteBatch = spriteBatch;
            SpriteBatch.Begin(sortMode, blendState, samplerState, depthStencilState, rasterizerState, effect, transformMatrix);
            _isChild = false;
        }

        private SpriteBatchDrawer(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Transform transform)
        {
            SpriteBatch = spriteBatch;
            //_offset = offset;
            //LocalOffset = localOffset;
            Transform = transform;
            _isChild = true;
        }

        void IDisposable.Dispose()
        {
            if (!_isChild)
                SpriteBatch.End();
        }

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
            => SpriteBatch.Draw(texture, AddOffset(destinationRectangle), color);

        public void Draw(Texture2D texture, Vector2 position, Color color)
            => SpriteBatch.Draw(texture, AddOffset(position), color);

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
            => SpriteBatch.Draw(texture, AddOffset(position), sourceRectangle, color);

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, SpriteEffects effects, float layerDepth)
            => SpriteBatch.Draw(texture, AddOffset(destinationRectangle), sourceRectangle, color, rotation, origin, effects, layerDepth);

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
            => SpriteBatch.Draw(texture, AddOffset(destinationRectangle), sourceRectangle, color);

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            => SpriteBatch.Draw(texture, AddOffset(position), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
            => SpriteBatch.Draw(texture, AddOffset(position), sourceRectangle, color, rotation, origin, scale, effects, layerDepth);

        public void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            => SpriteBatch.DrawString(spriteFont, text, AddOffset(position), color, rotation, origin, scale, effects, layerDepth);

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color)
            => SpriteBatch.DrawString(spriteFont, text, AddOffset(position), color);

        public void DrawStringCentered(SpriteFont spriteFont, string text, Vector2 position, Color color)
            => DrawStringCentered(spriteFont, text, position, new Vector2(.5f), color);

        public void DrawStringCentered(SpriteFont spriteFont, string text, Vector2 position, Vector2 center, Color color)
        {
            var size = spriteFont.MeasureString(text);
            position -= size * center;
            SpriteBatch.DrawString(spriteFont, text, AddOffset(position), color);
        }

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
            => SpriteBatch.DrawString(spriteFont, text, AddOffset(position), color, rotation, origin, scale, effects, layerDepth);

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
            => SpriteBatch.DrawString(spriteFont, text, AddOffset(position), color, rotation, origin, scale, effects, layerDepth);

        public void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color)
            => SpriteBatch.DrawString(spriteFont, text, AddOffset(position), color);

        public void DrawString(SpriteFont spriteFont, System.Text.StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
            => SpriteBatch.DrawString(spriteFont, text, AddOffset(position), color, rotation, origin, scale, effects, layerDepth);


        public void DrawLine(Point start, Point end, int width, Color color)
        {
            var edge = (end - start).ToVector2();
            var angle = (float)Math.Atan2(edge.Y, edge.X);
            DrawLine(start, (int)edge.Length(), angle, width, color);
        }

        public void DrawLine(Point center, int radius, float angle, int width, Color color)
            => SpriteBatch.Draw(Library.SpriteBatch.Pixel, AddOffset(new Rectangle(center, new Point(radius, width))), null, color, angle, new Vector2(0, .5f), SpriteEffects.None, 0);


        public void DrawCircle(Point center, int radius, Color color, int? lineWidth = null)
        {
            lineWidth = lineWidth ?? 5;
            for (var angle = 0; angle < 360; angle++)
            {
                var rad = angle / 360d * 2 * Math.PI;
                DrawLine(center, radius, (float)rad, lineWidth.Value, color);
            }
        }

        public void DrawPoint(Point position, int size, Color color)
            => SpriteBatch.Draw(Library.SpriteBatch.Pixel, AddOffset(new Rectangle((position.ToVector2() - new Vector2(size) / 2).ToPoint(), new Point(size))), color);

        public void DrawRectangle(Rectangle rectangle, Color color)
            => SpriteBatch.Draw(Library.SpriteBatch.Pixel, AddOffset(rectangle), color);

        public void DrawRectangle(Vector2 size, Color color)
            => DrawRectangle(new Rectangle(Point.Zero, size.ToPoint()), color);

        public SpriteBatchShape DrawShape(int width, Color color, bool close = false)
            => new SpriteBatchShape(this, width, color, close);

        public SpriteBatchShape DrawShape(int width, Point start, Color color, bool close = false)
            => new SpriteBatchShape(this, start, width, color, close);

        public SpriteBatchDrawer Offset(Vector2 offset)
            => new SpriteBatchDrawer(SpriteBatch, Transform.Move(offset));
    }
}
