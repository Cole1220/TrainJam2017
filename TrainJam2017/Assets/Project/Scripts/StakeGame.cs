using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StakeGame : MonoBehaviour, IMinigame
{
    private const float COMBO_TIME = 5f;
    private const int DEFAULT_POINTS = 2;

    private const int STAKE_NORMAL = 0;
    private const int STAKE_LEFT = 1;
    private const int STAKE_RIGHT = 2;

    private static float PROBABILITY_SUCCESS = 0.5f;
    private static float PROBABILITY_SUCCESS_AI = 0.5f;

    private static int AMOUNT_TO_STAKE = 3;
    private static float POSITION_STAKE_INITIAL_Y = .35f;
    private static float POSITION_STAKE_END_Y = -2f;
    private static Vector3 HAMMER_NAIL_DIFF = new Vector3(-.8f, 1.8f, 0);

    private KeyCode m_kLeftKey = KeyCode.A;
    private KeyCode m_kCenterKey = KeyCode.S;
    private KeyCode m_kRightKey = KeyCode.D;

    private GameObject m_gPlayerStake;
    private GameObject m_gAIStake;
    private GameObject m_gHammer;
    private GameObject m_gAudio;

    private AudioHolder m_cAudioHolder;
    private AudioSource m_audAudioSource;
    private Animator m_animHammerAnim;

    private Dictionary<KeyCode,int> m_dInputToPosition = new Dictionary<KeyCode, int>();
    private Dictionary<int, float> m_dPositionToRotation = new Dictionary<int, float>();

    private int m_iStakeState = STAKE_NORMAL;

    private int m_iPlayerRandomNumber;
    private int m_iCurrentHammerAmount = 0;

    private Vector3 m_vStakePosition;
    private Vector3 m_vStakeEndPosition;
    private float m_fStakeDivision;

    private bool m_bIsComplete = false;
    private bool m_bReset = false;

    // Use this for initialization
    public void Init()
    {
        m_dInputToPosition = new Dictionary<KeyCode, int>();
        m_dInputToPosition[m_kLeftKey] = STAKE_LEFT;
        m_dInputToPosition[m_kCenterKey] = STAKE_NORMAL;
        m_dInputToPosition[m_kRightKey] = STAKE_RIGHT;

        m_dPositionToRotation = new Dictionary<int, float>();
        m_dPositionToRotation[STAKE_LEFT] = 20f;
        m_dPositionToRotation[STAKE_NORMAL] = 0f;
        m_dPositionToRotation[STAKE_RIGHT] = -20f;

        //Set up Bar
        GameObjectHolder component = gameObject.GetComponent<GameObjectHolder>();
        if (component)
        {
            Debug.Log("StakeGame: Init: Component: Found It");
            m_gPlayerStake = component.FocusItem0;
            m_gAIStake = component.FocusItem1;
            m_gHammer = component.AnimatedObj;
            m_gAudio = component.AudioObj;

            m_animHammerAnim = m_gHammer.GetComponent<Animator>();
            m_audAudioSource = m_gAudio.GetComponent<AudioSource>();
            m_cAudioHolder = m_gAudio.GetComponent<AudioHolder>();
        }
        else
        {
            Debug.Log("StakeGame: Init: Failed");
        }

        Reset();
    }

    public void Reset()
    {
        m_bIsComplete = false;
        m_iCurrentHammerAmount = 0;

        //Reset Positions
        m_vStakePosition = new Vector3(m_gAIStake.transform.localPosition.x, POSITION_STAKE_INITIAL_Y, m_gAIStake.transform.localPosition.z);
        m_gAIStake.transform.localPosition = m_vStakePosition;
        m_vStakePosition = new Vector3(m_gPlayerStake.transform.localPosition.x, POSITION_STAKE_INITIAL_Y, m_gPlayerStake.transform.localPosition.z);
        m_gPlayerStake.transform.localPosition = m_vStakePosition;

        m_gHammer.transform.localPosition = m_vStakePosition + HAMMER_NAIL_DIFF;

        m_vStakeEndPosition = new Vector3(m_gPlayerStake.transform.localPosition.x, POSITION_STAKE_END_Y, m_gPlayerStake.transform.localPosition.z);
        m_fStakeDivision = (m_gPlayerStake.transform.localPosition.y - m_vStakeEndPosition.y) / AMOUNT_TO_STAKE;

        m_iStakeState = STAKE_NORMAL;
        PositionStake();

        m_bReset = true;
    }
	
	// Update is called once per frame
	void Update ()
    {

        if(Input.GetKeyDown(m_kLeftKey))
        {
            Compare(m_kLeftKey);
        }
        else if(Input.GetKeyDown(m_kCenterKey))
        {
            Compare(m_kCenterKey);
        }
        else if (Input.GetKeyDown(m_kRightKey))
        {
            Compare(m_kRightKey);
        }
    }

    private void Compare(KeyCode keyPressed)
    {
        m_animHammerAnim.SetTrigger("SwingHammer");

        if (m_dInputToPosition[keyPressed] == m_iStakeState)
        {
            m_audAudioSource.PlayOneShot(m_cAudioHolder.Audio[0]);
            Progress();
        }
        else
        {
            m_audAudioSource.PlayOneShot(m_cAudioHolder.Audio[1]);
            Game.game.gamePage.comboMaker.SetCombo();
        }
    }

    private void Progress()
    {
        Game.game.AddScore(DEFAULT_POINTS * Game.game.gamePage.comboMaker.GetComboModifier());
        Game.game.gamePage.comboMaker.SetCombo(COMBO_TIME);

        if (m_iCurrentHammerAmount + 1 < AMOUNT_TO_STAKE)
        {
            //Move stake lower (-1.2f end)
            m_iCurrentHammerAmount += 1;
            m_gPlayerStake.transform.localPosition -= new Vector3(0, m_fStakeDivision, 0);
            m_gHammer.transform.localPosition -= new Vector3(0, m_fStakeDivision, 0);

            //Move to new rotation state
            if (m_iCurrentHammerAmount == AMOUNT_TO_STAKE - 1)
            {
                PositionStake(STAKE_NORMAL);
            }
            else
            {
                PositionStake((Random.Range(STAKE_NORMAL, STAKE_RIGHT * 10)) % (STAKE_RIGHT + 1));
            }
        }
        else
        {
            Debug.Log("Stake Game Completed");
            m_bIsComplete = true;
        }
    }

    private void PositionStake(int forcePosition = -1)
    {
        if (forcePosition < 0) { forcePosition = m_iStakeState; }
        if (m_dPositionToRotation.ContainsKey(forcePosition)) { m_iStakeState = forcePosition; }

        float zPosition = m_dPositionToRotation[m_iStakeState];
        Vector3 rotation = new Vector3(0, 0, zPosition);
        m_gPlayerStake.transform.eulerAngles = rotation;
    }

    public bool IsComplete()
    {
        if(m_bReset)
        {
            if (m_bIsComplete)
            {
                m_bIsComplete = false;
                return true;
            }
            return false;
        }
        return false;
    }
}
