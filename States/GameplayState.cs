using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CookingGame.Enum;
using CookingGame.Managers;
using CookingGame.Objects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.States
{
    public class GameplayState : BaseState
    {
        private readonly Queue<Customer> _customerList = new Queue<Customer>();
        private Customer _currentCustomer;
        private Shawarma _currentShawarma = new Shawarma();
        private Order _currentOrder = null;

        private int score = 0;
        private int timeMin = 0;
        
        private const float PatienceDecreaseRate = 0.25f;

        private GameTime gameTime;
        private float elapsedTime = 0;

        public override void LoadContent()
        {
            gameTime = new GameTime();
            Trace.Listeners.Add(new ConsoleTraceListener());


            AddGameObject(new SplashImage(LoadTexture("GameplayState")));

            AddVisitorStation();
            AddOrderStation();
            AddCookingStation();

            AddCustomer();

            AddGUI();
        }

        public override void Update()
        {
            elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            UpdateTime();
            DecreasePatience();
        }

        public override void HandleInput()
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            var clickableSprites = GameObjects.OfType<ClickableSprite>();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var clickPosition = new Point(mouseState.X, mouseState.Y);
                clickableSprites.ToList().ForEach(x => x.HandleClick(clickPosition));
            }

            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                NotifyEvent(Events.GAME_QUIT);
            }
        }

        private void IncreaseScore(object sender, EventArgs e)
        {
            score += _currentCustomer.Order.Price;
        }

        private void DecreaseScore(object sender, EventArgs e)
        {
            score -= _currentCustomer.Order.Price;
        }

        public void GradeOrder()
        {
            var grade = 0f;
            grade += _currentCustomer.Patience;
            grade -= _currentShawarma.burntMeter;
            // Check all the ingredients
            if (grade >= 80)
            {
                // Perfect
            }
        }
        private void CookShawarma(object sender, EventArgs e)
        {
            _currentCustomer.Order.Cook();
        }
        private void RemoveCurrentCustomer(object sender, EventArgs e)
        {
            _currentCustomer.Order.OnOrderCooked -= IncreaseScore;
            _currentCustomer.Order.OnOrderCooked -= RemoveCurrentCustomer;
            _currentCustomer.Order.OnOrderCooked -= RemoveCurrentShawarma;
            _currentCustomer.OnCustomerPatienceRunOut -= RemoveCurrentCustomer;
            _currentCustomer.OnCustomerPatienceRunOut -= DecreaseScore;
            _currentCustomer.Clicked -= AddOrder;

            RemoveGameObject(_currentCustomer);
            RemoveGameObject(_currentOrder);

            _currentOrder = null;

            if (_customerList.Count > 0)
                _customerList.Dequeue();

            elapsedTime = 0f;
            WaitForOtherCustomer();
        }

        private void RemoveCurrentShawarma(object sender, EventArgs e)
        {
            _currentShawarma = new Shawarma();
            _currentCustomer.Order.OnOrderCooked -= RemoveCurrentShawarma;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            AddGameObject(new Station(LoadTexture("background")));
        }

        #region ADD OBJECTS
        private void AddCustomer()
        {
            _currentCustomer = new Customer(LoadTexture("Character"));
            _customerList.Enqueue(_currentCustomer);
            _currentCustomer.Order.OnOrderCooked += IncreaseScore;
            _currentCustomer.Order.OnOrderCooked += RemoveCurrentCustomer;
            _currentCustomer.Order.OnOrderCooked += RemoveCurrentShawarma;

            _currentCustomer.OnCustomerPatienceRunOut += RemoveCurrentCustomer;
            _currentCustomer.OnCustomerPatienceRunOut += DecreaseScore;

            _currentCustomer.Clicked += AddOrder;

            AddGameObject(_currentCustomer);
        }

        public void AddOrder(object sender, EventArgs e)
        {
            if (_currentCustomer.Order.State == OrderState.NotTaken || _currentOrder != null) return;
            _currentCustomer.Order.AddTexture(LoadTexture("orderStation_order"));
            _currentOrder = _currentCustomer.Order;
            AddGameObject(_currentOrder);
        }

        private void AddOrderStation()
        {
            AddGameObject(new SplashImage(LoadTexture("orderStation_bg")));
        }

        private void AddVisitorStation()
        {

        }

        private void AddCookingStation()
        {
            var a = new StationItem(Ingredient.Cabbage, LoadTexture("CabbageStationItem"));
            AddGameObject(a);
        }

        private void AddGUI()
        {
            var cookBtn = new Button(LoadTexture("cookButton"),
                new Vector2(1200, 650));
            var menuBtn = new Button(LoadTexture("menu_btn"),
                new Vector2(20, 20));

            cookBtn.Clicked += CookShawarma;
            menuBtn.Clicked += SwitchToMenu;

            AddGameObject(cookBtn);
            AddGameObject(menuBtn);
        }

        private void AddText()
        {
            
        }

        #endregion
        private void UpdateTime()
        {
            if (gameTime.TotalGameTime.TotalSeconds % 15 == 0)
            {
                timeMin++;
            }
        }

        private void DecreasePatience()
        {
            _currentCustomer.Patience -= PatienceDecreaseRate;

            if (_currentCustomer.Patience <= 0)
            {
                _currentCustomer.OnPatienceRunOut();
            }
        }

        private void WaitForOtherCustomer()
        {
            float seconds = 5f;

            if (_customerList.Count == 0 )
            {
                if (elapsedTime <= seconds)
                {
                    AddCustomer();
                    elapsedTime = 0;
                }
                else
                {
                    WaitForOtherCustomer();
                }
            }
        }

        private void SwitchToMenu(object sender, EventArgs e)
        {
            SwitchState(new MenuState());
        }
    }
}
