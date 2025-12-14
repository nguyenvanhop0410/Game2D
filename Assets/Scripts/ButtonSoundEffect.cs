using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Attach script này vào Button để tự động phát âm thanh click
/// Sử dụng IPointerDownHandler để phát âm thanh ngay khi nhấn (giảm lag)
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSoundEffect : MonoBehaviour, IPointerDownHandler
{
    [Tooltip("Phát âm thanh khi nhấn xuống (true) hoặc khi nhả ra (false)")]
    [SerializeField] private bool playOnPointerDown = true;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        // Chỉ dùng onClick nếu không dùng OnPointerDown
        if (!playOnPointerDown && button != null)
        {
            button.onClick.AddListener(PlayClickSound);
        }
    }

    void OnDestroy()
    {
        if (!playOnPointerDown && button != null)
        {
            button.onClick.RemoveListener(PlayClickSound);
        }
    }

    // Phát âm thanh ngay khi pointer down (nhanh hơn onClick)
    public void OnPointerDown(PointerEventData eventData)
    {
        if (playOnPointerDown && button != null && button.interactable)
        {
            PlayClickSound();
        }
    }

    private void PlayClickSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClick();
        }
    }
}
