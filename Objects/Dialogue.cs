﻿using CookingGame.Managers;
using CookingGame.Objects.Base;
using Microsoft.Xna.Framework;

namespace CookingGame.Objects;

public class Dialogue : ClickableSprite
{
    private DialogueManager dialogueManager;
    public override void HandleClick(Point clickPosition)
    {
        
    }

    public override void HandleHold(Point clickPosition)
    {
        throw new System.NotImplementedException();
    }

    public override void HandleRelease(Point clickPosition)
    {
        
    }
}