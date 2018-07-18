using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace MonoGame.Library.Components
{
    public abstract class GameInputs : Updateable
    {
        protected GameInputs(Game game)
            : base(game)
        { }

        protected enum MouseButton
        {
            Left,
            Middle,
            Right,
            X1,
            X2
        }

        protected GamePadState _currentGamePad;
        protected KeyboardState _currentKeyboard;
        protected MouseState _currentMouse;

        protected GamePadState _oldGamePad;
        protected KeyboardState _oldKeyboard;
        protected MouseState _oldMouse;

        public virtual bool Exit => _currentGamePad.Buttons.Back == ButtonState.Pressed || _currentKeyboard.IsKeyDown(Keys.Escape);
        public int MouseWheelPosition { get; protected set; }
        public int MouseWheelDirection => _currentMouse.ScrollWheelValue > _oldMouse.ScrollWheelValue ? 1 : _currentMouse.ScrollWheelValue < _oldMouse.ScrollWheelValue ? -1 : 0;
        public Vector2 MouseDisplacement => (_currentMouse.Position - _oldMouse.Position).ToVector2();

        public virtual bool IsInputsAvailable => Exit;

        public override void Initialize()
        {
            _currentGamePad = GamePad.GetState(PlayerIndex.One);
            _currentKeyboard = Keyboard.GetState();
            _currentMouse = Mouse.GetState();
        }

        public override void Update(GameTime gameTime, GameInputs gameInputs)
        { }

        public override void Update(GameTime gameTime)
        {
            (_oldGamePad, _oldKeyboard, _oldMouse) = (_currentGamePad, _currentKeyboard, _currentMouse);
            _currentGamePad = GamePad.GetState(PlayerIndex.One);
            _currentKeyboard = Keyboard.GetState();
            _currentMouse = Mouse.GetState();
            MouseWheelPosition += MouseWheelDirection;
        }

        protected bool Clicked(Keys key)
            => _oldKeyboard.IsKeyUp(key) && _currentKeyboard.IsKeyDown(key);

        protected bool Clicked(Buttons button)
            => _oldGamePad.IsButtonUp(button) && _currentGamePad.IsButtonDown(button);

        protected bool Clicked(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return MouseClicked(_oldMouse.LeftButton, _currentMouse.LeftButton);
                case MouseButton.Middle:
                    return MouseClicked(_oldMouse.MiddleButton, _currentMouse.MiddleButton);
                case MouseButton.Right:
                    return MouseClicked(_oldMouse.RightButton, _currentMouse.RightButton);
                case MouseButton.X1:
                    return MouseClicked(_oldMouse.XButton1, _currentMouse.XButton1);
                case MouseButton.X2:
                    return MouseClicked(_oldMouse.XButton2, _currentMouse.XButton2);
                default:
                    throw new ArgumentOutOfRangeException(nameof(button));
            }

            bool MouseClicked(ButtonState oldMouse, ButtonState newMouse)
                => oldMouse == ButtonState.Released && newMouse == ButtonState.Pressed;
        }

        protected bool Released(Keys key)
            => _oldKeyboard.IsKeyDown(key) && _currentKeyboard.IsKeyUp(key);

        protected bool Released(Buttons button)
            => _oldGamePad.IsButtonDown(button) && _currentGamePad.IsButtonUp(button);

        protected bool Released(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return MouseReleased(_oldMouse.LeftButton, _currentMouse.LeftButton);
                case MouseButton.Middle:
                    return MouseReleased(_oldMouse.MiddleButton, _currentMouse.MiddleButton);
                case MouseButton.Right:
                    return MouseReleased(_oldMouse.RightButton, _currentMouse.RightButton);
                case MouseButton.X1:
                    return MouseReleased(_oldMouse.XButton1, _currentMouse.XButton1);
                case MouseButton.X2:
                    return MouseReleased(_oldMouse.XButton2, _currentMouse.XButton2);
                default:
                    throw new ArgumentOutOfRangeException(nameof(button));
            }

            bool MouseReleased(ButtonState oldMouse, ButtonState newMouse)
                => oldMouse == ButtonState.Pressed && newMouse == ButtonState.Released;
        }
    }
}
