using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    [SerializeField] private Transform target; // Player transform
    [SerializeField] private Vector3 worldOffset = new Vector3(0, 1.2f, 0); // Offset phía trên player
    [SerializeField] private bool smoothFollow = true;
    [SerializeField] private float smoothSpeed = 10f;

    private RectTransform rectTransform;
    private Canvas canvas;
    private Camera mainCamera;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        mainCamera = Camera.main;

        if (canvas == null)
        {
            Debug.LogError("HealthBarFollow cần nằm trong một Canvas!");
        }

        if (target == null)
        {
            // Tự động tìm Player nếu chưa gán
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
                Debug.Log("✓ Tự động tìm thấy Player để follow");
            }
            else
            {
                Debug.LogError("✗ Không tìm thấy target! Hãy gán Player transform.");
            }
        }
    }

    void LateUpdate()
    {
        if (target == null || canvas == null || mainCamera == null) return;

        // Tính vị trí world của player + offset
        Vector3 worldPosition = target.position + worldOffset;

        // Chuyển đổi từ world position sang screen position
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // Chuyển từ screen position sang local position trong Canvas
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
            out localPoint
        );

        // Cập nhật vị trí của thanh máu
        if (smoothFollow)
        {
            rectTransform.anchoredPosition = Vector2.Lerp(
                rectTransform.anchoredPosition,
                localPoint,
                Time.deltaTime * smoothSpeed
            );
        }
        else
        {
            rectTransform.anchoredPosition = localPoint;
        }
    }
}
