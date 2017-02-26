using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    //Consts
    public static string PROGRESS_BAR_PREFAB = "ProgressBar";
    private static string TRAIN_ICON = "TrainProgressIcon";
    private static string PLAYER_ICON = "PlayerProgressIcon";

    private static Vector3 DEFAULT_FLAG_POSITION = new Vector3(0.32f,0,0);

    //Stage Objects
    private GameObject m_gProgress;
    private GameObject m_gProgressBar;
    private GameObject m_gLeft;
    private GameObject m_gRight;
    private GameObject m_gTrainIcon;
    private GameObject m_gPlayerIcon;

    //Variables
    private int m_iTotalRails;
    private int m_iCurrentRail = 0;
    private Vector3 m_vDivision = Vector3.zero;
    private float m_fTimer = 0f;

    //Train Variables
    private float m_fTrainSpeed = 0.15f;
    private bool m_bMoveTrain = false;
    private bool m_bReset = false;
    private bool m_bComplete = false;

    // Use this for initialization
    public void Init()
    {
        m_gProgress = Instantiate(Resources.Load(PROGRESS_BAR_PREFAB)) as GameObject;
        m_gProgressBar = m_gProgress.GetComponent<GameObjectHolder>().FocusItem0;

        //Set up Bar
        GameObjectHolder component = m_gProgress.GetComponent<GameObjectHolder>();
        if (component)
        {
            Debug.Log("ProgressBar: Init: Component: Found It");
            m_gProgressBar = component.FocusItem0;
            m_gLeft = component.FocusItem1;
            m_gRight = component.Background;
        }
        else
        {
            Debug.Log("ProgressBar: Init: Component: Can't Find It");
        }

        //Get your data
        m_iTotalRails = Game.game.Rails;
        m_vDivision = (m_gRight.transform.position - m_gLeft.transform.position) / (m_iTotalRails - 1);

        Reset();
    }

    public void Reset()
    {
        m_bReset = true;
        m_bComplete = false;
        //Init and Position Train Icon
        m_gTrainIcon = Instantiate(Resources.Load(TRAIN_ICON)) as GameObject;
        m_gTrainIcon.transform.SetParent(m_gProgress.transform, false);
        m_gTrainIcon.transform.localPosition = new Vector3(m_gLeft.transform.localPosition.x - 0.01F, m_gLeft.transform.localPosition.y, m_gLeft.transform.localPosition.z - 1);

        //Init and Position Player Progress Icon
        m_gPlayerIcon = Instantiate(Resources.Load(PLAYER_ICON)) as GameObject;
        m_gPlayerIcon.transform.SetParent(m_gProgress.transform, false);
        m_gPlayerIcon.transform.localPosition = new Vector3(m_gLeft.transform.localPosition.x, m_gLeft.transform.localPosition.y, m_gLeft.transform.localPosition.z - 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        m_fTimer += Time.deltaTime;

        //Move Train
        if (m_bMoveTrain)
        {
            m_gTrainIcon.transform.position = (m_gTrainIcon.transform.position + (m_vDivision * (Time.deltaTime * m_fTrainSpeed)));

            if (m_gTrainIcon.transform.localPosition.x > m_gPlayerIcon.transform.localPosition.x)
            {
                Game.game.ChangeState(Game.STATE_GAMEOVER);
            }
        }
	}

    public void ProgressPlayerNext()
    {
        if (m_iCurrentRail + 1 < m_iTotalRails)
        {
            m_iCurrentRail += 1;
            if(!m_bMoveTrain)
            {
                m_bMoveTrain = true;
            }

            Vector3 targetPos = (m_gLeft.transform.position + (m_vDivision * m_iCurrentRail));
            targetPos.z -= 1;
            m_gPlayerIcon.transform.position = targetPos; //TODO: Smooth move this
        }
        else
        {
            //Win level and go to next area
            Debug.Log("You Win");
            Game.game.ChangeState(Game.STATE_BOSS);
        }
    }

    public void ProgressPlayerToIdx(int progressionIdx)
    {
        m_gPlayerIcon.transform.position = (m_gLeft.transform.position + (m_vDivision * progressionIdx));
    }

    public void Destroy()
    {
        Destroy(m_gProgress);
    }
}