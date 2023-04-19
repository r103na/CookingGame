using CookingGame.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base
{
    public class BaseSprite
    {
        protected Texture2D Texture;

        public Vector2 Position;
        protected float Rotation = 0f;
        protected SpriteEffects Effects;
        protected Vector2 Center => new(Texture.Width / 2f, Texture.Height / 2f);
        protected Vector2 Uhm => Position + Center;

        public int Layer;

        private const string FallbackTexture = "Empty";
        protected Color Color = Color.White;

        public virtual void OnNotify(Events eventType) { }


        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Uhm, null, Color, Rotation, Center, 1, Effects, 0);
        }

        public void ChangeTexture(Texture2D texture)
        {
            Texture = texture;
        }

    }
}
