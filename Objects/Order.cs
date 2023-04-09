using System;
using System.Collections.Generic;

using CookingGame.Enum;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.Objects
{
    public class Order : BaseSprite
    {
        private List<Ingredient> ingredients;
        public OrderState State;
        public int Price = 10;
        public string OrderText = "Flat bread, please";

        private Vector2 orderPosition = new Vector2(50, 120);

        public event EventHandler OrderCooked;

        public Order()
        {
            _position = orderPosition;
            State = new NotTakenState(this);
        }

        public Order(Texture2D texture)
        {
            _texture = texture;
            _position = orderPosition;
            State = new NotTakenState(this);
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
    }

    public abstract class OrderState
    {
        protected Order Order;

        protected OrderState(Order order)
        {
            Order = order;
        }

        public abstract void Take();
        public abstract void Cook();
    }

    public class NotTakenState : OrderState
    {
        public NotTakenState(Order order) : base(order) { }

        public override void Take()
        {
            Order.State = new TakenState(Order);
        }

        public override void Cook()
        {
            // Нельзя готовить заказ, который еще не взят
        }
    }

    public class TakenState : OrderState
    {
        public TakenState(Order order) : base(order) { }

        public override void Take()
        {
            // Заказ уже взят
        }

        public override void Cook()
        {
            Order.State = new DoneState(Order);
            Order.OnOrderCooked();
        }
    }

    public class DoneState : OrderState
    {
        public DoneState(Order order) : base(order) { }

        public override void Take()
        {
            // Нельзя взять уже готовый заказ
        }

        public override void Cook()
        {
            // Заказ уже готов
        }
    }
}



