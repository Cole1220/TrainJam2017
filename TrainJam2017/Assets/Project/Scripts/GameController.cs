using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour, IScreens, IInputAvailable
{
    private GameObject m_gGameplayObject;

    public ProgressBar progressBarComponent;
    public ComboController comboMaker;
    public MiniGameController miniGamesController;
    public MapController mapController;
    public AudioHolder m_cAudioHolder;
    public AudioSource m_cAudioSource;

    // Use this for initialization
    public void Init()
    {
        Game.game.m_iScore = 0;

        m_gGameplayObject = new GameObject();

        m_cAudioSource = m_gGameplayObject.AddComponent<AudioSource>();
        Debug.Log("GameController: Init: m_iCurrentLevel: " + Game.game.m_iCurrentLevel);
        Debug.Log("GameController: Init: levelorder: " + Game.game.m_arrLevelOrder[Game.game.m_iCurrentLevel]);
        Debug.Log("GameController: Init: audio: " + Game.game.m_arrLevelAudio[Game.game.m_arrLevelOrder[Game.game.m_iCurrentLevel]]);
        m_cAudioSource.clip = (Game.game.m_arrLevelAudio[Game.game.m_arrLevelOrder[Game.game.m_iCurrentLevel]]);
        m_cAudioSource.Play();

        //Init Player Controller
        //GameObject progressBar = Instantiate(Resources.Load(PROGRESS_BAR_PREFAB)) as GameObject;
        progressBarComponent = m_gGameplayObject.AddComponent<ProgressBar>();
        progressBarComponent.Init();

        //Init top map
        mapController = m_gGameplayObject.AddComponent<MapController>();
        mapController.Init();

        //Init combo meter
        comboMaker = m_gGameplayObject.AddComponent<ComboController>();
        comboMaker.Init();

        //Init games
        miniGamesController = m_gGameplayObject.AddComponent<MiniGameController>();
        miniGamesController.Init();

        Reset();
    }
	
    public void Reset()
    {
        //No need atm

        Game.game.inputManager.SetFocus(this);
    }

	// Update is called once per frame
	void Update ()
    {
		
	}

    public void HandleInput(KeyCode key)
    {
        miniGamesController.HandleInput(key);
    }

    public void Populate()
    {
        //No need atm
    }

    public void DestroyScreen()
    {
        progressBarComponent.Destroy();
        mapController.Destroy();
        comboMaker.Destroy();
        miniGamesController.Destroy();

        Destroy(m_gGameplayObject);
    }
}
