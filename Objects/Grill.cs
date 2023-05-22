using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class Grill : ClickableSprite
{
    public float GrillWaitTime = 5f;
    public Grill(Texture2D texture)
    {
        Texture = texture;
        Position = new Vector2(1340, 80);
    }
}