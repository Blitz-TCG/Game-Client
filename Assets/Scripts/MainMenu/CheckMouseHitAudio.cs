using UnityEngine;

public class CheckMouseHitAudio : MonoBehaviour
{
    public CursorManager cursorManager;
    public GameObject profileDropdown;

    private void OnMouseDown()
    {
        if (!MainMenuUIManager.instance.friendsUI.activeSelf && !MainMenuUIManager.instance.settingsUI.activeSelf)
        {
            cursorManager.AudioClickButtonStandard();

            if (profileDropdown.activeSelf)
            {
                profileDropdown.SetActive(false);
            }
            else if (!profileDropdown.activeSelf)
            {
                profileDropdown.SetActive(true);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (!MainMenuUIManager.instance.friendsUI.activeSelf && !MainMenuUIManager.instance.settingsUI.activeSelf)
        {
            cursorManager.CursorSelect();
            cursorManager.AudioHoverButtonStandard();
        }
    }

    private void OnMouseExit()
    {
        if (!MainMenuUIManager.instance.friendsUI.activeSelf && !MainMenuUIManager.instance.settingsUI.activeSelf)
        {
            cursorManager.CursorNormal();
        }
    }
}
