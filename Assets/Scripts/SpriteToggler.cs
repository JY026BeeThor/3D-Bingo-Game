using UnityEngine;

public class ZoomAlternate : MonoBehaviour
{
    public Transform objectA;
    public Transform objectB;

    public float zoomScale = 1.2f;        // How much to zoom in
    public float zoomDuration = 2f;       // Duration of each zoom

    private Vector3 originalScaleA;
    private Vector3 originalScaleB;

    void Start()
    {
        if (objectA == null || objectB == null)
        {
            Debug.LogError("Assign Object A and Object B in the inspector.");
            return;
        }

        originalScaleA = objectA.localScale;
        originalScaleB = objectB.localScale;

        StartCoroutine(ZoomLoop());
    }

    private System.Collections.IEnumerator ZoomLoop()
    {
        while (true)
        {
            // A zooms out, B zooms in
            yield return StartCoroutine(AnimateZoom(objectA, objectB, zoomOutA: true));

            // A zooms in, B zooms out
            yield return StartCoroutine(AnimateZoom(objectA, objectB, zoomOutA: false));
        }
    }

    private System.Collections.IEnumerator AnimateZoom(Transform a, Transform b, bool zoomOutA)
    {
        float elapsed = 0f;

        Vector3 aStart = zoomOutA ? originalScaleA * zoomScale : originalScaleA;
        Vector3 aEnd = zoomOutA ? originalScaleA : originalScaleA * zoomScale;

        Vector3 bStart = zoomOutA ? originalScaleB : originalScaleB * zoomScale;
        Vector3 bEnd = zoomOutA ? originalScaleB * zoomScale : originalScaleB;

        while (elapsed < zoomDuration)
        {
            float t = Mathf.SmoothStep(0f, 1f, elapsed / zoomDuration);
            a.localScale = Vector3.Lerp(aStart, aEnd, t);
            b.localScale = Vector3.Lerp(bStart, bEnd, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        a.localScale = aEnd;
        b.localScale = bEnd;
    }
}
