/*
    Claude generated
 */


using UnityEngine;
using System.Collections;

public class SnakeTongue : MonoBehaviour
{
    [Header("Tongue Settings")]
    public Color tongueColor;
    public float tongueWidth = 0.08f;
    public float tongueLength = 0.3f;
    public float forkSpread = 0.15f;
    public float forkLength = 0.12f;

    [Header("Animation Settings")]
    public float minFlickInterval = 2f;
    public float maxFlickInterval = 4f;
    public float flickDuration = 0.2f;
    public float flickSpeed = 2f;
    public float vibrationAmount = 0.03f;

    private GameObject tongue;
    private LineRenderer tongueRenderer;
    private SnakeGenerator snakeGenerator;
    private bool isFlicking = false;
    private Vector3[] tonguePoints;

    private void Start()
    {
        snakeGenerator = GetComponent<SnakeGenerator>();
        if (snakeGenerator.snakeParts.Count == 0) return;

        CreateTongue();
        StartCoroutine(AutoFlickRoutine());
        tonguePoints = new Vector3[3];
    }

    private void CreateTongue()
    {
        tongue = new GameObject("SnakeTongue");
        tongue.transform.SetParent(snakeGenerator.snakeParts[0].transform);

        tongueRenderer = tongue.AddComponent<LineRenderer>();
        tongueRenderer.startWidth = tongueWidth;
        tongueRenderer.endWidth = tongueWidth * 0.3f;
        tongueRenderer.positionCount = 3;
        tongueRenderer.material = new Material(Shader.Find("Sprites/Default"));
        tongueRenderer.startColor = tongueColor;
        tongueRenderer.endColor = tongueColor;
        tongueRenderer.sortingOrder = 3;

        HideTongue();
    }

    private void Update()
    {
        if (tongue == null || !isFlicking) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 headPos = snakeGenerator.snakeParts[0].transform.position;
        Vector2 direction = (mousePos - headPos).normalized;

        // Получаем толщину головы
        float headWidth = snakeGenerator.BodyWidth(0) / 100f;

        // Позиционируем основание языка точно на краю головы
        Vector2 tongueBase = headPos + direction * (headWidth / 2f);

        // Добавляем вибрацию
        float vibration = Mathf.Sin(Time.time * flickSpeed) * vibrationAmount;
        Vector2 perpDirection = new Vector2(-direction.y, direction.x);

        // Центральная точка языка
        Vector2 tongueCenter = tongueBase + direction * tongueLength;
        tongueCenter += perpDirection * vibration;

        // Конечные точки вилки
        Vector2 leftFork = tongueCenter + direction * forkLength + perpDirection * forkSpread;
        Vector2 rightFork = tongueCenter + direction * forkLength - perpDirection * forkSpread;

        // Обновляем позиции в LineRenderer
        tongueRenderer.SetPosition(0, tongueBase);
        tongueRenderer.SetPosition(1, tongueCenter);
        tongueRenderer.SetPosition(2, Time.frameCount % 2 == 0 ? leftFork : rightFork);
    }

    private IEnumerator AutoFlickRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minFlickInterval, maxFlickInterval));
            if (!isFlicking)
            {
                StartCoroutine(FlickTongue());
            }
        }
    }

    private IEnumerator FlickTongue()
    {
        if (isFlicking) yield break;

        isFlicking = true;
        ShowTongue();

        // Высовываем язык
        float elapsedTime = 0f;
        while (elapsedTime < flickDuration)
        {
            tongueRenderer.startWidth = Mathf.Lerp(0, tongueWidth, elapsedTime / (flickDuration * 0.2f));
            tongueRenderer.endWidth = Mathf.Lerp(0, tongueWidth * 0.3f, elapsedTime / (flickDuration * 0.2f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(flickDuration);

        // Убираем язык
        elapsedTime = 0f;
        while (elapsedTime < flickDuration * 0.5f)
        {
            tongueRenderer.startWidth = Mathf.Lerp(tongueWidth, 0, elapsedTime / (flickDuration * 0.5f));
            tongueRenderer.endWidth = Mathf.Lerp(tongueWidth * 0.3f, 0, elapsedTime / (flickDuration * 0.5f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HideTongue();
        isFlicking = false;
    }

    private void ShowTongue()
    {
        tongueRenderer.enabled = true;
    }

    private void HideTongue()
    {
        tongueRenderer.enabled = false;
    }

    public void TriggerFlick()
    {
        if (!isFlicking)
        {
            StartCoroutine(FlickTongue());
        }
    }

    public void IncreaseFlickRate()
    {
        StopAllCoroutines();
        minFlickInterval = 0.5f;
        maxFlickInterval = 1f;
        StartCoroutine(AutoFlickRoutine());
    }

    public void RestoreNormalFlickRate()
    {
        StopAllCoroutines();
        minFlickInterval = 2f;
        maxFlickInterval = 4f;
        StartCoroutine(AutoFlickRoutine());
    }
}