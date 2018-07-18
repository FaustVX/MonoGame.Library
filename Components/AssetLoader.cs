using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace MonoGame.Library.Components
{
    public class AssetLoader<T> : IGameComponent
    {
        private readonly Game _game;
        public AssetLoader(string name, Game game)
        {
            _game = game;
            Name = name;
            _game.Components.Add(this);
        }
        public string Name { get; }
        public T Asset { get; private set; }

        public void Initialize()
        {
            Asset = _game.Content.Load<T>(Name);
            _game.Components.Remove(this);
            System.Diagnostics.Debug.WriteLine(Name, "Loaded");
        }

        public static implicit operator T(AssetLoader<T> loader)
            => loader.Asset;
    }
}