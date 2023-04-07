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
    public class Order : BaseSprite
    {
        private List<Ingredient> ingredients;
        public OrderState State = OrderState.NotTaken;
        public int Price = 10;
        public string OrderText = "Flatbread, please";

        private Vector2 orderPosition = new Vector2(50, 120);

        public event EventHandler OnOrderCooked;
        
        public Order()
        {
            _position = orderPosition;
        }
        public Order(Texture2D texture)
        {
            _texture = texture;
            _position = orderPosition;
        }

        public void Take()
        {
            State = OrderState.Taken;
        }

        public void AddTexture(Texture2D texture)
        {
            _texture = texture;
        }

        public void Cook()
        {
            if (State != OrderState.Taken) return;
            State = OrderState.Done;
            OnOrderCooked?.Invoke(this, EventArgs.Empty);
        }
    }
}
