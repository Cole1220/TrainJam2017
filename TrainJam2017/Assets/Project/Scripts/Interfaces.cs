using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigame
{
    void Init();
    void Reset();
    bool IsComplete();
}

public interface IScreens
{
    void Init();
    void Reset();
    void Populate();
    void DestroyScreen();
}

public interface IInputAvailable
{
    void HandleInput(KeyCode key);
}