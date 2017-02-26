using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameController : MonoBehaviour, IScreens, IInputAvailable
{
    private const int DEFAULT_POINTS = 1000;
    private const string STAGE_BOSS_FIGHT = "BossFight";

    private GameObject m_gGameplayObject;

    public BossComboController comboMaker;
    public BossMapController mapController;
    public BeatGame m_cBeatGame;
    public AudioHolder m_cAudioHolder;
    public AudioSource m_cAudioSource;

    public int m_iCurrentPoints = DEFAULT_POINTS;

    // Use this for initialization
    public void Init()
    {
        m_gGameplayObject = Instantiate(Resources.Load(STAGE_BOSS_FIGHT)) as GameObject;

        mapController = m_gGameplayObject.AddComponent<BossMapController>();
        mapController.Init();

        m_cAudioHolder = m_gGameplayObject.GetComponent<AudioHolder>();
        m_cAudioSource = m_gGameplayObject.AddComponent<AudioSource>();
        m_cAudioSource.clip = m_cAudioHolder.Audio[0];
        m_cAudioSource.Play();

        comboMaker = m_gGameplayObject.AddComponent<BossComboController>();
        comboMaker.Init();

        m_cBeatGame = m_gGameplayObject.AddComponent<BeatGame>();
        m_cBeatGame.Init();

        Reset();
    }

    public void Reset()
    {
        Game.game.inputManager.SetFocus(this);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Boss Update");
        if(m_cBeatGame.IsComplete())
        {
            Debug.Log("Boss Update complete");
            Game.game.m_iBossScore = m_iCurrentPoints;
        }
    }

    public void HandleInput(KeyCode key)
    {
        m_cBeatGame.HandleInput(key);
    }

    public void Populate()
    {
        //No need atm
    }

    public void DestroyScreen()
    {
        m_cBeatGame.Destroy();
        mapController.Destroy();
        comboMaker.Destroy();

        Destroy(m_gGameplayObject);
    }

    public void AddPoints(int points)
    {
        if(m_iCurrentPoints + points < 0)
        {
            m_iCurrentPoints = 0;
        }
        else
        {
            m_iCurrentPoints += points;
        }
    }

    public int GetPoints()
    {
        return m_iCurrentPoints;
    }
}

