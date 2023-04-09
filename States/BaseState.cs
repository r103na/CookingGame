using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CookingGame.Enum;
using CookingGame.Managers;
using CookingGame.Objects;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CookingGame.States
{
    public abstract class BaseState
    {
        private protected readonly List<BaseSprite> GameObjects = new List<BaseSprite>();
        private protected readonly List<Text> Texts = new List<Text>();

        private const string FallbackTexture = "Empty";
        protected ContentManager ContentManager;
        protected InputManager InputManager;

        public void Initialize(ContentManager contentManager)
        {
            ContentManager = contentManager;
        }

        public abstract void LoadContent();
        public abstract void Update();

        public void UnloadContent()
        {
            ContentManager.Unload();
        }

        public abstract void HandleInput();

        public event EventHandler<BaseState> OnStateSwitched;

        public event EventHandler<Events> OnEventNotification;

        protected void NotifyEvent(Events eventType, object argument = null)
        {
            OnEventNotification?.Invoke(this, eventType);

            foreach (var gameObject in GameObjects) 
                gameObject.OnNotify(eventType);
        }

        protected void SwitchState(BaseState gameState)
        {
            OnStateSwitched?.Invoke(this, gameState);
        }

        protected void AddGameObject(BaseSprite gameObject)
        {
            GameObjects.Add(gameObject);
        }

        protected void RemoveGameObject(BaseSprite gameObject)
        {
            GameObjects.Remove(gameObject);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach (var gameObject in GameObjects.OrderBy(a => a.zIndex))
            {
                gameObject.Render(spriteBatch);
            }

            foreach (var text in Texts)
            {
                text.Render(spriteBatch);
            }
        }
        protected Texture2D LoadTexture(string textureName)
        {
            var texture = ContentManager.Load<Texture2D>(textureName);
            return texture ?? ContentManager.Load<Texture2D>
                (FallbackTexture);
        }

        protected void AddText(Text text)
        {
            Texts.Add(text);
        }

        protected void RemoveText(Text text)
        {
            Texts.Remove(text);
        }
    }
}
