using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BallSpawner : MonoBehaviour
{
    [Header("Large Ball Prefabs")]
    public GameObject blueBallLarge;
    public GameObject greenBallLarge;
    public GameObject purpleBallLarge;
    public GameObject goldBallLarge;
    public GameObject redBallLarge;
    public GameObject goldenBallLarge;
    public GameObject freeBallLarge;

    [Header("Small Ball Prefabs")]
    public GameObject blueBallSmall;
    public GameObject greenBallSmall;
    public GameObject purpleBallSmall;
    public GameObject goldBallSmall;
    public GameObject redBallSmall;

    [Header("Star & Grids")]
    public GameObject starPrefab;
    public Transform upperGrid;
    public Transform lowerGrid;
    public Transform starGrid;
    public Transform[] extraBallSlots;

    [Header("Shooting Origin")]
    public Transform shootOrigin;

    [Header("UI")]
    public Button buyExtraBallButton;
    public Button cancelButton;
    public CardManager cardManager;
    public GameObject freeBallBorderPrefab;

    private List<int> drawnNumbers = new List<int>();
    private List<int> extraDrawnNumbers = new List<int>();
    private int goldenBallCount = 0;
    private int freeBallCount = 0;
    private int extraBallIndex = 0;
    private bool isDrawing = false;

    private void Start()
    {
        cancelButton.onClick.AddListener(ResetGame);
    }

    public void StartDraw()
    {
        if (isDrawing) return;

        drawnNumbers.Clear();
        extraDrawnNumbers.Clear();
        goldenBallCount = 0;
        freeBallCount = 0;
        extraBallIndex = 0;

        foreach (Transform child in upperGrid) Destroy(child.gameObject);
        foreach (Transform child in lowerGrid) Destroy(child.gameObject);
        foreach (Transform child in starGrid) Destroy(child.gameObject);
        foreach (Transform slot in extraBallSlots)
        {
            foreach (Transform child in slot)
                Destroy(child.gameObject);
        }

        StartCoroutine(SpawnBallsCoroutine());
    }

    IEnumerator SpawnBallsCoroutine()
    {
        isDrawing = true;

        while (drawnNumbers.Count < 30)
        {
            int number = Random.Range(1, 91);
            if (drawnNumbers.Contains(number)) continue;

            drawnNumbers.Add(number);

            bool isGolden = goldenBallCount < 3 && Random.value < 0.05f;
            if (isGolden) goldenBallCount++;

            bool isFreeBall = freeBallCount < 1 && Random.value < 0.01f;
            if (isFreeBall) freeBallCount++;

            if (isGolden)
            {
                yield return StartCoroutine(HandleGoldenBall(number));
                continue;
            }

            if (isFreeBall)
            {
                yield return StartCoroutine(HandleFreeBall(number));
                continue;
            }

            GameObject largePrefab = GetLargePrefab(number);
            GameObject smallPrefab = GetSmallPrefab(number);

            GameObject tempBall = Instantiate(largePrefab, shootOrigin.position, Quaternion.identity, shootOrigin);
            tempBall.transform.localScale = Vector3.zero;
            tempBall.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();

            yield return StartCoroutine(AnimateBall(tempBall.transform));
            cardManager.MarkDrawnNumbers(new List<int> { number });

            Transform targetGrid = drawnNumbers.Count <= 23 ? upperGrid : lowerGrid;
            if (targetGrid == lowerGrid && lowerGrid.childCount >= 7)
                Destroy(lowerGrid.GetChild(0).gameObject);

            yield return new WaitForSeconds(0.05f);
            yield return StartCoroutine(FlyToTarget(tempBall.transform, targetGrid));

            GameObject historyBall = Instantiate(smallPrefab, targetGrid);
            historyBall.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();

            Destroy(tempBall);
        }

        isDrawing = false;
    }

    IEnumerator HandleGoldenBall(int number)
    {
        GameObject goldenBall = Instantiate(goldenBallLarge, shootOrigin.position, Quaternion.identity, shootOrigin);
        goldenBall.transform.localScale = Vector3.zero;
        goldenBall.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();

        yield return AnimateBall(goldenBall.transform);
        yield return new WaitForSeconds(0.3f);

        Vector3 starPos = goldenBall.transform.position;
        Destroy(goldenBall);

        GameObject star = Instantiate(starPrefab, starPos, Quaternion.identity, shootOrigin);
        star.transform.localScale = Vector3.zero;

        yield return AnimateBall(star.transform);
        yield return FlyDownToStarGrid(star);

        star.transform.SetParent(starGrid);
        yield return ScaleUpStar(star);

        Transform targetGrid = upperGrid.childCount < 23 ? upperGrid : lowerGrid;
        GameObject historyBall = Instantiate(GetSmallPrefab(number), targetGrid);
        historyBall.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
    }

    IEnumerator HandleFreeBall(int number)
    {
        GameObject freeBall = Instantiate(freeBallLarge, shootOrigin.position, Quaternion.identity, shootOrigin);
        freeBall.transform.localScale = Vector3.zero;
        freeBall.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();

        yield return AnimateBall(freeBall.transform);
        yield return new WaitForSeconds(1.0f);
        Destroy(freeBall);

        Transform targetGrid = upperGrid.childCount < 23 ? upperGrid : lowerGrid;
        GameObject historyBall = Instantiate(GetSmallPrefab(number), targetGrid);
        historyBall.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();
        AddFreeBallBorder(historyBall);
    }

    void AddFreeBallBorder(GameObject historyBall)
    {
        GameObject border = Instantiate(freeBallBorderPrefab, historyBall.transform);
        border.transform.localPosition = Vector3.zero;
        border.transform.localScale = Vector3.one * 0.8f;
    }

    public void BuyExtraBall()
    {
        if (extraBallIndex >= extraBallSlots.Length || isDrawing) return;

        int number;
        do
        {
            number = Random.Range(1, 91);
        } while (drawnNumbers.Contains(number) || extraDrawnNumbers.Contains(number));

        extraDrawnNumbers.Add(number);

        GameObject largePrefab = GetLargePrefab(number);
        GameObject tempBall = Instantiate(largePrefab, shootOrigin.position, Quaternion.identity, shootOrigin);
        tempBall.transform.localScale = Vector3.zero;
        tempBall.GetComponentInChildren<TextMeshProUGUI>().text = number.ToString();

        StartCoroutine(AnimateExtraBall(tempBall.transform, extraBallIndex, number));
        extraBallIndex++;

        cardManager.MarkDrawnNumbers(new List<int> { number });
    }

    IEnumerator AnimateExtraBall(Transform ball, int slotIndex, int number)
    {
        yield return AnimateBall(ball);

        Transform targetSlot = extraBallSlots[slotIndex];
        yield return FlyToTarget(ball, targetSlot);

        ball.SetParent(targetSlot);
        ball.localScale = Vector3.one * 0.1f;
        yield return ScaleUpExtraBall(ball);
    }

    private void ResetGame()
    {
        cardManager.ResetCardMarks();

        foreach (Transform child in upperGrid) Destroy(child.gameObject);
        foreach (Transform child in lowerGrid) Destroy(child.gameObject);
        foreach (Transform child in starGrid) Destroy(child.gameObject);
        foreach (Transform slot in extraBallSlots)
        {
            foreach (Transform child in slot)
                Destroy(child.gameObject);
        }

        drawnNumbers.Clear();
        extraDrawnNumbers.Clear();
        goldenBallCount = 0;
        freeBallCount = 0;
        extraBallIndex = 0;
        isDrawing = false;
    }

    GameObject GetLargePrefab(int number)
    {
        if (number <= 20) return blueBallLarge;
        else if (number <= 35) return greenBallLarge;
        else if (number <= 50) return purpleBallLarge;
        else if (number <= 70) return goldBallLarge;
        else return redBallLarge;
    }

    GameObject GetSmallPrefab(int number)
    {
        if (number <= 20) return blueBallSmall;
        else if (number <= 35) return greenBallSmall;
        else if (number <= 50) return purpleBallSmall;
        else if (number <= 70) return goldBallSmall;
        else return redBallSmall;
    }

    IEnumerator AnimateBall(Transform ball)
    {
        float duration = 0.3f;
        Vector3 start = Vector3.zero;
        Vector3 end = Vector3.one;
        float t = 0;

        while (t < duration)
        {
            ball.localScale = Vector3.Lerp(start, end, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        ball.localScale = end;
    }

    IEnumerator FlyToTarget(Transform ball, Transform targetGrid)
    {
        Vector3 startPos = ball.position;
        Vector3 endPos = targetGrid.position;
        Vector3 startScale = ball.localScale * 0.5f;
        Vector3 endScale = Vector3.one * 0.5f;
        float duration = 0.2f;
        float t = 0;

        while (t < duration)
        {
            ball.position = Vector3.Lerp(startPos, endPos, t / duration);
            ball.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        ball.position = endPos;
        ball.localScale = endScale;
    }

    IEnumerator FlyDownToStarGrid(GameObject star)
    {
        Vector3 startPos = star.transform.position;
        Vector3 endPos = starGrid.position;
        Vector3 startScale = star.transform.localScale;
        Vector3 endScale = Vector3.one * 1.5f;
        float duration = 1.0f;
        float t = 0;

        while (t < duration)
        {
            star.transform.position = Vector3.Lerp(startPos, endPos, t / duration);
            star.transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        star.transform.position = endPos;
        star.transform.localScale = endScale;
    }

    IEnumerator ScaleUpStar(GameObject star)
    {
        float duration = 0.2f;
        Vector3 startScale = star.transform.localScale;
        Vector3 endScale = Vector3.one * 2f;
        float t = 0;

        while (t < duration)
        {
            star.transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        star.transform.localScale = endScale;
    }

    IEnumerator ScaleUpExtraBall(Transform ball)
    {
        float duration = 0.2f;
        Vector3 startScale = ball.localScale;
        Vector3 endScale = Vector3.one * 0.4f;
        float t = 0;

        while (t < duration)
        {
            ball.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        ball.localScale = endScale;
    }

    public List<int> GetAllDrawnNumbers()
    {
        List<int> all = new List<int>(drawnNumbers);
        all.AddRange(extraDrawnNumbers);
        return all;
    }
}
