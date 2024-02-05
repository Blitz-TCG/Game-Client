using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class VideoScrubber : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
{
    public Slider slider;
    public PlayVideo playVideo;

    private bool isDragging = false;
    private bool isVideoPrepared = false;

    void Start()
    {
        slider.onValueChanged.AddListener(HandleSliderChange);
        playVideo.videoPlayer.prepareCompleted += VideoPrepared;
        playVideo.videoPlayer.Prepare();
    }

    private void VideoPrepared(VideoPlayer source)
    {
        isVideoPrepared = true;
        slider.maxValue = 1; // Assuming video length will be used as a normalized value (0 to 1).
    }

    void Update()
    {
        if (!isDragging && isVideoPrepared && playVideo.videoPlayer.isPlaying)
        {
            slider.value = (float)(playVideo.videoPlayer.time / playVideo.videoPlayer.length);
        }
    }

    private void HandleSliderChange(float value)
    {
        if (!isDragging) return; // Ignore changes to the slider value unless the user is dragging the slider.

        playVideo.videoPlayer.time = value * playVideo.videoPlayer.length;
        if (!playVideo.videoPlayer.isPlaying)
        {
            playVideo.videoPlayer.Play();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        // Play video on releasing the drag if the video is prepared.
        if (isVideoPrepared && !playVideo.videoPlayer.isPlaying)
        {
            playVideo.videoPlayer.Play();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Calculate the value for the slider based on the mouse position.
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                slider.fillRect as RectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localPoint))
        {
            float pct = Mathf.InverseLerp(slider.fillRect.rect.min.x, slider.fillRect.rect.max.x, localPoint.x);
            slider.value = pct * slider.maxValue;
            HandleSliderChange(slider.value); // Update the video to the new slider position.
        }
    }
}
