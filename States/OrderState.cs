using CookingGame.Objects;

namespace CookingGame.States
{
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
