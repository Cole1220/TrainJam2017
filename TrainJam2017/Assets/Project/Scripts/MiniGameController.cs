using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameController : MonoBehaviour
{
    private static string PLANK_GAME_PREFAB = "PlankGame";
    private static string STAKE_GAME_PREFAB = "StakeGame";
    
    private string[] m_arrGameOrder = { PLANK_GAME_PREFAB, STAKE_GAME_PREFAB };
    private GameObject[] m_arrMinigameObjects;
    private GameObject m_gCurrentGame;
    private IMinigame m_cCurrentComponent;
    private int m_iArrayProgress = 0;
    private bool m_bSkipCheck = false;

    public void Init()
    {
        //Plank Game
        //Stake Game (One Side)
        m_arrMinigameObjects = new GameObject[m_arrGameOrder.Length];
        for(int i = 0; i < m_arrGameOrder.Length; ++i)
        {
            m_arrMinigameObjects[i] = Instantiate(Resources.Load(m_arrGameOrder[i])) as GameObject;
            m_arrMinigameObjects[i].SetActive(false);
            IMinigame component = m_arrMinigameObjects[i].GetComponent<IMinigame>();
            if (component != null)
            {
                component.Init();
            }
        }

        Reset();
    }

	// Use this for initialization
	public void Reset ()
    {
        m_iArrayProgress = 0;

        for (int i = 0; i < m_arrGameOrder.Length; ++i)
        {
            GameObject refGameObject = m_arrMinigameObjects[i];
            refGameObject.SetActive(false);
            IMinigame component = refGameObject.GetComponent<IMinigame>();
            if (component != null)
            {
                component.Reset();
            }
        }

        m_gCurrentGame = m_arrMinigameObjects[m_iArrayProgress];
        m_gCurrentGame.SetActive(true);
        m_cCurrentComponent = m_gCurrentGame.GetComponent<IMinigame>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!m_bSkipCheck)
        {
            Debug.Log("Dont skip check");
            if (m_cCurrentComponent != null)
            {
                Debug.Log("component Not null: ");
                if (m_cCurrentComponent.IsComplete())
                {
                    Debug.Log("is complete");
                    m_bSkipCheck = true;
                    Progress();
                }
            }
            else
            {
                Debug.Log("No Component, something went wrong");
            }
        }
	}

    public void HandleInput(KeyCode key)
    {

    }

    private void Progress()
    {
        if(m_iArrayProgress + 1 < m_arrGameOrder.Length)
        {
            m_gCurrentGame.SetActive(false);
            m_iArrayProgress += 1;

            m_gCurrentGame = m_arrMinigameObjects[m_iArrayProgress];
            m_gCurrentGame.SetActive(true);
            m_cCurrentComponent = m_gCurrentGame.GetComponent<IMinigame>();
            m_cCurrentComponent.Reset();
        }
        else
        {
            Debug.Log("Progress Bar");
            Game.game.gamePage.progressBarComponent.ProgressPlayerNext();
            Reset();
        }

        m_bSkipCheck = false;
    }

    public void EndGames()
    {
        for (int i = 0; i < m_arrGameOrder.Length; ++i)
        {
            GameObject refGameObject = m_arrMinigameObjects[i];
            refGameObject.SetActive(false);
            IMinigame component = refGameObject.GetComponent<IMinigame>();
            if (component != null)
            {
                component.Reset();
            }
        }
    }

    public void Destroy()
    {
        for (int i = m_arrGameOrder.Length - 1; i >= 0; --i)
        {
            Destroy(m_arrMinigameObjects[i]);
        }
    }
}
