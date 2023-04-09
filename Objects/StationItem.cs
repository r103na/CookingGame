using CookingGame.Enum;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class StationItem : ClickableSprite
{
    private Ingredient ingredient;
    private bool isDragging = false;

    public StationItem(Ingredient ingredient, Texture2D texture)
    {
        this.ingredient = ingredient;
        var str = nameof(ingredient);
        _texture = texture;
        BoundX = 30;
        BoundY = 30;
    }
    public override void HandleClick(Point clickPosition)
    {
        if (IsInBounds(clickPosition))
        {
            OnClicked();
            _position.X = clickPosition.X;
            _position.Y = clickPosition.Y;
        }
    }

    public override void HandleHold(Point clickPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleRelease(Point clickPosition)
    {
        
    }
}