using System;
using System.Collections.Generic;

using CookingGame.Objects;
using Microsoft.Xna.Framework;

namespace CookingGame.States;

public class BackstoryState : BaseState
{
    private Queue<ImageObject> _backstoryImages = new();
    private ImageObject _currentImageObject;
    private float waitNextImage;
    private bool IsBackstoryOver => _backstoryImages.Count == 0;

    public override void LoadContent()
    {
        
    }

    public override void Update(GameTime gameTime)
    {
        UpdateTime(gameTime);
        OnUpdated(null, EventArgs.Empty);
    }

    private void LoadImageQueue()
    {
        //_backstoryImages.Enqueue(new ImageObject());
    }

    public void LoadImage()
    {
        _currentImageObject = _backstoryImages.Dequeue();
        AddGameObject(_currentImageObject);
    }

    private void SwitchImage()
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

    private void WaitForImage()
    {

    }

}