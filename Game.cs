using Microsoft.Xna.Framework;

namespace MonoGame.Library
{
    public abstract class Game : Microsoft.Xna.Framework.Game
    {
        protected Game(Components.GameInputs gameInputs)
        {
            Components.Add(GameInputs = gameInputs);
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override sealed void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            // if(IsPaused)
            //     return;
            Update(gameTime, GameInputs);
        }

        // protected override void Draw(GameTime gameTime)
        // {
        //     base.Update(gameTime);
        //     if(IsPaused)
        //         return;
        // }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new Microsoft.Xna.Framework.Graphics.SpriteBatch(GraphicsDevice);
            SpriteBatch.LoadTextures(GraphicsDevice);
        }

        protected abstract void Update(GameTime gameTime, Components.GameInputs gameInputs);

        public Components.GameInputs GameInputs { get; }
        public SpriteBatch SpriteBatch { get; protected set; }
        // public bool IsPaused { get; set; }
        public GraphicsDeviceManager Graphics { get; }
    }

    public abstract class Game<TInputs> : Game
        where TInputs : Components.GameInputs
    {
        protected Game(TInputs gameInputs)
            : base(gameInputs)
        { }

        protected sealed override void Update(GameTime gameTime, Components.GameInputs gameInputs)
            => Update(gameTime, (TInputs)gameInputs);

        protected abstract void Update(GameTime gameTime, TInputs gameInputs);

        public new TInputs GameInputs => (TInputs)base.GameInputs;
    }
}
