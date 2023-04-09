using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class Text
    {
        public SpriteFont Font { get; }
        private readonly string _text;
        public Vector2 Position { get; }
        public Color color = new Color(82, 28, 32);

        public Text(SpriteFont font, string text, Vector2 position)
        {
            Font = font;
            _text = text;
            Position = position;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, _text, Position, color);
        }
    }
}