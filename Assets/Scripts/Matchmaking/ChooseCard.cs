using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChooseCard : MonoBehaviour
{
    public UnityEvent OnSelected;
    public bool isEnabled = false;

    private bool isSelected = false;

    void Start()
    {
        Debug.Log("start called choose card " + gameObject.name);
        Deselect();
    }

    public void Select()
    {
        Debug.Log("inside Select " + transform.name + " child name  " + transform.GetChild(0).name);
        Debug.Log(transform.GetComponent<ChooseCard>().enabled + " enabled or not ");
        if (isEnabled)
        {
            isSelected = true;
            OnSelected.Invoke();
        }
    }

    public void Deselect()
    {
        isSelected = false;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    void OnMouseDown()
    {
        Debug.Log("onmouse down called " + transform.name + " is Selected " + isSelected);
        if (!isSelected)
        {
            Debug.Log("!isSelected");
            Select();
        }
    }

}
