using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CookingGame.Objects.Base;

namespace CookingGame.Objects
{
    public class SplashImage : BaseSprite
    {
        public SplashImage(Texture2D texture)
        {
            _texture = texture;
        }
        public SplashImage(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }
    }
}
