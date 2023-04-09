using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CookingGame.Enum;
using CookingGame.Managers;
using CookingGame.Objects;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CookingGame.States
{
    public class GameplayState : BaseState
    {
        private readonly Queue<Customer> _customerList = new Queue<Customer>();
        private Customer _currentCustomer;
        private Shawarma _currentShawarma = new Shawarma();
        private Order _currentOrder = null;

        private Text _scoreText;
        private int _score = 0;

        private Text _orderText;
        
        private const float PatienceDecreaseRate = 0.2f;

        private GameTime gameTime;
        private int _waitTime = 0;
        private int _customerWaitTime = 70;

        private SplashImage _dialogueBox;

        private InputManager _inputManager;

        public override void LoadContent()
        {
            gameTime = new GameTime();
            _inputManager = new InputManager(GameObjects.OfType<ClickableSprite>().ToList());
            Trace.Listeners.Add(new ConsoleTraceListener());

            var font = _contentManager.Load<SpriteFont>("MyFont");
            _scoreText = new Text(font, $"{_score}", new Vector2(10, 690));
            _orderText = new Text(font, "", new Vector2(340, 95));

            _dialogueBox = new SplashImage(LoadTexture("dialogue_box"), new Vector2(320, 80));

            AddGameObject(new SplashImage(LoadTexture("GameplayState")));
            AddText(_scoreText);
            AddText(_orderText);

            AddVisitorStation();
            AddOrderStation();
            AddCookingStation();


            AddGUI();
            AddExtra();
        }

        public override void Update()
        {
            if (_customerList.Count == 0)
            {
                _waitTime++;
                if (_waitTime >= _customerWaitTime)
                {
                    AddCustomer();
                    _waitTime = 0;
                }
            }

            if (_score >= 50)
            {
                SwitchState(new SplashState());
            }

            DecreasePatience();
        }

        #region  INPUT
        public override void HandleInput()
        {
            _inputManager.UpdateGameObjects(GameObjects.OfType<ClickableSprite>().ToList());
            _inputManager.UpdateStates();
            _inputManager.HandleLeftClick();
            //if (keyboardState.IsKeyDown(Keys.Enter))
            //{
               // NotifyEvent(Events.GAME_QUIT);
            //}
        }
        #endregion

        private void IncreaseScore(object sender, EventArgs e)
        {
            _score += _currentCustomer.Order.Price;
            ChangeScoreText();
        }

        private void DecreaseScore(object sender, EventArgs e)
        {
            _score -= _currentCustomer.Order.Price;
            ChangeScoreText();
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
            _currentCustomer?.Order.Cook();
        }

        private void RemoveCurrentCustomer(object sender, EventArgs e)
        {
            _currentCustomer.Order.OrderCooked -= IncreaseScore;
            _currentCustomer.Order.OrderCooked -= RemoveCurrentCustomer;
            _currentCustomer.Order.OrderCooked -= RemoveCurrentShawarma;
            _currentCustomer.OnCustomerPatienceRunOut -= RemoveCurrentCustomer;
            _currentCustomer.OnCustomerPatienceRunOut -= DecreaseScore;
            _currentCustomer.Clicked -= AddOrder;
            _currentCustomer.Clicked -= ClearOrderText;

            RemoveGameObject(_currentCustomer);
            RemoveGameObject(_currentOrder);
            RemoveGameObject(_dialogueBox);

            _currentOrder = null;

            ChangeText(ref _orderText, "");

            if (_customerList.Count > 0)
                _customerList.Dequeue();
        }

        private void RemoveCurrentShawarma(object sender, EventArgs e)
        {
            _currentShawarma = new Shawarma();
            _currentCustomer.Order.OrderCooked -= RemoveCurrentShawarma;
        }

        #region ADD OBJECTS
        private void AddCustomer()
        {
            _currentCustomer = new Customer(LoadTexture("Characters/Tonya"));
            _customerList.Enqueue(_currentCustomer);
            _currentCustomer.Order.OrderCooked += IncreaseScore;
            _currentCustomer.Order.OrderCooked += RemoveCurrentCustomer;
            _currentCustomer.Order.OrderCooked += RemoveCurrentShawarma;
            _currentCustomer.Order.OrderCooked += ChangeWaitTime;

            _currentCustomer.OnCustomerPatienceRunOut += RemoveCurrentCustomer;
            _currentCustomer.OnCustomerPatienceRunOut += DecreaseScore;
            _currentCustomer.OnCustomerPatienceRunOut += RemoveDialogueBox;
            _currentCustomer.OnCustomerPatienceRunOut += ChangeWaitTime;

            _currentCustomer.Clicked += AddOrder;
            _currentCustomer.Clicked += ClearOrderText;
            _currentCustomer.Clicked += RemoveDialogueBox;

            ChangeText(ref _orderText, _currentCustomer.Order.OrderText);
            AddGameObject(_currentCustomer);
            AddDialogueBox();
        }

        public void AddOrder(object sender, EventArgs e)
        {
            if (_currentOrder != null) return;
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
            var table = new SplashImage(LoadTexture("items/table"), new Vector2(260, 580));
            AddGameObject(table);
        }

        private void AddCookingStation()
        {
            var cookingTable = new SplashImage(LoadTexture("cookingtable"), new Vector2(724, 360));
            var sauce = new MovableSprite(LoadTexture("items/sauce"), new Vector2(755, 400));

            AddGameObject(cookingTable);
            AddGameObject(sauce);

            AddStationItems();
        }

        private void AddStationItems()
        {
            var cabbageStation = new SplashImage(LoadTexture("items/stationitem_cucumber"), new Vector2(755, 70));
            var cheeseStation = new SplashImage(LoadTexture("items/stationitem_cheese"), new Vector2(755 + 110, 70));
            var potatoStation = new SplashImage(LoadTexture("items/stationitem_potato"), new Vector2(755 + 220, 70));
            var station = new SplashImage(LoadTexture("items/stationitem_cucumber"), new Vector2(755 + 330, 70));

            var tomatoStation = new SplashImage(LoadTexture("items/stationitem_tomato"), new Vector2(755, 194));
            var onionStation = new SplashImage(LoadTexture("items/stationitem_onion"), new Vector2(755 + 110, 194));
            var carrotStation = new SplashImage(LoadTexture("items/stationitem_carrot"), new Vector2(755 + 220, 194));
            var cucumberStation = new SplashImage(LoadTexture("items/stationitem_cucumber"), new Vector2(755 + 330, 194));

            AddGameObject(cabbageStation);
            AddGameObject(cheeseStation);
            AddGameObject(potatoStation);
            AddGameObject(station);

            AddGameObject(tomatoStation);
            AddGameObject(onionStation);
            AddGameObject(carrotStation);
            AddGameObject(cucumberStation);
        }

        private void AddGUI()
        {
            var cookBtn = new Button(LoadTexture("cookButton"),
                new Vector2(1134, 660));
            var menuBtn = new Button(LoadTexture("menu_btn"),
                new Vector2(20, 20));

            cookBtn.Clicked += CookShawarma;
            menuBtn.Clicked += SwitchToMenu;

            AddGameObject(cookBtn);
            AddGameObject(menuBtn);
        }

        private void AddExtra()
        {
            var div1 = new SplashImage(LoadTexture("divider"), new Vector2(720, 0));
            var div2 = new SplashImage(LoadTexture("divider"), new Vector2(272, 0));
            AddGameObject(div1);
            AddGameObject(div2);
        }

        private void AddDialogueBox()
        {
            AddGameObject(_dialogueBox);
        }

        private void RemoveDialogueBox(object sender, EventArgs e)
        {
            RemoveGameObject(_dialogueBox);
        }

        private void ClearOrderText(object sender, EventArgs e)
        {
            ChangeText(ref _orderText, "");
        }

        private void ChangeText(ref Text text, string newText)
        {
            RemoveText(text);
            text = new Text(text.Font, newText, text.Position);
            AddText(text);
        }

        private void ChangeScoreText()
        {
            RemoveText(_scoreText);
            _scoreText = new Text(_scoreText.Font, $"{_score}", _scoreText.Position);
            AddText(_scoreText);
        }

        #endregion

        private void DecreasePatience()
        {
            if (_currentCustomer == null) return;
            _currentCustomer.Patience -= PatienceDecreaseRate;

            if (_currentCustomer.Patience <= 30f)
            {
                _currentCustomer.ChangeTexture(LoadTexture("Characters/Tonyamad"));
            }

            if (_currentCustomer.Patience <= 0)
            {
                _currentCustomer.OnPatienceRunOut();
            }
        }

        private void ChangeWaitTime(object sender, EventArgs e)
        {
            _customerWaitTime = GetRandomWaitTime();
        }

        private int GetRandomWaitTime()
        {
            Random random = new Random();
            int randomNumber = random.Next(75, 380);
            return randomNumber;
        }
        private void SwitchToMenu(object sender, EventArgs e)
        {
            SwitchState(new MenuState());
        }
    }
}
