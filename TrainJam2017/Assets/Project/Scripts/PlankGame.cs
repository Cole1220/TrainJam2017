using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlankGame : MonoBehaviour, IMinigame
{
    private const float COMBO_TIME = 10.0f;
    private const int DEFAULT_POINTS = 10;

    private GameObject m_iGoalObject;
    private GameObject m_iPlankObject;

    private Vector3 m_iGoalRotation; //Range of -45 - 45
    private Vector3 m_iPlankRotation;
    private Vector3 m_vDiffThreshold = new Vector3(0,0,2f);
    private bool m_bIsComplete = false;
    private bool m_bReset = false;

    private float m_fRotationSpeed = 20f;

    // Use this for initialization
    public void Init()
    {
        //Set up Bar
        GameObjectHolder component = gameObject.GetComponent<GameObjectHolder>();
        if (component)
        {
            Debug.Log("PlankGame: Init: Component: Found It");
            m_iGoalObject = component.FocusItem0;
            m_iPlankObject = component.FocusItem1;
        }
        else
        {
            Debug.Log("PlankGame: Init: Failed");
        }

        Reset();
    }

    public void Reset()
    {
        m_bIsComplete = false;

        float rot = Random.Range(-45, 45);

        m_iGoalRotation = new Vector3(0,0,rot);
        m_iGoalObject.transform.eulerAngles = m_iGoalRotation;

        m_iPlankRotation = new Vector3(0, 0, 0);
        m_iPlankObject.transform.eulerAngles = m_iPlankRotation;

        m_bReset = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Input Block
        if(Input.GetKey(KeyCode.A))//Think L2/R2
        {
            Debug.Log("A");
            m_iPlankRotation.z += (Time.deltaTime * m_fRotationSpeed);
        }
        else if (Input.GetKey(KeyCode.D))//Think L2/R2
        {
            Debug.Log("D");
            m_iPlankRotation.z -= (Time.deltaTime * m_fRotationSpeed);
        }

        m_iPlankObject.transform.eulerAngles = m_iPlankRotation;

        if (Input.GetKeyDown(KeyCode.S))
        {
            Vector3 rotDifference = m_iPlankObject.transform.eulerAngles - m_iGoalObject.transform.eulerAngles;
            if (Mathf.Abs(rotDifference.z) <= m_vDiffThreshold.z)
            {
                m_bIsComplete = true;
                Debug.Log("Correct: diff: " + rotDifference);
                Game.game.AddScore(DEFAULT_POINTS * Game.game.gamePage.comboMaker.GetComboModifier());
                Game.game.gamePage.comboMaker.SetCombo(COMBO_TIME);
            }
            else
            {
                Debug.Log("Incorrect: diff: " + rotDifference);
                Game.game.gamePage.comboMaker.SetCombo();
            }
        }
    }

    public bool IsComplete()
    {
        if(m_bReset)
        {
            if(m_bIsComplete)
            {
                m_bIsComplete = false;
                return true;
            }
            return false;
        }
        return false;
    }
}
