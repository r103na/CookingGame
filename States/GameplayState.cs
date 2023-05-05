using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using CookingGame.Enum;
using CookingGame.Managers;
using CookingGame.Objects;
using CookingGame.Objects.Base;
using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.States;

public class GameplayState : BaseState
{
    #region  VARIABLES
    private Customer _currentCustomer;
    private Shawarma _currentShawarma;
    private Order _currentOrder;

    private Text _scoreText;
    private Text _orderCountText;

    private Text _orderText;
    private Text _orderNameText;

    private float _waitTime;
    private float waitToGive;
    private bool waitingToGive;
    private bool isGrilling;
    private float grillTime;

    private int _customerWaitTime = 1;

    private SplashImage _dialogueBox;

    private ScoreManager _scoreManager;

    private Tip _tip;
    private PatienceBar _patienceBar;
    private BaseSprite _exclamation;

    private float _elapsed;
    private float _cameraLerp;
    private bool _cameraMovingRight;
    private bool shawarmaMoving;
    private bool _cameraMovingLeft;

    private SpriteFont font;
    public EventHandler Updated;
    #endregion

    public override void LoadContent()
    {
        _scoreManager = new ScoreManager();
        _scoreManager.ScoreIncreased += ChangeScoreText;
        _scoreManager.ScoreDecreased += ChangeScoreText;

        font = ContentManager.Load<SpriteFont>("Fonts/MyFont");
        _scoreText = new Text(font, $"Деньги: {_scoreManager.Score}", new Vector2(45, 550));
        _orderCountText = new Text(font, $"Заказы: {_scoreManager.OrderCount}", new Vector2(45, 575));
        _orderText = new Text(font, "", new Vector2(340, 105));
        _orderNameText = new Text(font, "", new Vector2(65, 135));

        _tip = new Tip(LoadTexture("gui/tip"), InputManager.MousePosition);
        _patienceBar = new PatienceBar(LoadTexture("gui/patiencebar1"));

        _dialogueBox = new SplashImage(LoadTexture("gui/dialogue_box"), new Vector2(320, 80));

        AddGameObject(new SplashImage(LoadTexture("backgrounds/GameplayState")));
        AddText(_scoreText);
        AddText(_orderCountText);
        AddText(_orderText);

        AddVisitorStation();
        AddOrderStation();
        AddCookingStation();
        AddGrillStation();

        AddGUI();
        AddExtra();

        AddIngredientItems();
    }

    public override void Update(GameTime gameTime)
    {
        Gametime = gameTime;
        _elapsed = (float)Gametime.ElapsedGameTime.TotalSeconds;

        OnUpdated(null, EventArgs.Empty);
        WaitForCustomer();

        if (waitingToGive)
        {
            RemoveDialogueBox();
            ChangeText(ref _orderText, "");
            AddScoreToOrder();
            waitToGive += _elapsed;
            if (waitToGive is >= 0.8f and <= 2.25f)
            {
                AddDialogueBox(null, EventArgs.Empty);
                if (_currentOrder.Score >= 10 && _currentOrder != null)
                {
                    ChangeText(ref _orderText, "Спасибо!");
                }
                else
                {
                    _currentCustomer?.ChangeToMad();
                    ChangeText(ref _orderText, "Что это?!");
                }
            }
            if (waitToGive >= 2.25f)
            {
                CookShawarma(null, EventArgs.Empty);
                waitingToGive = false;
                waitToGive = 0;
            }
        }

        if (isGrilling)
        {
            grillTime += _elapsed;
            if (grillTime >= 2f)
            {
                isGrilling = false;
                grillTime = 0;
            }
        }

        foreach (var item in GameObjects.OfType<IngredientItem>())
        {
            item.CheckIfInBounds();
        }

        _tip.UpdatePosition(InputManager.MousePosition);

        DecreasePatience();
        ChangePatienceBar();
    }

    #region SCORE

    private void AddScoreToOrder()
    {
        if (_currentOrder == null) return;
        var shawarmaIngredientList = _currentShawarma.IngredientList
            .Select(x => x.Ingredient)
            .ToList();
        _currentOrder.Score = _currentOrder.CheckIngredients(shawarmaIngredientList) * 10;
    }

    private void IncreaseScore(object sender, EventArgs e)
    {
        _scoreManager.IncreaseScore(_currentOrder.Score);
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

    #region COOKING 
    private void FinishCooking(object sender, EventArgs e)
    {
        ClearOrderNameText();
        RemoveCurrentCustomer();
        ChangeWaitTime();
        RemoveDialogueBox();
        RemovePatienceBar();
        RemoveExclamationMark(sender, e);
    }

    public void TakeOrder(object sender, EventArgs e)
    {
        _scoreManager.OrderCount++;
        _currentCustomer?.Order.Take();
    }

    private void CookShawarma(object sender, EventArgs e)
    {
        _currentCustomer?.Order.Cook();
        RemoveCurrentShawarma(sender, e);
    }
    #endregion

    #region ADD OBJECTS
    private void AddShawarma(string textureName)
    {
        if (_currentShawarma != null) return;
        shawarmaMoving = false;
        _currentShawarma = new Shawarma(LoadTexture("items/" + textureName));
        AddGameObject(_currentShawarma);
    }

    private void AddCustomer()
    {
        var name = GetRandomCharacterName();

        _currentCustomer = new Customer(LoadTexture("Characters/" + name), LoadTexture("Characters/" + name + "mad"), name);

        _currentCustomer.Order.OrderCooked += IncreaseScore;
        _currentCustomer.Order.OrderCooked += FinishCooking;

        _currentCustomer.OnCustomerPatienceRunOut += DecreaseScore;
        _currentCustomer.OnCustomerPatienceRunOut += FinishCooking;
        _currentCustomer.OnCustomerPatienceRunOut += RemoveExclamationMark;

        _currentCustomer.Clicked += AddOrder;
        _currentCustomer.Clicked += TakeOrder;
        _currentCustomer.Clicked += ClearOrderText;
        _currentCustomer.Clicked += AddOrderIngredientText;

        _currentCustomer.Hovered += AddTip;
        _currentCustomer.Clicked += RemoveTip;
        _currentCustomer.Clicked += RemoveExclamationMark;
        _currentCustomer.Clicked += AddDialogueBox;
        _currentCustomer.Unhovered += RemoveTip;

        AddGameObject(_currentCustomer);
        AddExclamationMark();
        AddPatienceBar();
    }

    public void AddPatienceBar()
    {
        AddGameObject(_patienceBar);
    }

    public void AddExclamationMark()
    {
        _exclamation = new ImageObject(LoadTexture("gui/exclamation"), _currentCustomer.Position - new Vector2(-110, 100));
        AddGameObject(_exclamation);
    }

    #region ADD STATIONS
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
        var sauce = new Sauce(LoadTexture("items/sauce"), new Vector2(755, 400));

        AddGameObject(cookingTable);

        var sauceItem = new IngredientItem(LoadTexture("items/sauceitem"), Vector2.Zero, Ingredient.Sauce);
        sauce.Clicked += (_, _) =>
        {
            sauceItem = new IngredientItem(
                LoadTexture("items/sauceitem"),
                Vector2.One,
                sauceItem.Ingredient);
        };
        sauce.Released += (_, _) =>
        {
            if (_currentShawarma == null) return;
            var tomatPos = new Point((int)sauce.Position.X, (int)sauce.Position.Y + 120);
            if (_currentShawarma.IsInBounds(tomatPos))
            {
                sauceItem.CanClick = false;
                sauceItem.Position = _currentShawarma.Position + new Vector2(20, 40);
                AddGameObject(sauceItem);
                _currentShawarma.IngredientList.Add(sauceItem);
                sauce.ResetPosition();
                return;
            }
            sauce.ResetPosition();
            RemoveGameObject(sauceItem);
        };
        AddStationItems();
        AddGameObject(sauce);
    }

    private void AddStationItems()
    {
        var table = new SplashImage(LoadTexture("items/station"), new Vector2(730, 0));

        var cabbageStation = new StationItem(Ingredient.Cabbage, LoadTexture("items/stationitem_cabbage"), new Vector2(755, 70));
        var cheeseStation = new StationItem(Ingredient.Cheese, LoadTexture("items/stationitem_cheese"), new Vector2(755 + 104, 70));
        var potatoStation = new StationItem(Ingredient.Potato, LoadTexture("items/stationitem_potato"), new Vector2(755 + 208, 70));
        var station = new StationItem(Ingredient.Chicken, LoadTexture("items/stationitem_chicken"), new Vector2(755 + 312, 70));
        var station2 = new StationItem(Ingredient.Pepper, LoadTexture("items/stationitem_pepper"), new Vector2(755 + 416, 70));

        var tomatoStation = new StationItem(Ingredient.Tomato, LoadTexture("items/stationitem_tomato"), new Vector2(755, 194));
        var onionStation = new StationItem(Ingredient.Onion, LoadTexture("items/stationitem_onion"), new Vector2(755 + 104, 194));
        var carrotStation = new StationItem(Ingredient.Carrot, LoadTexture("items/stationitem_carrot"), new Vector2(755 + 208, 194));
        var cucumberStation = new StationItem(Ingredient.Cucumber, LoadTexture("items/stationitem_cucumber"), new Vector2(755 + 312, 194));
        var station3 = new StationItem(Ingredient.Cabbage, LoadTexture("items/cabbagestationitem"), new Vector2(755 + 416, 194));

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

    private void AddGrillStation()
    {
        AddGameObject(new ImageObject(LoadTexture("backgrounds/grillstation_bg"), new Vector2(1280, 0)));
    }
    #endregion

    #region ADD GUI

    private void AddStats()
    {
        AddGameObject(new ImageObject(LoadTexture("gui/stats"), new Vector2(30, 500)));
        AddText(new Text(font, "Статистика", new Vector2(45, 515)));
    }

    public void AddOrder(object sender, EventArgs e)
    {
        if (_currentOrder != null) return;
        _currentCustomer.Order.AddTexture(LoadTexture("items/orderStation_order"));
        _currentOrder = _currentCustomer.Order;
        AddGameObject(_currentOrder);
        ChangeText(ref _orderNameText, _currentCustomer.Order.OrderName);
    }

    private void AddTip(object sender, EventArgs e)
    {
        if (GameObjects.OfType<Tip>().Any() || _tip.TipUsed) return;
        AddGameObject(_tip);
        _tip.TipUsed = true;
    }

    private void AddShawarmaButton()
    {
        var sb1 = new Button(LoadTexture("gui/flatbreadIcon"), new Vector2(900, 673));
        sb1.Clicked += (_, _) => AddShawarma("flatbread");
        var sb2 = new Button(LoadTexture("gui/flatbreadIcon_garlic"), new Vector2(900 + 60, 673));
        sb2.Clicked += (_, _) => AddShawarma("flatbread3");
        var sb3 = new Button(LoadTexture("gui/flatbreadIcon_cheesy"), new Vector2(900 + 120, 673));
        sb3.Clicked += (_, _) => AddShawarma("flatbread2");
        var sb4 = new Button(LoadTexture("gui/flatbreadIcon_red"), new Vector2(900 + 180, 673));
        sb4.Clicked += (_, _) => AddShawarma("flatbread4");
        AddGameObject(sb1);
        AddGameObject(sb2);
        AddGameObject(sb3);
        AddGameObject(sb4);
    }

    private void AddGUI()
    {
        var cookBtn = new Button(LoadTexture("gui/cookButton"),
            new Vector2(1200, 660));
        var menuBtn = new Button(LoadTexture("gui/menu_btn"),
            new Vector2(20, 20));
        var discardButton = new Button(LoadTexture("gui/discardButton"), new Vector2(750, 660));
        var finishBtn = new Button(LoadTexture("gui/cookButton"),
            new Vector2(1900, 660));

        finishBtn.Clicked += (_, _) =>
        {
            waitingToGive = true;
        };

        cookBtn.Clicked += (_, _) =>
        {
            Updated += MoveCameraToGrill;
            Updated += MoveCurrentShawarma;
            Updated -= MoveCameraToVisitor;
        };

        menuBtn.Clicked += SwitchToMenu;

        discardButton.Clicked += RemoveCurrentShawarma;
        finishBtn.Clicked += (_, _) => {
            Updated += MoveCameraToVisitor;
            Updated -= MoveCameraToGrill;
        };
        discardButton.Clicked += (_, _) => {
            Updated += MoveCameraToVisitor;
            Updated -= MoveCameraToGrill;
        };

        AddGameObject(cookBtn);
        AddGameObject(finishBtn);
        AddGameObject(menuBtn);
        AddGameObject(discardButton);
        AddShawarmaButton();
        AddStats();
    }

    private void AddExtra()
    {
        var div1 = new SplashImage(LoadTexture("gui/divider"), new Vector2(720, 0));
        var div2 = new SplashImage(LoadTexture("gui/divider"), new Vector2(267, 0));
        var div3 = new SplashImage(LoadTexture("gui/divider"), new Vector2(1275, 0));
        AddGameObject(div1);
        AddGameObject(div2);
        AddGameObject(div3);
    }

    private void AddDialogueBox(object sender, EventArgs e)
    {
        AddGameObject(_dialogueBox);
        ChangeText(ref _orderText, _currentCustomer.Order.OrderText);

    }
    #endregion

    private void AddIngredientItems()
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
                    if (_currentShawarma == null)
                    {
                        RemoveGameObject(tomato);
                        return;
                    }
                    var tomatPos = new Point((int)tomato.Position.X, (int)tomato.Position.Y);
                    if (_currentShawarma.IsInBounds(tomatPos))
                    {
                        tomato.CanClick = false;
                        _currentShawarma.IngredientList.Add(tomato);
                        return;
                    }
                    RemoveGameObject(tomato);
                };
                AddGameObject(tomato);
            };
        }
    }
    #endregion

    #region REMOVE OBJECTS
    private void RemoveCurrentCustomer()
    {
        _currentCustomer.Order.OrderCooked -= IncreaseScore;
        _currentCustomer.OnCustomerPatienceRunOut -= DecreaseScore;

        _currentCustomer.Clicked -= AddOrder;
        _currentCustomer.Clicked -= TakeOrder;
        _currentCustomer.Clicked -= ClearOrderText;
        _currentCustomer.Clicked -= AddOrderIngredientText;

        RemoveGameObject(_currentCustomer);
        RemoveGameObject(_currentOrder);
        RemoveGameObject(_dialogueBox);

        _currentOrder = null;
        _currentCustomer = null;

        ChangeText(ref _orderText, "");
    }

    public void RemovePatienceBar()
    {
        RemoveGameObject(_patienceBar);
        _patienceBar.ChangeTexture(LoadTexture("gui/patiencebar1"));
    }

    public void RemoveExclamationMark(object sender, EventArgs e)
    {
        RemoveGameObject(_exclamation);
    }

    private void RemoveTip(object sender, EventArgs e)
    {
        RemoveGameObject(_tip);
    }

    private void RemoveCurrentShawarma(object sender, EventArgs e)
    {
        if (_currentShawarma == null) return;
        ClearShawarmaIngredients();
        RemoveGameObject(_currentShawarma);
        _currentShawarma = null;
    }

    private void ClearShawarmaIngredients()
    {
        if (_currentShawarma == null) return;
        foreach (var ingredientItem in _currentShawarma.IngredientList)
        {
            RemoveGameObject(ingredientItem);
        }
    }

    private void RemoveDialogueBox()
    {
        RemoveGameObject(_dialogueBox);
    }
    #endregion

    #region CAMERA MOVEMENT

    private void MoveCameraToGrill(object sender, EventArgs e)
    {
        _cameraLerp = MathHelper.Lerp(CameraManager.Position.X, -720, _elapsed * 5f);
        MoveToGrillStation();
        if (CameraManager.Position.X <= -718)
            Updated -= MoveCameraToGrill;
    }

    private void MoveCameraToVisitor(object sender, EventArgs e)
    {
        _cameraLerp = MathHelper.Lerp(CameraManager.Position.X, 0, _elapsed * 5f);
        MoveToVisitor();
        if (CameraManager.Position.X >= -2f)
            Updated -= MoveCameraToVisitor;
    }

    private void MoveToGrillStation()
    {
        CameraManager.MoveCamera(new Vector3(_cameraLerp, 0, 0));
        InputManager.ChangeOffset(new Point((int)_cameraLerp, 0));
    }

    private void MoveToVisitor()
    {
        CameraManager.MoveCamera(new Vector3(_cameraLerp, 0, 0));
        //_patienceBar.Position.X = CameraManager.Position.X + 50;
        InputManager.ChangeOffset(new Point((int)_cameraLerp, 0));
    }

    #endregion

    #region TEXT
    private void AddOrderIngredientText(object sender, EventArgs e)
    {
        List<Text> ingredientsText = new();

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

    private void ClearOrderNameText()
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
        ChangeText(ref _scoreText, $"Деньги: {_scoreManager.Score}");
        ChangeText(ref _orderCountText, $"Заказы: {_scoreManager.OrderCount}");
    }
    #endregion

    #region PATIENCE
    private void DecreasePatience()
    {
        if (_currentCustomer == null) return;
        _currentCustomer.DecreasePatience();
        // TODO this might mess with animations!
        if (_currentCustomer.Patience <= 60f)
        {
            _currentCustomer.ChangeToMad();
        }

        if (_currentCustomer.Patience <= 0)
        {
            _currentCustomer.OnPatienceRunOut();
        }
    }

    private void ChangePatienceBar()
    {
        if (_currentCustomer == null) return;

        var textureName = (6 - (int)(_currentCustomer.Patience / 50));
        if (textureName is < 6 and > 0)
            _patienceBar.ChangeTexture(LoadTexture("gui/patiencebar" + textureName));
    }

    #endregion

    #region WAIT TIME
    private void ChangeWaitTime()
    {
        _customerWaitTime = GetRandomWaitTime();
    }

    private void WaitForCustomer()
    {
        if (_currentCustomer == null)
        {
            _waitTime += _elapsed;

            if (_waitTime >= _customerWaitTime)
            {
                AddCustomer();
                _waitTime = 0;
            }
        }
    }

    private static int GetRandomWaitTime()
    {
        var random = new Random();
        var randomNumber = random.Next(1, 7);
        return randomNumber;
    }

    #endregion

    private void MoveShawarma(Vector2 position)
    {
        _currentShawarma.Position.X = position.X;
        foreach (var ingredient in _currentShawarma.IngredientList)
        {
            ingredient.AcceptableBounds = new Rectangle(0, 0, 100000, 10000);
            ingredient.Position.X = _currentShawarma.Position.X + 30;
        }
    }

    private void MoveCurrentShawarma(object sender, EventArgs e)
    {
        if (_currentShawarma != null)
        {
            MoveShawarma(new Vector2(MathHelper.Lerp(_currentShawarma.Position.X, 1590, _elapsed * 6), 0));
            if (_currentShawarma.Position.X >= 1580)
            {
                Updated -= MoveCurrentShawarma;
            }
        }
    }

    private void SwitchToMenu(object sender, EventArgs e)
    {
        SwitchState(new MenuState());
    }

    private static string GetRandomCharacterName()
    {
        var names = new[] { "Tonya", "Sonya" };
        var random = new Random();
        var randomNumber = random.Next(0, 2);
        return names[randomNumber];
    }

    private void OnUpdated(object sender, EventArgs e)
    {
        Updated?.Invoke(sender, e);
    }
}