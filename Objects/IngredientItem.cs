using CookingGame.Enum;
using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CookingGame.Objects
{
    public class IngredientItem : MovableSprite
    {
        public Ingredient Ingredient;
        public IngredientItem(Texture2D texture, Vector2 position, Ingredient ingredient) : base(texture, position)
        {
            Position = position;
            Texture = texture;
            Ingredient = ingredient;
            Layer = 2;
            acceptableBounds = new Rectangle(735, 0, 700, 1400);
        }
        public void CheckIfInBounds(Vector2 bounds)
        {
            if (Position.Y < acceptableBounds.Y)
            {
                Position.Y = acceptableBounds.Y;
            }

            if (Position.X < acceptableBounds.X)
            {
                Position.X = acceptableBounds.X;
            }
        }
    }
}
