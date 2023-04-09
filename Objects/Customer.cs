using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

using CookingGame.Enum;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CookingGame.Objects
{
    public class Customer : ClickableSprite
    {
        private string Name = "Посетитель";
        public List<string> DialogueOptions = new List<string>() {"Hello", "Bye"};
        private string jsonFilePath = "Customers.json";
        
        private readonly Vector2 customerPosition = new Vector2(325, 185);
        public Order Order;
        
        public float Patience = 100;

        public event EventHandler OnCustomerPatienceRunOut;

        // use clicked event next time btw
        
        public Customer(Texture2D texture)
        {
            _texture = texture; // load texture from a character name
            _position = customerPosition;
            Order = new Order();
        }

        public override void HandleClick(Point clickPosition)
        {
            if (IsInBounds(clickPosition))
            {
                OnClicked();
                Order.Take();

            }
        }

        public override void HandleRelease(Point clickPosition)
        {
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
