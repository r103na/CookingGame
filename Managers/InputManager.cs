using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using CookingGame.Objects.Base;

namespace CookingGame.Managers
{
    public class InputManager
    {
        private readonly List<ClickableSprite> clickableSprites = new List<ClickableSprite>();

        public void RegisterClickableSprite(ClickableSprite sprite)
        {
            clickableSprites.Add(sprite);
            //sprite.Clicked += OnClickableSpriteClicked;
        }

        private static void OnClickableSpriteClicked(object sender, EventArgs e)
        {
            // Handle the sprite click event
        }

        public void HandleClick(Point clickPosition)
        {
            foreach (var sprite in clickableSprites)
            {
                sprite.HandleClick(clickPosition);
            }
        }
    }

}
