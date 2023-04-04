using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CookingGame.Enum;

namespace CookingGame.Objects
{
    public class Order
    {
        private List<Ingredient> ingredients;
        public OrderState State = OrderState.NotTaken;
        public int Price = 10;

        public event EventHandler OnOrderCooked;
        
        public Order()
        {
            
        }
        
        public void Take()
        {
            State = OrderState.Taken;
        }

        public void Cook()
        {
            if (State != OrderState.Taken) return;
            OnOrderCooked?.Invoke(this, EventArgs.Empty);
            State = OrderState.Done;
        }
    }
}
