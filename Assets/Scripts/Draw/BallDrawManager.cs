using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BallDrawManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button startButton;
    public Button extraButton;

    [Header("Prefabs & Parents")]
    public GameObject ballPrefab;
    public Transform gridOdd;   // For balls 1,3,5...
    public Transform gridEven;  // For balls 2,4,6...

    [Header("Sprites")]
    public Sprite[] ballSprites;   // 6 ball styles for final appearance
    public Sprite[] toggleSprites; // 3 sprites used for toggling animation
    public Sprite[] digitSprites;  // 0-9 digit sprites

    private int ballCount = 0;
    private int extraCount = 0;
    private const int maxExtraBalls = 12;

    void Start()
    {
        startButton.onClick.AddListener(OnStartPressed);
        extraButton.onClick.AddListener(OnExtraPressed);
        extraButton.gameObject.SetActive(false);
    }

    void OnStartPressed()
    {
        startButton.interactable = false;
        ballCount = 0;
        extraCount = 0;

        foreach (Transform child in gridOdd) Destroy(child.gameObject);
        foreach (Transform child in gridEven) Destroy(child.gameObject);

        StartCoroutine(StartDrawSequence());
    }

    IEnumerator StartDrawSequence()
    {
        yield return new WaitForSeconds(3f);

        for (int i = 1; i <= 30; i++)
        {
            Transform parentGrid = (i % 2 == 1) ? gridOdd : gridEven;
            StartCoroutine(CreateBall(parentGrid, i));
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.5f);
        extraButton.gameObject.SetActive(true);
        extraButton.interactable = true;
    }

    void OnExtraPressed()
    {
        if (extraCount >= maxExtraBalls) return;

        StartCoroutine(CreateBallWithSlide((ballCount % 2 == 0) ? gridEven : gridOdd));
        extraCount++;

        if (++ballCount >= 42)
        {
            extraButton.interactable = false;
            startButton.interactable = true;
        }
    }

    IEnumerator CreateBall(Transform parentGrid, int ballIndex)
    {
        GameObject ball = Instantiate(ballPrefab, parentGrid);
        RectTransform rect = ball.GetComponent<RectTransform>();

        float xOffset = (ballIndex - 1) * 80f;
        rect.anchoredPosition = new Vector2(xOffset, 0);

        Image ballImage = ball.GetComponent<Image>();
        Transform digitObject = ball.transform.Find("DigitObject");
        Image digit1 = digitObject.Find("Digit1").GetComponent<Image>();
        Image digit2 = digitObject.Find("Digit2").GetComponent<Image>();
        GameObject winnerBall = digitObject.Find("WinerBall")?.gameObject;

        for (int i = 0; i < 3; i++)
        {
            ballImage.sprite = toggleSprites[Random.Range(0, toggleSprites.Length)];
            yield return new WaitForSeconds(0.1f);
        }

        Sprite selectedSprite = ballSprites[Random.Range(0, ballSprites.Length)];
        ballImage.sprite = selectedSprite;

        if (selectedSprite.name == "ball-w")
        {
            selectedSprite = ballSprites[Random.Range(0, ballSprites.Length)];
            ballImage.sprite = selectedSprite;
            if (winnerBall != null) winnerBall.SetActive(true);
        }
        else
        {
            if (winnerBall != null) winnerBall.SetActive(false);
        }

        int number = Random.Range(0, 100);
        int d1 = number / 10;
        int d2 = number % 10;
        digit1.sprite = digitSprites[d1];
        digit2.sprite = digitSprites[d2];

        Debug.Log($"Ball {ballIndex}: Sprite={ballImage.sprite.name}, Number={number:D2}");

        ballCount++;
    }

    IEnumerator CreateBallWithSlide(Transform parentGrid)
    {
        GameObject ball = Instantiate(ballPrefab, parentGrid);
        RectTransform rect = ball.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -300);

        Image ballImage = ball.GetComponent<Image>();
        Transform digitObject = ball.transform.Find("DigitObject");
        Image digit1 = digitObject.Find("Digit1").GetComponent<Image>();
        Image digit2 = digitObject.Find("Digit2").GetComponent<Image>();
        GameObject winnerBall = digitObject.Find("WinerBall")?.gameObject;

        float duration = 0.5f;
        float elapsed = 0f;
        Vector2 startPos = rect.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, 0);

        while (elapsed < duration)
        {
            rect.anchoredPosition = Vector2.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        rect.anchoredPosition = endPos;

        for (int i = 0; i < 3; i++)
        {
            ballImage.sprite = toggleSprites[Random.Range(0, toggleSprites.Length)];
            yield return new WaitForSeconds(0.1f);
        }

        Sprite selectedSprite = ballSprites[Random.Range(0, ballSprites.Length)];
        ballImage.sprite = selectedSprite;

        if (selectedSprite.name == "ball-w")
        {
            selectedSprite = ballSprites[Random.Range(0, ballSprites.Length)];
            ballImage.sprite = selectedSprite;
            if (winnerBall != null) winnerBall.SetActive(true);
        }
        else
        {
            if (winnerBall != null) winnerBall.SetActive(false);
        }

        int number = Random.Range(0, 100);
        int d1 = number / 10;
        int d2 = number % 10;
        digit1.sprite = digitSprites[d1];
        digit2.sprite = digitSprites[d2];

        Debug.Log($"Extra Ball: Sprite={ballImage.sprite.name}, Number={number:D2}");
    }
}
