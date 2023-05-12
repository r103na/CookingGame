using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;
public class Alert : ClickableSprite
{
    public Button ConfirmButton;

    public Alert(Texture2D texture, Texture2D buttonTexture)
    {
        Texture = texture;
        Position = new Vector2(440, 260);
        ConfirmButton = new Button(buttonTexture, new Vector2(560, 355));
        Layer = 10;
        ConfirmButton.Layer = 11;
    }
}

