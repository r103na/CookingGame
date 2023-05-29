using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects;

public class Text
{
    public SpriteFont Font { get; }
    public string StringText;
    public Vector2 Position { get; }
    public Color color = new Color(82, 28, 32);

    public Text(SpriteFont font, string stringText, Vector2 position)
    {
        Font = font;
        StringText = stringText;
        Position = position;
    }

    public void Render(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawString(Font, StringText, Position, color);
    }
}