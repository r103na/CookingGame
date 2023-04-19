using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class Sauce : MovableSprite
    {
        public Sauce(Texture2D texture, Vector2 position) : base(texture, position)
        {
            _texture = texture;
            Position = position;
            zIndex = 1;
        }

        public void Rotate()
        {

        }
    }
}
