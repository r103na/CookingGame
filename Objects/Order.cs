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
        public List<string> TransladedIngredients;

        private readonly Vector2 _orderPosition = new(50, 120);

        public event EventHandler OrderCooked;

        private const string Filepath = "C:\\Users\\user\\source\\repos\\CookingGame\\CookingGame\\Data\\recipes\\";
        #endregion

        public Order()
        {
            Position = _orderPosition;
            State = new NotTakenState(this);
            LoadOrderFromJson();
        }

        public void Take()
        {
            State.Take();
        }

        public void AddTexture(Texture2D texture)
        {
            Texture = texture;
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
            int score = ingredients.Count(ingredient =>
                Ingredients.Contains(ingredient) && Ingredients.Count(c => c == ingredient) == 1) + 1;
            score -= ingredients.Count(ingredient => !Ingredients.Contains(ingredient));
            score -= Ingredients.Count(ingredient => !ingredients.Contains(ingredient)) * 2;
            return score;
        }

        public void LoadOrderFromJson()
        {
            var filePath = Filepath + GetRandomOrder() + ".json";
            var jsonString = File.ReadAllText(filePath);
            var orderData = JsonSerializer.Deserialize<OrderData>(jsonString);
            OrderName = orderData?.OrderName;
            OrderText = orderData?.Dialogue;
            Ingredients = orderData?.Ingredients.Select(s => (Ingredient)System.Enum.Parse(typeof(Ingredient), s)).ToList();
            TransladedIngredients = orderData?.Translation.ToList();
        }

        public static string GetRandomOrder()
        {
            var values = System.Enum.GetValues(typeof(Recipe));
            var random = new Random();
            var randomEnumValue = (Recipe)values.GetValue(random.Next(values.Length))!;
            return randomEnumValue.ToString();
        }
    }

    public class OrderData
    {
        public string OrderName { get; set; }
        public string Dialogue { get; set; }
        public int Price { get; set; }
        public string[] Ingredients { get; set; }
        public string[] Translation { get; set; }
    }
}



