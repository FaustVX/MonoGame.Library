using System;
using Microsoft.Xna.Framework;

namespace MonoGame.Library.Components
{
    public abstract class Updateable : IGameComponent, IUpdateable
    {
        public Game Game { get; }
        protected Updateable(Game game)
        {
            Game = game;
        }

        private bool _enabled = true;
        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == Enabled)
                    return;
                _enabled = value;
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> EnabledChanged;

        private int _updateOrder = 0;
        public virtual int UpdateOrder
        {
            get => _updateOrder;
            set
            {
                if (value == UpdateOrder)
                    return;
                _updateOrder = value;
                UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public virtual void Update(GameTime gameTime)
            => Update(gameTime, Game.GameInputs);
        public abstract void Update(GameTime gameTime, GameInputs gameInputs);


        public virtual void Initialize()
        { }
    }

    public abstract class Drawable : IGameComponent, IDrawable
    {
        protected Drawable(Game game)
        {
            Game = game;
        }

        private bool _visible = true;
        public virtual bool Visible
        {
            get => _visible;
            set
            {
                if (value == Visible)
                    return;
                _visible = value;
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> VisibleChanged;

        private int _drawOrder = 0;
        public virtual int DrawOrder
        {
            get => _drawOrder;
            set
            {
                if (value == DrawOrder)
                    return;
                _drawOrder = value;
                DrawOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public Game Game { get; }

        public event EventHandler<EventArgs> DrawOrderChanged;

        public abstract void Draw(GameTime gameTime);

        public virtual void Initialize()
        { }
    }

    public abstract class CompleteComponent : IGameComponent, IUpdateable, IDrawable
    {
        public Game Game { get; }
        protected CompleteComponent(Game game)
        {
            Game = game;
        }

        private bool _enabled = true;
        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                if (value == Enabled)
                    return;
                _enabled = value;
                EnabledChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> EnabledChanged;

        private int _updateOrder = 0;
        public virtual int UpdateOrder
        {
            get => _updateOrder;
            set
            {
                if (value == UpdateOrder)
                    return;
                _updateOrder = value;
                UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> UpdateOrderChanged;
        private bool _visible = true;
        public virtual bool Visible
        {
            get => _visible;
            set
            {
                if (value == Visible)
                    return;
                _visible = value;
                VisibleChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> VisibleChanged;

        private int _drawOrder = 0;
        public virtual int DrawOrder
        {
            get => _drawOrder;
            set
            {
                if (value == DrawOrder)
                    return;
                _drawOrder = value;
                DrawOrderChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> DrawOrderChanged;

        public abstract void Draw(GameTime gameTime);

        public virtual void Update(GameTime gameTime)
            => Update(gameTime, Game.GameInputs);
        public abstract void Update(GameTime gameTime, GameInputs gameInputs);

        public virtual void Initialize()
        { }
    }
}