using System;
using System.Collections.Generic;
using System.Linq;

using CookingGame.Enum;
using CookingGame.Managers;
using CookingGame.Objects;
using CookingGame.Objects.Base;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.States
{
    public class GameplayState : BaseState
    {
        #region  VARIABLES
        private readonly Queue<Customer> _customerList = new();
        private Customer _currentCustomer;
        private Shawarma _currentShawarma;
        private Order _currentOrder;

        private Text _scoreText;

        private Text _orderText;
        private Text _orderNameText;
        
        private const float PatienceDecreaseRate = 0.2f;

        private int _waitTime;
        private int _customerWaitTime = 45;

        private SplashImage _dialogueBox;

        private ScoreManager _scoreManager;
        #endregion

        public override void LoadContent()
        {
            _scoreManager = new ScoreManager();
            _scoreManager.ScoreIncreased += ChangeScoreText;
            _scoreManager.ScoreDecreased += ChangeScoreText;
            InputManager = new InputManager();

            var font = ContentManager.Load<SpriteFont>("Fonts/MyFont");
            _scoreText = new Text(font, $"{_scoreManager.Score}", new Vector2(10, 690));
            _orderText = new Text(font, "", new Vector2(340, 95));
            _orderNameText = new Text(font, "", new Vector2(65, 135));

            _dialogueBox = new SplashImage(LoadTexture("gui/dialogue_box"), new Vector2(320, 80));

            AddGameObject(new SplashImage(LoadTexture("backgrounds/GameplayState")));
            AddText(_scoreText);
            AddText(_orderText);

            AddVisitorStation();
            AddOrderStation();
            AddCookingStation();

            AddShawarma();

            AddGUI();
            AddExtra();

            idk();
        }

        public override void Update()
        {
            if (_customerList == null || _customerList.Count == 0)
            {
                _waitTime++;
                if (_waitTime >= _customerWaitTime)
                {
                    AddCustomer();
                    _waitTime = 0;
                }
            }

            DecreasePatience();
        }

        #region  INPUT
        public override void HandleInput()
        {
            InputManager.UpdateMouseScale(TransformMatrix);
            InputManager.UpdateGameObjects(GameObjects.OfType<ClickableSprite>().ToList());
            InputManager.UpdateStates();
            InputManager.HandleInput();
        }
        #endregion

        #region SCORE
        private void IncreaseScore(object sender, EventArgs e)
        {
            var shawarmaIngredientList = _currentShawarma.IngredientList
                .Select(x => x.Ingredient)
                .ToList();
            var score = _currentCustomer.Order.CheckIngredients(shawarmaIngredientList) * 10;
            _scoreManager.IncreaseScore(score);
            if (_scoreManager.Score >= _scoreManager.MaxScore)
            {
                SwitchState(new SplashState());
            }
        }

        private void DecreaseScore(object sender, EventArgs e)
        {
            _scoreManager.DecreaseScore(10);
        }

        #endregion


        public void TakeOrder(object sender, EventArgs e)
        {
            _currentCustomer?.Order.Take();
        }

        private void CookShawarma(object sender, EventArgs e)
        {
            _currentCustomer?.Order.Cook();
        }

        private void RemoveCurrentCustomer(object sender, EventArgs e)
        {
            _currentCustomer.Order.OrderCooked -= IncreaseScore;
            _currentCustomer.Order.OrderCooked -= RemoveCurrentCustomer;
            _currentCustomer.Order.OrderCooked -= ChangeWaitTime;
            _currentCustomer.Order.OrderCooked -= ClearOrderNameText;

            _currentCustomer.OnCustomerPatienceRunOut -= RemoveCurrentCustomer;
            _currentCustomer.OnCustomerPatienceRunOut -= DecreaseScore;
            _currentCustomer.OnCustomerPatienceRunOut -= RemoveDialogueBox;
            _currentCustomer.OnCustomerPatienceRunOut -= ChangeWaitTime;
            _currentCustomer.OnCustomerPatienceRunOut -= ClearOrderNameText;

            _currentCustomer.Clicked -= AddOrder;
            _currentCustomer.Clicked -= TakeOrder;
            _currentCustomer.Clicked -= ClearOrderText;
            _currentCustomer.Clicked -= RemoveDialogueBox;
            _currentCustomer.Clicked -= AddOrderIngredientText;

            RemoveGameObject(_currentCustomer);
            RemoveGameObject(_currentOrder);
            RemoveGameObject(_dialogueBox);

            _currentOrder = null;

            ChangeText(ref _orderText, "");

            if (_customerList.Count > 0)
                _customerList.Dequeue();
        }


        #region ADD OBJECTS

        private void AddShawarma()
        {
            _currentShawarma = new Shawarma(LoadTexture("items/flatbread"));
            AddGameObject(_currentShawarma);
        }
        
        private void AddCustomer()
        {
            _currentCustomer = new Customer(LoadTexture("Characters/Tonya"));
            _customerList.Enqueue(_currentCustomer);
            _currentCustomer.Order.OrderCooked += IncreaseScore;
            _currentCustomer.Order.OrderCooked += RemoveCurrentCustomer;
            _currentCustomer.Order.OrderCooked += ChangeWaitTime;
            _currentCustomer.Order.OrderCooked += ClearOrderNameText;

            _currentCustomer.OnCustomerPatienceRunOut += RemoveCurrentCustomer;
            _currentCustomer.OnCustomerPatienceRunOut += DecreaseScore;
            _currentCustomer.OnCustomerPatienceRunOut += RemoveDialogueBox;
            _currentCustomer.OnCustomerPatienceRunOut += ChangeWaitTime;
            _currentCustomer.OnCustomerPatienceRunOut += ClearOrderNameText;

            _currentCustomer.Clicked += AddOrder;
            _currentCustomer.Clicked += TakeOrder;
            _currentCustomer.Clicked += ClearOrderText;
            _currentCustomer.Clicked += RemoveDialogueBox;
            _currentCustomer.Clicked += AddOrderIngredientText;

            ChangeText(ref _orderText, _currentCustomer.Order.OrderText);
            AddGameObject(_currentCustomer);
            AddDialogueBox();
        }

        public void AddOrder(object sender, EventArgs e)
        {
            if (_currentOrder != null) return;
            _currentCustomer.Order.AddTexture(LoadTexture("items/orderStation_order"));
            _currentOrder = _currentCustomer.Order;
            AddGameObject(_currentOrder);
            ChangeText(ref _orderNameText, _currentCustomer.Order.OrderName);
        }

        private void AddOrderStation()
        {
            AddGameObject(new SplashImage(LoadTexture("items/orderStation_bg")));
        }

        private void AddVisitorStation()
        {
            var table = new SplashImage(LoadTexture("items/table"), new Vector2(260, 580));
            AddGameObject(table);
        }

        private void AddCookingStation()
        {
            var cookingTable = new SplashImage(LoadTexture("items/cookingtable"), new Vector2(724, 360));
            var sauce = new MovableSprite(LoadTexture("items/sauce"), new Vector2(755, 400));

            AddGameObject(cookingTable);

            var sauceItem = new IngredientItem(LoadTexture("items/sauceitem"), Vector2.Zero, Ingredient.Sauce);
            sauce.Clicked += (_, _) =>
            {
                sauceItem = new IngredientItem(
                    LoadTexture("items/sauceitem"),
                    new Vector2(
                        InputManager.MouseState.X,
                        InputManager.MouseState.Y),
                    sauceItem.Ingredient);
                sauce.Released += (_, _) =>
                {
                    var tomatPos = new Point((int)sauceItem.Position.X, (int)sauceItem.Position.Y);
                    if (_currentShawarma.IsInBounds(tomatPos))
                    {
                        sauceItem.canClick = false;
                        AddGameObject(sauceItem);
                        _currentShawarma.IngredientList.Add(sauceItem);
                        return;
                    }

                    RemoveGameObject(sauceItem);
                };
            };
            AddStationItems();
            AddGameObject(sauce);
        }

        private void AddStationItems()
        {
            var table = new SplashImage(LoadTexture("items/station"), new Vector2(730, 0));

            var cabbageStation = new StationItem(Ingredient.Cabbage, LoadTexture("items/stationitem_cabbage"), new Vector2(755, 70));
            var cheeseStation = new StationItem(Ingredient.Cheese, LoadTexture("items/stationitem_cheese"), new Vector2(755 + 110, 70));
            var potatoStation = new StationItem(Ingredient.Potato, LoadTexture("items/stationitem_potato"), new Vector2(755 + 220, 70));
            var station = new StationItem(Ingredient.Chicken, LoadTexture("items/stationitem_tomato"), new Vector2(755 + 330, 70));
            var station2 = new StationItem(Ingredient.Cabbage, LoadTexture("items/cabbagestationitem"), new Vector2(755 + 440, 70));

            var tomatoStation = new StationItem(Ingredient.Tomato, LoadTexture("items/stationitem_tomato"), new Vector2(755, 194));
            var onionStation = new StationItem(Ingredient.Onion, LoadTexture("items/stationitem_onion"), new Vector2(755 + 110, 194));
            var carrotStation = new StationItem(Ingredient.Carrot, LoadTexture("items/stationitem_carrot"), new Vector2(755 + 220, 194));
            var cucumberStation = new StationItem(Ingredient.Cucumber, LoadTexture("items/stationitem_cucumber"), new Vector2(755 + 330, 194));
            var station3 = new StationItem(Ingredient.Cabbage, LoadTexture("items/cabbagestationitem"), new Vector2(755 + 440, 194));

            AddGameObject(table);

            AddGameObject(cabbageStation);
            AddGameObject(cheeseStation);
            AddGameObject(potatoStation);
            AddGameObject(station);
            AddGameObject(station2);

            AddGameObject(tomatoStation);
            AddGameObject(onionStation);
            AddGameObject(carrotStation);
            AddGameObject(cucumberStation);
            AddGameObject(station3);
        }

        private void AddGUI()
        {
            var cookBtn = new Button(LoadTexture("gui/cookButton"),
                new Vector2(1134, 660));
            var menuBtn = new Button(LoadTexture("gui/menu_btn"),
                new Vector2(20, 20));

            cookBtn.Clicked += CookShawarma;
            cookBtn.Clicked += ClearShawarmaIngredients;
            cookBtn.Clicked += RemoveCurrentShawarma;
            menuBtn.Clicked += SwitchToMenu;

            AddGameObject(cookBtn);
            AddGameObject(menuBtn);
        }

        private void AddExtra()
        {
            var div1 = new SplashImage(LoadTexture("gui/divider"), new Vector2(720, 0));
            var div2 = new SplashImage(LoadTexture("gui/divider"), new Vector2(267, 0));
            AddGameObject(div1);
            AddGameObject(div2);
        }

        private void idk()
        {
            var stationItems = GameObjects.OfType<StationItem>().ToList();
            foreach (var item in stationItems)
            {
                var tomato = new IngredientItem(LoadTexture("items/tomato"), Vector2.Zero, item.Ingredient);
                item.Clicked += (_, _) =>
                {
                    tomato = new IngredientItem(
                        LoadTexture("items/" + tomato.Ingredient),
                        new Vector2(
                            InputManager.MouseState.X,
                            InputManager.MouseState.Y),
                        tomato.Ingredient);
                    tomato.Released += (_, _) =>
                    {
                        var tomatPos = new Point((int)tomato.Position.X, (int)tomato.Position.Y);
                        if (_currentShawarma.IsInBounds(tomatPos))
                        {
                            tomato.canClick = false;
                            _currentShawarma.IngredientList.Add(tomato);
                            return;
                        }
                        RemoveGameObject(tomato);
                    };
                    AddGameObject(tomato);
                };
            }
        }

        private void RemoveCurrentShawarma(object sender, EventArgs e)
        {
            _currentShawarma = new Shawarma(LoadTexture("items/flatbread"));
        }

        private void ClearShawarmaIngredients(object sender, EventArgs e)
        {
            foreach (var ingredientItem in _currentShawarma.IngredientList)
            {
                RemoveGameObject(ingredientItem);
            }
        }

        private void AddDialogueBox()
        {
            AddGameObject(_dialogueBox);
        }

        private void RemoveDialogueBox(object sender, EventArgs e)
        {
            RemoveGameObject(_dialogueBox);
        }

        private void AddOrderIngredientText(object sender, EventArgs e)
        {
            List<Text> ingredientsText = new();

            var font = ContentManager.Load<SpriteFont>("Fonts/MyFont");

            for (var index = 0; index < _currentCustomer.Order.TransladedIngredients.Count; index++)
            {
                var ingredient = _currentCustomer.Order.TransladedIngredients[index];
                var text = new Text(font, ingredient, new Vector2(65, 175 + index * 28));
                ingredientsText.Add(text);
                AddText(text);
            }

            _currentCustomer.Order.OrderCooked += (_, _) =>
            {
                foreach (var ingredient in ingredientsText)
                {
                    RemoveText(ingredient);
                }
                ingredientsText.Clear();
            };

            _currentCustomer.OnCustomerPatienceRunOut += (_, _) =>
            {
                foreach (var ingredient in ingredientsText)
                {
                    RemoveText(ingredient);
                }
                ingredientsText.Clear();
            };
        }

        private void ClearOrderText(object sender, EventArgs e)
        {
            ChangeText(ref _orderText, "");
        }

        private void ClearOrderNameText(object sender, EventArgs e)
        {
            ChangeText(ref _orderNameText, "");
        }

        private void ChangeText(ref Text text, string newText)
        {
            RemoveText(text);
            text = new Text(text.Font, newText, text.Position);
            AddText(text);
        }

        private void ChangeScoreText(object sender, EventArgs e)
        {
            RemoveText(_scoreText);
            _scoreText = new Text(_scoreText.Font, $"{_scoreManager.Score}", _scoreText.Position);
            AddText(_scoreText);
        }

        #endregion

        private void DecreasePatience()
        {
            if (_currentCustomer == null) return;
            _currentCustomer.DecreasePatience(PatienceDecreaseRate);

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

        private static int GetRandomWaitTime()
        {
            var random = new Random();
            var randomNumber = random.Next(75, 380);
            return randomNumber;
        }
        private void SwitchToMenu(object sender, EventArgs e)
        {
            SwitchState(new MenuState());
        }
    }
}
