using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManagerHandler : MonoBehaviour
{
    private IInputAvailable handleInput = null;

    private KeyCode[] keys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space, KeyCode.Return };

	// Update is called once per frame
	void Update ()
    {
        if(handleInput != null)
        {
            for(int i = 0; i < keys.Length; ++i)
            {
                if (Input.GetKeyDown(keys[i]))
                {
                    handleInput.HandleInput(keys[i]);
                }
            }
        }
	}

    public void SetFocus(IInputAvailable focusObj)
    {
        handleInput = focusObj;
    }
}
