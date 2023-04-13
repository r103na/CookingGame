using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

using CookingGame.Enum;
using CookingGame.Objects.Base;
using CookingGame.States;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class Order : BaseSprite
    {
        #region VARIABLES
        public string OrderName { get; private set; }

        public OrderState State;
        public int Price = 10;
        public string OrderText = "Flat bread, please";
        public List<Ingredient> Ingredients = new();

        private readonly Vector2 _orderPosition = new(50, 120);

        public event EventHandler OrderCooked;

        private const string Filepath = "C:\\Users\\user\\source\\repos\\CookingGame\\CookingGame\\Data\\recipes\\classic.json";
        #endregion

        public Order()
        {
            Position = _orderPosition;
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

        public int CheckIngredients(List<Ingredient> ingredients)
        {
            return ingredients.Count(ingredient => Ingredients.Contains(ingredient));
        }

        public void LoadOrderFromJson(string filePath)
        {
            var jsonString = File.ReadAllText(filePath);
            var orderData = JsonSerializer.Deserialize<OrderData>(jsonString);
            OrderName = orderData?.OrderName;
            OrderText = orderData?.Dialogue;
            Ingredients = orderData?.Ingredients.Select(s => (Ingredient)System.Enum.Parse(typeof(Ingredient), s)).ToList();
        }
    }

    public class OrderData
    {
        public string OrderName { get; set; }
        public string Dialogue { get; set; }
        public int Price { get; set; }
        public string[] Ingredients { get; set; }
    }
}



