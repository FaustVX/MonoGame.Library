# MonoGame.Library
A Library for easing the use of MonoGame

## How to use

``` csharp
using MonoGame.Library;
using MonoGame.Library.Components;
using XNA = Microsoft.Xna.Framework;

class MyGameInputs : GameInputs
{
    public MyGameInputs()
        : base(null)
    { }

    public bool IsMouseLeftCliched => Clicked(MouseButton.Left);
}

class Game1 : Game<MyGameInputs>
{
    public Cursor Cursor { get; }
    public FpsUps FPS { get; }

    public Game1()
        : base(new MyGameInputs())
    {
        Components.Add(FPS = new FpsUps(this, "Fonts/Arial", XNA.Color.Black) { TargetFPS = 60 /*Target FPS to null for maximum fps*/
        /*VSync to true/false*/
        /*You can draw it with Visible*/ });
        Components.Add(Cursor = new Cursor(this, "Images/Cursor", new XNA.Point(24)) { Offset = new XNA.Point(24, 0) /*You can use IsNative/Visible*/ });

        /*
        Add all of you componnents with Components.Add
        for your components, use MonoGame.Library.Components base classes
        Also, you can use MonoGame.Library.Components.AssetLoader<T> as component for loading Asset (such as Texture2D, SpriteFont, ...), they will automatically load the ressource.
        */
    }

    /// <summary>
    /// Allows the game to run logic such as updating the world,
    /// checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    /// <param name="gameInputs">Provides a snapshot of game inputs.</param>
    protected override void Update(XNA.GameTime gameTime, MyGameInputs gameInputs)
    {
        if (gameInputs.Exit)
            Exit();

        // TODO: Add your update logic here
    }

    /// <summary>
    /// This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(XNA.GameTime gameTime)
    {
        GraphicsDevice.Clear(XNA.Color.CornflowerBlue);

        using (var spriteBatch = SpriteBatch.Begin())
        {
            // You can Draw here

            // You can also offset the origin
            using (var offset = spriteBatch.Offset(new XNA.Vector2D(5, 5)))
            {

            }
        }

        base.Draw(gameTime);
    }
}
```