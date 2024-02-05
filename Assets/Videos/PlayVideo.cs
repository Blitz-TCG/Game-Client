using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayVideo : MonoBehaviour
{
    public Button yourButton; // Assign this in the inspector
    public VideoPlayer videoPlayer; // Assign this in the inspector

    void Start()
    {
        // Register the onclick event
        yourButton.onClick.AddListener(ToggleVideoPlayPause);
    }

    void ToggleVideoPlayPause()
    {
        if (videoPlayer != null)
        {
            // Check if the video is playing
            if (videoPlayer.isPlaying)
            {
                // Pause the video if it's currently playing
                videoPlayer.Pause();
            }
            else
            {
                // Play the video if it's currently paused
                videoPlayer.Play();
            }
        }
    }
}
