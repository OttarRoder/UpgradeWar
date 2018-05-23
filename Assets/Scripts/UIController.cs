using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance { set; get; }
    public Canvas canvas;

	void Awake ()
    {
        instance = this;
	}

    public Canvas getCanvas()
    {
        return canvas;
    }
}
