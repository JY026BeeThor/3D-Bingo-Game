using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BallSpawner : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public Sprite[] digitSprites; // 0–9

    public List<Transform> startPositionsOdd;
    public List<Transform> startPositionsEven;

    public List<Transform> endPositionsOdd;
    public List<Transform> endPositionsEven;

    public float slideDuration = 1f;
    private int oddIndex = 0;
    private int evenIndex = 0;

    public void SpawnBall()
    {
        int number = Random.Range(0, 100);
        GameObject prefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];

        bool isEven = number % 2 == 0;

        Transform start = isEven ? startPositionsEven[evenIndex % startPositionsEven.Count]
                                 : startPositionsOdd[oddIndex % startPositionsOdd.Count];

        Transform end = isEven ? endPositionsEven[evenIndex % endPositionsEven.Count]
                               : endPositionsOdd[oddIndex % endPositionsOdd.Count];

        if (isEven) evenIndex++;
        else oddIndex++;

        StartCoroutine(SlideBall(prefab, number, start, end));
    }

    IEnumerator SlideBall(GameObject prefab, int number, Transform startPoint, Transform endPoint)
    {
        GameObject ball = Instantiate(prefab, startPoint.position, Quaternion.identity, transform);

        int leftDigit = number / 10;
        int rightDigit = number % 10;

        foreach (Image img in ball.GetComponentsInChildren<Image>())
        {
            if (img.name == "LeftDigit")
                img.sprite = digitSprites[leftDigit];
            else if (img.name == "RightDigit")
                img.sprite = digitSprites[rightDigit];
        }

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;
            ball.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, t);
            yield return null;
        }

        ball.transform.position = endPoint.position;
    }
}
