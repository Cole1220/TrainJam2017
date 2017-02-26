using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePageController : MonoBehaviour, IScreens, IInputAvailable
{
    private const string SCREEN_PREFAB = "ScoreScreen";
    private const string STRING_LEVEL = "Level";
    private const string STRING_OF = "of";

    private GameObject m_gScoreScreen;

    //Text components
    TextHolder m_cTextHolder;

    public void Init()
    {
        m_gScoreScreen = Instantiate(Resources.Load(SCREEN_PREFAB)) as GameObject;
        m_gScoreScreen.transform.SetParent(Game.game.canvas.transform, false);

        //Get Text Components
        m_cTextHolder = m_gScoreScreen.GetComponent<TextHolder>();

        Reset();
    }

    public void Reset()
    {
        Populate();
        Game.game.inputManager.SetFocus(this);
    }

	// Update is called once per frame
	void Update ()
    {
		
	}

    public void HandleInput(KeyCode key)
    {
        switch(key)
        {
            case KeyCode.Space:
            case KeyCode.Return:
                //As To Go To Next Level
                Game.game.NextLevel();
                break;
            default:
                break;
        }
    }

    public void Populate()
    {
        int score = Game.game.m_iScore;
        int level = Game.game.m_iCurrentLevel;
        int tLevel = Game.game.TotalLevels;
        int combo = Game.game.m_iBestCombo;
        int boss  = Game.game.m_iBossScore;

        m_cTextHolder.Text0.GetComponent<Text>().text = "" + score;
        m_cTextHolder.Text1.GetComponent<Text>().text = "" + (level + 1);
        m_cTextHolder.Text2.GetComponent<Text>().text = "" + combo;
        m_cTextHolder.Text3.GetComponent<Text>().text = "" + ((score * (level + 1)) + combo + boss);
        m_cTextHolder.Text5.GetComponent<Text>().text = "" + boss;

        m_cTextHolder.Text4.GetComponent<Text>().text = STRING_LEVEL + " " + (level + 1) + " " + STRING_OF + " " + tLevel;
    }

    public void DestroyScreen()
    {
        Destroy(m_gScoreScreen);
    }
}
