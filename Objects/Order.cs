using System;
using System.IO;
using System.Text.Json;

using CookingGame.Objects.Base;
using CookingGame.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class Order : BaseSprite
    {
        public string OrderName { get; set; }

        public OrderState State;
        public int Price = 10;
        public string OrderText = "Flat bread, please";

        private readonly Vector2 _orderPosition = new(50, 120);

        public event EventHandler OrderCooked;

        private const string Filepath = "C:\\Users\\user\\source\\repos\\CookingGame\\CookingGame\\Data\\orders.json";

        public Order()
        {
            _position = _orderPosition;
            State = new NotTakenState(this);
            LoadOrderFromJson(Filepath);
        }

        public void Take()
        {
            State.Take();
        }

        public void AddTexture(Texture2D texture)
        {
            _texture = texture;
        }

        public void Cook()
        {
            State.Cook();
        }

        public virtual void OnOrderCooked()
        {
            OrderCooked?.Invoke(this, EventArgs.Empty);
        }

        public void LoadOrderFromJson(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var orderData = JsonSerializer.Deserialize<OrderData>(jsonString);
            OrderName = orderData?.OrderName;
            if (orderData != null) 
                OrderText = orderData.Dialogue;
            //_ingredients = orderData?.Ingredients;
        }
    }

    public class OrderData
    {
        public string OrderName { get; set; }
        public string Dialogue { get; set; }
    }
}



