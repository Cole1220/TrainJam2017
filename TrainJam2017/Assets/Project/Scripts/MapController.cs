using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    private const string CANVAS_TOP_MASK = "CanvasTopMask";
    private const string CANVAS_TOP_IMAGE = "CanvasBgdImage";
    private const string CANVAS_BOTTOM_MASK = "CanvasBottomMask";
    private const string CANVAS_BOTTOM_IMAGE = "CanvasBottomBgdImage";
    private const string CANVAS_BOTTOM_DIRT = "Dirt";

    private GameObject m_gCanvasTopMask;
    private Image m_imgTopImage0;

    private GameObject m_gCanvasBottomMask;
    private Image m_imgBottomImage0;

    // Use this for initialization
    public void Init ()
    {
        //Init mask Images will live in
        m_gCanvasTopMask = Instantiate(Resources.Load(CANVAS_TOP_MASK)) as GameObject;
        m_gCanvasTopMask.transform.SetParent(Game.game.canvas.gameObject.transform, false);

        //Init Image 0
        GameObject image0 = Instantiate(Resources.Load(CANVAS_TOP_IMAGE)) as GameObject;
        image0.transform.SetParent(m_gCanvasTopMask.transform, false);
        m_imgTopImage0 = image0.GetComponent<Image>();
        m_imgTopImage0.sprite = Game.game.GetLevel()[0];

        //BOTTOM
        //Init mask Images will live in
        m_gCanvasBottomMask = Instantiate(Resources.Load(CANVAS_BOTTOM_MASK)) as GameObject;
        m_gCanvasBottomMask.transform.SetParent(Game.game.canvas.gameObject.transform, false);

        //Init Image 0
        GameObject image1 = Instantiate(Resources.Load(CANVAS_BOTTOM_IMAGE)) as GameObject;
        image1.transform.SetParent(m_gCanvasBottomMask.transform, false);
        m_imgBottomImage0 = image1.GetComponent<Image>();
        m_imgBottomImage0.sprite = Game.game.GetLevel()[1];

        GameObject image2 = Instantiate(Resources.Load(CANVAS_BOTTOM_DIRT)) as GameObject;
        image2.transform.SetParent(m_gCanvasBottomMask.transform, false);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Destroy()
    {
        Destroy(m_gCanvasTopMask);
        Destroy(m_gCanvasBottomMask);
    }

}
