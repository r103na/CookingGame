using System.Collections.Generic;

using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class BackstoryState : BaseState
{
    private Queue<ImageObject> _backstoryImages = new();
    private ImageObject _currentImageObject;
    private bool IsBackstoryOver => _backstoryImages.Count == 0;

    public override void LoadContent()
    {
        //var image = new ImageObject();
        //LoadImage();
    }

    public override void Update(GameTime gameTime)
    {
            
    }

    public void LoadImage()
    {
        _currentImageObject = _backstoryImages.Dequeue();
        AddGameObject(_currentImageObject);
    }

    public void SwitchImage()
    {
        if (!IsBackstoryOver)
        {
            RemoveGameObject(_currentImageObject);
            LoadImage();
        }
        else
        {
            SwitchState(new GameplayState());
        }
    }
}