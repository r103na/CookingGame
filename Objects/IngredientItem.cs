using CookingGame.Enum;
using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CookingGame.Objects;

public class IngredientItem : MovableSprite
{
    public Ingredient Ingredient;
    public IngredientItem(Texture2D texture, Vector2 position, Ingredient ingredient) : base(texture, position)
    {
        Position = position;
        Texture = texture;
        Ingredient = ingredient;
        Layer = 2;
        AcceptableBounds = new Rectangle(735, 0, 460, 1400);
        IsSpriteTaken = true;
    }
    public void CheckIfInBounds()
    {
        if (Position.Y < AcceptableBounds.Y)
        {
            Position.Y = AcceptableBounds.Y;
        }

        if (Position.X < AcceptableBounds.X)
        {
            Position.X = AcceptableBounds.X;
        }
        if (Position.X > AcceptableBounds.Right)
        {
            Position.X = AcceptableBounds.Right;
        }
    }
}