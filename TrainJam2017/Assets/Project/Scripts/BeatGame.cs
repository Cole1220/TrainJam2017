using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatGame : MonoBehaviour, IMinigame
{
    private const string STAGE_BEAT_BAR = "BeatBar";
    private const string STAGE_PLAYER_ICON = "PlayerBossIcon";
    private const string STAGE_BOSS_ICON = "BossIcon";
    private const int TURNS_TOTAL = 2;
    private const int DOTS_TOTAL = 16;

    private const string BUTTON_A = "A_btn";
    private const string BUTTON_S = "S_btn";
    private const string BUTTON_D = "D_btn";
    private const string BUTTON_NONE = "NONE";

    private const float THRESHOLD_PERFECT = 0.01f;
    private const float THRESHOLD_EARLY_LATE = 0.1f;
    private const float THRESHOLD_BAD = .15f;

    private GameObject m_gBeatBar;
    private GameObject m_gPlayerIcon;
    private GameObject m_gBossIcon;
    private GameObject m_gIconStart;
    private GameObject m_gIconEnd;
    private GameObject[] m_arrDots;
    private GameObjectHolder m_cBeatBarItems;
    private SpriteHolder m_cSpriteHolder;

    private Dictionary<string, Sprite> m_dStringKeyToSprite;
    private Dictionary<string, KeyCode> m_dStringToKey;
    private Dictionary<int, string> m_dArrToStringKey;
    private Dictionary<int, Sprite> m_dArrToDefaultSprite;
    private string[] m_arrStrings;

    private Vector3 m_vBossPosition;
    Dictionary<GameObject, int> m_dGameObjToArr;
    Dictionary<int, bool> m_dArrToBool;
    Dictionary<int, bool> m_dBossDotData;
    private int m_iCurrBossDot;

    private Vector3 m_vPlayerPosition;
    Dictionary<int, bool> m_dPlayerDotData;
    private int m_iCurrPlayerDot;
    private ColliderScript colScript;

    private KeyCode m_kCurrentKey;

    private int m_iDotArrStart;
    private int m_iDotArrEnd;
    private int TotalTurns = TURNS_TOTAL;
    private int m_iCurrentTurn = 0;
    private bool m_bBossTurn = false;
    private bool m_bPlayerTurn = false;
    private bool m_bStartRound = false;
    private bool m_bRoundOver = false;
    private bool m_bComplete = false;

	public void Init()
    {
        m_gBeatBar = Instantiate(Resources.Load(STAGE_BEAT_BAR)) as GameObject;
        m_gBeatBar.transform.SetParent(gameObject.transform, false);

        m_cBeatBarItems = m_gBeatBar.GetComponent<GameObjectHolder>();
        m_cSpriteHolder = m_gBeatBar.GetComponent<SpriteHolder>();

        m_gIconStart = m_cBeatBarItems.gObjects[0];
        m_gIconEnd = m_cBeatBarItems.gObjects[1];

        m_gBossIcon = Instantiate(Resources.Load(STAGE_BOSS_ICON)) as GameObject;
        m_gBossIcon.transform.SetParent(m_gBeatBar.transform, false);
        m_gBossIcon.transform.localPosition = m_gIconStart.transform.localPosition;

        m_gPlayerIcon = Instantiate(Resources.Load(STAGE_PLAYER_ICON)) as GameObject;
        m_gPlayerIcon.transform.SetParent(m_gBeatBar.transform, false);
        m_gPlayerIcon.transform.localPosition = m_gIconStart.transform.localPosition;
        colScript = m_gPlayerIcon.GetComponent<ColliderScript>();

        m_arrStrings = new string[] { BUTTON_A, BUTTON_S, BUTTON_D };
        m_dStringToKey = new Dictionary<string, KeyCode>();
        m_dStringToKey[BUTTON_A] = KeyCode.A;
        m_dStringToKey[BUTTON_S] = KeyCode.S;
        m_dStringToKey[BUTTON_D] = KeyCode.D;
        m_dStringToKey[BUTTON_NONE] = KeyCode.At;

        m_dStringKeyToSprite = new Dictionary<string, Sprite>();
        m_dStringKeyToSprite[BUTTON_A] = m_cSpriteHolder.m_sprSpriteObj[0];
        m_dStringKeyToSprite[BUTTON_S] = m_cSpriteHolder.m_sprSpriteObj[1];
        m_dStringKeyToSprite[BUTTON_D] = m_cSpriteHolder.m_sprSpriteObj[2];

        m_iDotArrStart = 3;
        m_iDotArrEnd = 19;
        int totalDots = m_iDotArrEnd - m_iDotArrStart;
        m_arrDots = new GameObject[totalDots];
        m_dGameObjToArr = new Dictionary<GameObject, int>();
        m_dPlayerDotData = new Dictionary<int, bool>();
        m_dArrToStringKey = new Dictionary<int, string>();
        m_dArrToDefaultSprite = new Dictionary<int, Sprite>();
        for (int i = 0; i < DOTS_TOTAL; ++i)
        {
            m_arrDots[i] = m_cBeatBarItems.gObjects[i + m_iDotArrStart];
            m_dGameObjToArr[m_arrDots[i]] = i;
            m_dPlayerDotData[i] = false;
            m_dArrToStringKey[i] = BUTTON_NONE;
            if(i != 0 && (i + 1) % 4 == 0)
            {
                m_dArrToDefaultSprite[i] = m_cSpriteHolder.m_sprSpriteObj[4];
            }
            else
            {
                m_dArrToDefaultSprite[i] = m_cSpriteHolder.m_sprSpriteObj[3];
            }
        }

        BossTurn();
    }

    public void BossTurn()
    {
        m_iCurrBossDot = 0;
        m_gBossIcon.transform.localPosition = m_gIconStart.transform.localPosition;

        if (m_iCurrentTurn + 1 > TotalTurns)
        {
            Debug.Log("Boss Done!");
            Game.game.m_iBossScore = Game.game.bossPage.m_iCurrentPoints;
            Game.game.ChangeState(Game.STATE_SCORE);
        }

        //choose a random int (range - amount)
        int dotsAmount = Random.Range(1,3);
        //choose that amount with random dots on bar
        m_dArrToBool = new Dictionary<int, bool>();
        int randNum = 0;

        //Make all false
        for(int i = 0; i < DOTS_TOTAL; ++i)
        {
            m_dArrToBool[i] = false;
        }

        //Set rand trues
        for (int i = 0; i < dotsAmount;)
        {
            randNum = Random.Range(0, DOTS_TOTAL);
            if (!m_dArrToBool[randNum])
            {
                m_dArrToBool[randNum] = true;
                m_dArrToStringKey[randNum] = m_arrStrings[Random.Range(0, (m_arrStrings.Length - 1))];
                ++i;
            }
        }
        //choose a button
        string buttonString = m_arrStrings[Random.Range(0, m_arrStrings.Length)];
        m_kCurrentKey = m_dStringToKey[buttonString];

        //Send off
        m_bBossTurn = true;
        m_bStartRound = true;
    }

    public void UpdateBossTurn()
    {
        if (m_gBossIcon.transform.position.x + THRESHOLD_BAD > m_gIconEnd.transform.position.x)
        {
            //We're done
            m_bBossTurn = false;
            PlayerTurn();
        }
        else
        {
            //Run turn
            //Hit those dots, and highlight them
            float speed = Time.deltaTime;
            m_vBossPosition = m_gBossIcon.transform.position;
            m_vBossPosition.x += speed;

            m_gBossIcon.transform.position = m_vBossPosition;
            GameObject gCurrentDot = m_arrDots[m_iCurrBossDot];
            if (Mathf.Abs(m_gBossIcon.transform.position.x - gCurrentDot.transform.position.x) <= THRESHOLD_EARLY_LATE)
            {
                if(m_dArrToBool[m_iCurrBossDot])
                {
                    //anim and highlight dot
                    string buttonString = m_dArrToStringKey[m_iCurrBossDot];
                    if(buttonString != BUTTON_NONE)
                    {
                        gCurrentDot.GetComponent<SpriteRenderer>().sprite = m_dStringKeyToSprite[buttonString];
                        gCurrentDot.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else
                    {
                        gCurrentDot.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
                else
                {
                    gCurrentDot.GetComponent<SpriteRenderer>().sprite = m_dArrToDefaultSprite[m_iCurrBossDot];
                    gCurrentDot.GetComponent<SpriteRenderer>().color = Color.gray;
                }

                if(m_iCurrBossDot + 1 < m_arrDots.Length)
                {
                    m_iCurrBossDot += 1;
                }
            }
        }
    }

    public void PlayerTurn()
    {
        m_iCurrPlayerDot = 0;
        m_gPlayerIcon.transform.localPosition = m_gIconStart.transform.localPosition;

        m_dPlayerDotData = new Dictionary<int, bool>();
        for (int i = 0; i < DOTS_TOTAL; ++i)
        {
            m_dPlayerDotData[i] = false;
        }

        m_bPlayerTurn = true;
    }

    public void UpdatePlayerTurn()
    {
        if (m_gPlayerIcon.transform.position.x + THRESHOLD_BAD > m_gIconEnd.transform.position.x)
        {
            //We're done
            m_iCurrentTurn += 1;
            
            Reset();
        }
        else
        {
            //Run your turn
            float speed = Time.deltaTime;
            m_vPlayerPosition = m_gPlayerIcon.transform.position;
            m_vPlayerPosition.x += speed;

            m_gPlayerIcon.transform.position = m_vPlayerPosition;
        }
    }

    public void Reset()
    {
        m_bPlayerTurn = false;

        BossTurn();
    }

    private void Update()
    {
        if(m_iCurrentTurn < TotalTurns)
        {
            if (m_bStartRound)
            {
                if (m_bBossTurn)
                {
                    UpdateBossTurn();
                }
                else if (m_bPlayerTurn)
                {
                    UpdatePlayerTurn();
                }
            }
        }
    }

    public void HandleInput(KeyCode key)
    {
        int arrIdx = m_dGameObjToArr[colScript.m_gCurrentCollide];
        string stringIdx = m_dArrToStringKey[arrIdx];
        KeyCode correctKey = m_dStringToKey[stringIdx];
        switch (key)
        {
            case KeyCode.At: //Null case
                Game.game.bossPage.AddPoints(-10);
                break;
            default:
                if(key == correctKey)
                {
                    //Check if right place
                    ComparePress();
                }
                else
                {
                    //Wrong Key
                    Debug.Log("Wrong key");
                    Game.game.bossPage.AddPoints(-10);
                }
                break;
        }
    }

    public void ComparePress()
    {
        if (m_bPlayerTurn)
        {
            int arrIdx = m_dGameObjToArr[colScript.m_gCurrentCollide];
            if (m_dPlayerDotData[arrIdx]) { return; }
            m_dPlayerDotData[arrIdx] = true;
            GameObject gCurrentDot = m_arrDots[arrIdx];
            if (m_dArrToBool[arrIdx])
            {
                float playerDotDiff = (m_gPlayerIcon.transform.position.x - gCurrentDot.transform.position.x);
                float absDiff = Mathf.Abs(playerDotDiff);
                if (absDiff <= THRESHOLD_BAD)
                {
                    if (absDiff <= THRESHOLD_EARLY_LATE)
                    {
                        if (absDiff <= THRESHOLD_PERFECT)
                        {
                            //feedback perfect
                            gCurrentDot.GetComponent<SpriteRenderer>().color = Color.blue;
                            Game.game.bossPage.AddPoints(0);
                        }
                        else
                        {
                            //Feedback early/late
                            gCurrentDot.GetComponent<SpriteRenderer>().color = Color.cyan;
                            Game.game.bossPage.AddPoints(-5);
                        }
                    }
                    else
                    {
                        //Feedback Bad
                        gCurrentDot.GetComponent<SpriteRenderer>().color = Color.red;
                        Game.game.bossPage.AddPoints(-10);
                    }
                }
                else
                {
                    //Buzz?
                    gCurrentDot.GetComponent<SpriteRenderer>().color = Color.black;
                    Game.game.bossPage.AddPoints(-100);
                }
            }
        }
    }

    public bool IsComplete()
    {
        if(m_bComplete)
        {
            m_bComplete = false;
            return true;
        }
        return false;
    }

    public void Destroy()
    {
        Destroy(m_gBeatBar);
    }
}
