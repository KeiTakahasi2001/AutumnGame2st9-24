using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class WaypointPath : MonoBehaviour
{
    public Transform[] points;
    private LineRenderer lineRenderer;
    private Material lineMaterial;

    [Header("予告線の設定")]
    [SerializeField] private float displayDuration = 3f; // 予告線を出す時間（秒）
    [SerializeField] private float scrollSpeed = 2f;     // 矢印が流れるスピード

    private Coroutine hideCoroutine;

    void Awake()
    {
        points = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            points[i] = transform.GetChild(i);
        }

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer.material != null)
        {
            lineMaterial = lineRenderer.material;
        }

        DrawPathLine();
        SetPathLineVisible(false); // 最初は隠しておく
    }

    void Update()
    {
        // 予告線が表示されている間、テクスチャを進行方向に流す！
        if (lineRenderer.enabled && lineMaterial != null)
        {
            float offset = Time.time * scrollSpeed;
            lineMaterial.mainTextureOffset = new Vector2(-offset, 0);
        }
    }

    public void DrawPathLine()
    {
        if (points == null || points.Length < 2) return;

        lineRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, points[i].position);
        }
    }

    // 【新機能！】指定された時間だけ予告線を表示して、自動で消す！
    public void ShowPathLineForSeconds(float duration = -1f)
    {
        if (duration < 0) duration = displayDuration;

        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        SetPathLineVisible(true);
        hideCoroutine = StartCoroutine(HideAfterTime(duration));
    }

    private IEnumerator HideAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetPathLineVisible(false);
    }

    public void SetPathLineVisible(bool visible)
    {
        if (lineRenderer != null)
        {
            lineRenderer.enabled = visible;
        }
    }

    private void OnDrawGizmos()
    {
        if (transform.childCount < 2) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }
    }
}