using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookingGame.Enum;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class Station : BaseSprite
    {
        private string stationTexture = "background";
        private Vector2 stationPosition = new Vector2(0, 400);

        public List<StationItem> stationItems = new List<StationItem>()
        {
        };
        
        public Station(Texture2D texture)
        {
            _texture = texture;
            _position = stationPosition;
        }
    }
}
