using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossComboController : MonoBehaviour
{
    private static string TEXT_COMBO_STRING = "COMBO";
    private static string CANVAS_COMBO = "CanvasBossCombo";

    private static float FULL_BAR = 0f;
    private static float EMPTY_BAR = -65f;

    private static float POSITION_X = 260f;
    private static float POSITION_Y = 116f;

    private int m_iComboMax = 0;
    private int m_iCurrentCombo = 0;

    private float m_fMaxTime = 5f;
    private float m_fCurrentTime = 0f;
    private float m_fRemoveAmount;
    private float m_fFillAmount;

    GameObject m_gComboObject;
    GameObject m_gFillBar;

    Text ComboTitleText;
    Text ComboText;

    // Use this for initialization
    public void Init()
    {
        m_gComboObject = Instantiate(Resources.Load(CANVAS_COMBO)) as GameObject;
        m_gComboObject.transform.SetParent(Game.game.canvas.gameObject.transform, false);

        GameObjectHolder holder = m_gComboObject.GetComponent<GameObjectHolder>();
        ComboTitleText = holder.gObjects[0].GetComponent<Text>();
        ComboTitleText.text = TEXT_COMBO_STRING;

        //ComboText = holder.gObjects[1].GetComponent<Text>();
        //ComboText.text = "";

        m_gComboObject.SetActive(false);
    }

    public void Reset()
    {
        SetCombo();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetCombo(float ComboAmount = -1f)
    {
        m_fMaxTime = m_fCurrentTime = ComboAmount;
        UpdateCombo();
    }

    private void UpdateCombo()
    {
        m_fFillAmount = FULL_BAR;
        //Debug.Log("UpdateCombo: Fillamount: " + m_fFillAmount);
        if (m_fCurrentTime > 0)
        {
            m_iCurrentCombo += 1;
            if (m_iCurrentCombo > m_iComboMax)
            {
                Game.game.m_iBestCombo = m_iComboMax = m_iCurrentCombo;
            }
        }
        else
        {
            m_iCurrentCombo = 0;
        }

        ComboText.text = "" + m_iCurrentCombo;

        if (m_iCurrentCombo > 0)
        {
            m_gComboObject.SetActive(true);
        }
        else
        {
            m_gComboObject.SetActive(false);
        }
    }

    public int GetComboModifier()
    {
        return m_iCurrentCombo;
    }

    public int GetBestCombo()
    {
        return m_iComboMax;
    }

    public void Destroy()
    {
        Destroy(m_gComboObject);
        Destroy(m_gFillBar);
    }
}
