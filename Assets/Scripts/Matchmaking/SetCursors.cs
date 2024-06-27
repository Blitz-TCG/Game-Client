using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursors : MonoBehaviour
{
    private CursorManager manager;

    public void SearchCursor()
    {
        manager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
        manager.CursorIbeam();
    }

    public void NormalCursor()
    {
        manager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
        manager.CursorNormal();
    }
}
