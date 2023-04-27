using System;

using CookingGame.Objects.Base;
using CookingGame.States;

using Microsoft.Xna.Framework.Graphics;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace CookingGame.Objects;

public class Customer : ClickableSprite
{
    #region VARIABLES
    private readonly Vector2 _customerPosition = new(325, 185);
    public Order Order { get; }

    public string Name { get; set; }

    public float MaxPatience = 300f;
    public float Patience = 300f;

    private const float PatienceDecreaseRate = 0.4f;
    private Texture2D madTexture2D;
    private const float PatienceDecreaseRateOrderCooking = 0.15f;

    public event EventHandler OnCustomerPatienceRunOut;
    #endregion

    #region CONSTRUCTOR
    public Customer(Texture2D texture, Texture2D madTexture, string name)
    {
        Texture = texture;
        madTexture2D = madTexture;
        Position = _customerPosition;
        Order = new Order();
        Name = name;
        Clicked += IncreasePatience;
        Clicked += (_, _) =>
        {
            ChangeToNormal();
        };
    }
    #endregion

    #region PATIENCE
    public void DecreasePatience()
    {
        Patience -= Order.State is NotTakenState ? PatienceDecreaseRate : PatienceDecreaseRateOrderCooking;
    }

    public void IncreasePatience(object sender, EventArgs e)
    {
        if (Order.State is TakenState) return;
        if (Patience + 120f > MaxPatience)
        {
            Patience = MaxPatience;
        }
        else Patience += 120;
    }

    public void OnPatienceRunOut()
    {
        OnCustomerPatienceRunOut?.Invoke(this, EventArgs.Empty);
    }
    #endregion

    public void ChangeToMad()
    {
        ChangeTexture(madTexture2D);
    }

    public void ChangeToNormal()
    {
        ChangeTexture(Texture);
    }
}