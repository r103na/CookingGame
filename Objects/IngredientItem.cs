using CookingGame.Enum;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class IngredientItem : MovableSprite
    {
        public Ingredient Ingredient;
        public IngredientItem(Texture2D texture, Vector2 position, Ingredient ingredient) : base(texture, position)
        {
            Position = position;
            _texture = texture;
            Ingredient = ingredient;
            BoundX = 50;
            BoundY = 50;
        }
    }
}
