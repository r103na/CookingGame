using System;

using CookingGame.Objects.Base;
using CookingGame.States;

using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CookingGame.Objects
{
    public class Customer : ClickableSprite
    {
        #region VARIABLES
        private readonly Vector2 _customerPosition = new(325, 185);
        public Order Order { get; }

        public string Name { get; set; }

        public float MaxPatience = 300f;
        public float Patience = 300f;

        private const float PatienceDecreaseRate = 0.5f;
        private const float PatienceDecreaseRateOrderCooking = 0.2f;

        public event EventHandler OnCustomerPatienceRunOut;
        #endregion

        #region CONSTRUCTOR
        public Customer(Texture2D texture, string name)
        {
            Texture = texture;
            Position = _customerPosition;
            Order = new Order();
            Name = name;
            Clicked += IncreasePatience;
        }
        #endregion

        #region PATIENCE
        public void DecreasePatience()
        {
            Patience -= Order.State is NotTakenState ? PatienceDecreaseRate : PatienceDecreaseRateOrderCooking;
        }

        public void IncreasePatience(object sender, EventArgs e)
        {
            if (Patience + 80f > MaxPatience)
            {
                Patience = MaxPatience;
            }
            else Patience += 80;
        }

        public void OnPatienceRunOut()
        {
            OnCustomerPatienceRunOut?.Invoke(this, EventArgs.Empty);
        }
        #endregion
    }
}
