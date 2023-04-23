using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class PatienceBar : ImageObject
    {
        private readonly Vector2 _position = new(50, 400);
        public PatienceBar(Texture2D texture) : base(texture)
        {
            Texture = texture;
            Position = _position;
        }

        public PatienceBar(Texture2D texture, Vector2 position) : base(texture, position)
        {
            Texture = texture;
            Position = position;
        }
    }
}
