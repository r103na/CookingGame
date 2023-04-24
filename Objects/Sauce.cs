using System;
using CookingGame.Enum;
using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class Sauce : IngredientItem
    {
        private readonly Vector2 _sauceOriginalPos = new(755, 400);
        public Sauce(Texture2D texture, Vector2 position, Ingredient ingredient = Ingredient.Sauce) : base(texture, position, ingredient)
        {
            Texture = texture;
            Position = position;
            Layer = 3;
            acceptableBounds = new Rectangle(735, 300, 480, 1400);
            BoundY = 0;
            BoundX = (int)Center.X * 2;
            IsSpriteTaken = false;
            Clicked += Rotate;
            Released += ResetRotation;
        }

        public void Rotate(object sender, EventArgs e)
        {
            Rotation = 1.5f * 2;
        }

        public void ResetRotation(object sender, EventArgs e)
        {
            Rotation = 0f;
        }

        public void ResetPosition()
        {
            Position = _sauceOriginalPos;
        }
    }
}
