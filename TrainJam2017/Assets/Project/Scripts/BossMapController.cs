using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossMapController : MonoBehaviour
{
    private const string CANVAS_MASK = "CanvasMask";
    private const string CANVAS_IMAGE = "CanvasImage";

    private GameObject m_gCanvasMask;
    private Image m_imgImage;

    // Use this for initialization
    public void Init()
    {
        //Init mask Images will live in
        m_gCanvasMask = Instantiate(Resources.Load(CANVAS_MASK)) as GameObject;
        m_gCanvasMask.transform.SetParent(Game.game.canvas.gameObject.transform, false);

        //Init Image 0
        GameObject image0 = Instantiate(Resources.Load(CANVAS_IMAGE)) as GameObject;
        image0.transform.SetParent(m_gCanvasMask.transform, false);
        m_imgImage = image0.GetComponent<Image>();
        m_imgImage.sprite = Game.game.GetLevel()[0];
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Destroy()
    {
        Destroy(m_gCanvasMask);
    }
}