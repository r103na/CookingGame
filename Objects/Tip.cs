using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class Tip : BaseSprite
    {
        public bool TipUsed;
        public Tip(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public void UpdatePosition(Vector2 position)
        {
            Position = position;
        }
    }
}
