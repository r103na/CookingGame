using System;
using System.Collections.Generic;
using System.Diagnostics;
using CookingGame.Enum;
using CookingGame.Managers;
using CookingGame.Objects;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.States
{
    public class GameplayState : BaseState
    {
        private readonly Queue<Customer> customerList = new Queue<Customer>();
        private Customer currentCustomer;
        private Shawarma currentShawarma = new Shawarma();

        private int score = 0;
        private int day = 1;
        private int timeMin = 0;

        private GameTime gameTime;

        // Generate new day

        public override void LoadContent()
        {
            gameTime = new GameTime();
            
            AddGameObject(new SplashImage(LoadTexture("GameplayState")));
            customerList.Enqueue(new Customer(LoadTexture("Character")));
            currentCustomer = customerList.Dequeue();
            
            AddGameObject(currentCustomer);
            var station = new Station(LoadTexture("background"));
            //AddGameObject(new Station(LoadTexture("background")));
            
            //foreach (var stationItem in station.stationItems)
            //{
            //    AddGameObject(stationItem);
            //}

            var a = new StationItem(Ingredient.Cabbage, LoadTexture("CabbageStationItem"));
            
            AddGameObject(a);
            
            // subscribe event
            currentCustomer.Order.OnOrderCooked += IncreaseScore;
            currentCustomer.Order.OnOrderCooked += RemoveCurrentCustomer;
            currentCustomer.Order.OnOrderCooked += RemoveCurrentShawarma;

            currentCustomer.OnCustomerPatienceRunOut += RemoveCurrentCustomer;

            a.Clicked += DoSomething;
        }

        public override void Update()
        {
            if (gameTime.TotalGameTime.TotalSeconds % 15 == 0)
            {
                timeMin++;
                if (timeMin >= 60)
                {
                    day++;
                    timeMin = 0;
                }
            }
            
            if (currentCustomer.Patience > 0)
            {
                currentCustomer.Patience -= 0.3f;
                
                if (currentCustomer.Patience <= 0)
                {
                    currentCustomer.OnPatienceRunOut();
                }
            }
        }

        public void GradeOrder()
        {
            // Check for patience
            // Check how burnt the shawarma was
            // Check all the ingredients
        }
        
        public override void HandleInput()
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var clickPosition = new Point(mouseState.X, mouseState.Y);
                foreach (var gameObject in GameObjects)
                {
                    if (gameObject is ClickableSprite o)
                    {
                        o.HandleClick(clickPosition);
                        //o.OnClicked();
                    }
                }
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                currentCustomer.Order.Cook();
            }
            
            if (keyboardState.IsKeyDown(Keys.Enter))
            {
                NotifyEvent(Events.GAME_QUIT);
            }
        }

        private void IncreaseScore(object sender, EventArgs e)
        {
            score += currentCustomer.Order.Price;
        }

        private void RemoveCurrentCustomer(object sender, EventArgs e)
        {
            RemoveGameObject(currentCustomer);
            currentCustomer.Order.OnOrderCooked -= RemoveCurrentCustomer;
        }

        private void RemoveCurrentShawarma(object sender, EventArgs e)
        {
            currentShawarma = new Shawarma();
            currentCustomer.Order.OnOrderCooked -= RemoveCurrentShawarma;
        }

        private void DoSomething(object sender, EventArgs e)
        {
            AddGameObject(new Station(LoadTexture("background")));
        }
    }
}
