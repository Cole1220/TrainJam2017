using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePageController : MonoBehaviour, IScreens, IInputAvailable
{
    private const string SCREEN_PREFAB = "TitleScreen";

    private GameObject m_gTitleScreen;

    //Text components
    TextHolder m_cTextHolder;

    public void Init()
    {
        m_gTitleScreen = Instantiate(Resources.Load(SCREEN_PREFAB)) as GameObject;
        m_gTitleScreen.transform.SetParent( Game.game.canvas.transform, false );

        //Get Text Components
        m_cTextHolder = m_gTitleScreen.GetComponent<TextHolder>();

        Reset();
    }

    public void Reset()
    {
        //TODO
        Game.game.inputManager.SetFocus(this);
        Game.game.ResetLevels(); //Reset Levels
    }

    void Update()
    {
        
    }

    public void HandleInput(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.Space:
            case KeyCode.Return:
            case KeyCode.A:
                //Progress to game
                Game.game.ChangeState(Game.STATE_GAMEPLAY);
                break;
            case KeyCode.S:
                //Instructions
                break;
            case KeyCode.D:
                Application.Quit();
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
