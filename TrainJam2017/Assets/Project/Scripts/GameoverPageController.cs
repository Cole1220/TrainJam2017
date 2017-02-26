using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverPageController : MonoBehaviour, IScreens, IInputAvailable
{
    private const string SCREEN_PREFAB = "GameOverScreen";

    private GameObject m_gTitleScreen;

    //Text components
    TextHolder m_cTextHolder;

    public void Init()
    {
        m_gTitleScreen = Instantiate(Resources.Load(SCREEN_PREFAB)) as GameObject;
        m_gTitleScreen.transform.SetParent(Game.game.canvas.transform, false);

        Reset();
    }

    public void Reset()
    {
        //TODO
        Game.game.inputManager.SetFocus(this);
    }

    void Update()
    {

    }

    public void HandleInput(KeyCode key)
    {
        switch (key)
        {
            case KeyCode.Space:
            case KeyCode.Return:
                //Progress to game
                Game.game.ChangeState(Game.STATE_TITLE);
                break;
            default:
                break;
        }
    }

    public void Populate()
    {

    }

    public void DestroyScreen()
    {
        Destroy(m_gTitleScreen);
    }
}
