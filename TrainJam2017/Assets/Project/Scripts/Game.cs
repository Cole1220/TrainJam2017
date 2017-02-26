using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    private static string LEVEL_FARMLAND = "Farmland";
    private static string LEVEL_MOUNTAIN = "Mountain";
    private static string LEVEL_OCEAN    = "Ocean";
    private static string LEVEL_ARCTIC   = "Arctic";
    private static string LEVEL_SKY      = "Sky";
    private static string LEVEL_SPACE    = "Space";

    public const int STATE_TITLE = 0;
    public const int STATE_GAMEPLAY = 1;
    public const int STATE_SCORE = 2;
    public const int STATE_GAMEOVER = 3;
    public const int STATE_CREDITS = 4;
    public const int STATE_BOSS = 5;

    public int TotalLevels = 6;

    public Sprite[] Farmland = new Sprite[2];
    public Sprite[] Mountain = new Sprite[2];
    public Sprite[] Ocean = new Sprite[2];
    public Sprite[] Arctic = new Sprite[2];
    public Sprite[] Sky = new Sprite[2];
    public Sprite[] Space = new Sprite[2];

    public Dictionary<string, AudioClip> m_arrLevelAudio = new Dictionary<string, AudioClip>();

    public string[] m_arrLevelOrder;
    public Dictionary<string,Sprite[]> m_dLevelOrder = new Dictionary<string, Sprite[]>();

    public static Game game;
    public Camera cam;
    public Canvas canvas;
    public InputManagerHandler inputManager;

    public AudioHolder m_cAudioHolder;
    /*public ProgressBar progressBarComponent;
    public ComboController comboMaker;
    public MiniGameController miniGamesController;
    public MapController mapController;
    public AudioSource m_cAudioSource;

    private GameObject gameplayObject;//*/

    public TitlePageController titlePage;
    public ScorePageController scorePage;
    public CreditPageController creditPage;
    public GameController gamePage;
    public GameoverPageController gameoverPage;
    public BossGameController bossPage;

    [Header("Game Length Variables")]
    public int Rails = 100;
    private int m_iGameState = -1;
    public int m_iCurrentLevel = 0;
    public int m_iScore = 0;
    public int m_iBossScore = 0;
    public int m_iBestCombo = 0;

    public bool m_bGameOver = false;

    void Awake()
    {
        //Persistence
        if(game == null)
        {
            DontDestroyOnLoad(this);
            game = this;
        }
        else if(game != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    // Use this for initialization
    void Start()
    {
        inputManager = game.gameObject.AddComponent<InputManagerHandler>();

        m_arrLevelOrder = new string[]{ LEVEL_FARMLAND, LEVEL_MOUNTAIN, LEVEL_OCEAN, LEVEL_ARCTIC, LEVEL_SKY, LEVEL_SPACE };

        m_dLevelOrder[LEVEL_FARMLAND] = Farmland;
        m_dLevelOrder[LEVEL_MOUNTAIN] = Mountain;
        m_dLevelOrder[LEVEL_OCEAN]    = Ocean;
        m_dLevelOrder[LEVEL_ARCTIC]   = Arctic;
        m_dLevelOrder[LEVEL_SKY]      = Sky;
        m_dLevelOrder[LEVEL_SPACE]    = Space;

        m_cAudioHolder = game.GetComponent<AudioHolder>();
        for(int i = 0; i < m_arrLevelOrder.Length; ++i)
        {
            if (i < m_cAudioHolder.Audio.Length)
            {
                m_arrLevelAudio[m_arrLevelOrder[i]] = m_cAudioHolder.Audio[i];
            }
        }

        ChangeState(STATE_TITLE);
    }

    private void InitTitle()
    {
        titlePage = new TitlePageController();
        titlePage.Init();
    }

    private void InitScore()
    {
        scorePage = new ScorePageController();
        scorePage.Init();
    }

    private void InitCredit()
    {
        creditPage = new CreditPageController();
        creditPage.Init();
    }

    private void InitGameOver()
    {
        gameoverPage = new GameoverPageController();
        gameoverPage.Init();
    }

    public void InitGame()
    {
        gamePage = new GameController();
        gamePage.Init();
     }

    public void InitBoss()
    {
        bossPage = new BossGameController();
        bossPage.Init();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public void ChangeState(int state)
    {
        switch(m_iGameState)
        {
            case STATE_TITLE:
                titlePage.DestroyScreen();
                break;
            case STATE_GAMEPLAY:
                gamePage.DestroyScreen();
                break;
            case STATE_SCORE:
                scorePage.DestroyScreen();
                break;
            case STATE_GAMEOVER:
                gameoverPage.DestroyScreen();
                break;
            case STATE_CREDITS:
                creditPage.DestroyScreen();
                break;
            case STATE_BOSS:
                bossPage.DestroyScreen();
                break;
            default:
                break;
        }

        switch (state)
        {
            case STATE_TITLE:
                m_iGameState = state;
                InitTitle();
                break;
            case STATE_GAMEPLAY:
                m_iGameState = state;
                InitGame();
                break;
            case STATE_SCORE:
                m_iGameState = state;
                InitScore();
                break;
            case STATE_GAMEOVER:
                m_iGameState = state;
                InitGameOver();
                break;
            case STATE_CREDITS:
                m_iGameState = state;
                InitCredit();
                break;
            case STATE_BOSS:
                m_iGameState = state;
                InitBoss();
                break;
            default:
                break;
        }
    }


    public void NextLevel()
    {
        Debug.Log("Game: NextLevel: pre m_iCurrentLevel: " + m_iCurrentLevel);
        if(m_bGameOver)
        {
            ChangeState(STATE_GAMEOVER);
        }
        else
        {
            if(m_iCurrentLevel + 1 < TotalLevels)
            {
                m_iCurrentLevel += 1;

                ChangeState(STATE_GAMEPLAY);
            }
            else
            {
                ChangeState(STATE_CREDITS);
            }
        }
        Debug.Log("Game: NextLevel: post m_iCurrentLevel: " + m_iCurrentLevel);
    }

    public Sprite[] GetLevel()
    {
        Debug.Log("Game: GetLevel: m_iCurrentLevel: " + m_iCurrentLevel + ", m_arrLevelOrder[m_iCurrentLevel]: " + m_arrLevelOrder[m_iCurrentLevel] + ", ");
        return m_dLevelOrder[m_arrLevelOrder[m_iCurrentLevel]];
    }

    public void AddScore(int scoreAmount)
    {
        m_iScore += scoreAmount;
    }

    public void ResetLevels()
    {
        m_iCurrentLevel = 0;
    }
}
