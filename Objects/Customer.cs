using System;

using CookingGame.Objects.Base;

using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CookingGame.Objects
{
    public class Customer : ClickableSprite
    {
        private readonly Vector2 _customerPosition = new Vector2(325, 185);
        public Order Order;

        public string Name;

        public float Patience = 175;

        public event EventHandler OnCustomerPatienceRunOut;

        // use clicked event next time btw
        
        public Customer(Texture2D texture, string name)
        {
            _texture = texture; // load texture from a character name
            Position = _customerPosition;
            Order = new Order();
            Name = name;
        }

        public void DecreasePatience(float value)
        {
            Patience -= value;
        }

        public void OnPatienceRunOut()
        {
            OnCustomerPatienceRunOut?.Invoke(this, EventArgs.Empty);
        }

        public void ChangeTexture(Texture2D texture)
        {
            _texture = texture;
        }
    }
}
