using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookingGame.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects.Base
{
    public class BaseSprite
    {
        protected Texture2D _texture;

        public Vector2 _position;

        public int zIndex;

        private const string FallbackTexture = "Empty";
        private ContentManager _contentManager;

        public void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public virtual void OnNotify(Events eventType) { }

        public void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }


        protected Texture2D LoadTexture(string textureName)
        {
            var texture = _contentManager.Load<Texture2D>(textureName);
            return texture ?? _contentManager.Load<Texture2D>
                (FallbackTexture);
        }

    }
}
