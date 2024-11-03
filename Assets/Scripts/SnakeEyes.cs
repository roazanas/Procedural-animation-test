/*
    Claude generated
 */


using UnityEngine;
using System.Collections;

public class SnakeEyes : MonoBehaviour
{
    [Header("Eye Settings")]
    public GameObject eyePrefab;
    public float eyeSize = 0.15f;
    public float eyeDistanceFromCenter = 0.3f;
    public float eyeForwardOffset = 0.1f;

    [Header("Blinking Settings")]
    public bool enableAutoBlinking = true;
    public float minBlinkInterval = 2f;
    public float maxBlinkInterval = 5f;
    public float blinkDuration = 0.1f;

    private GameObject leftEye;
    private GameObject rightEye;
    private SnakeGenerator snakeGenerator;
    private DistanceConstraint headConstraint;
    private bool isBlinking = false;
    private Vector2 currentDirection;

    private void Start()
    {
        snakeGenerator = GetComponent<SnakeGenerator>();
        if (snakeGenerator.snakeParts.Count == 0) return;

        headConstraint = snakeGenerator.snakeParts[0].GetComponent<DistanceConstraint>();

        // Создаем глаза с дополнительным пустым объектом для вращения
        leftEye = CreateEyeWithRotation("LeftEye");
        rightEye = CreateEyeWithRotation("RightEye");

        if (enableAutoBlinking)
        {
            StartCoroutine(AutoBlinkRoutine());
        }
    }

    private GameObject CreateEyeWithRotation(string eyeName)
    {
        // Создаем контейнер для вращения
        GameObject eyeContainer = new GameObject(eyeName + "Container");
        eyeContainer.transform.SetParent(snakeGenerator.snakeParts[0].transform, false);

        // Создаем сам глаз как дочерний объект контейнера
        GameObject eye = Instantiate(eyePrefab, Vector3.zero, Quaternion.identity);
        eye.name = eyeName;
        eye.transform.localScale = Vector3.one * eyeSize;
        eye.transform.SetParent(eyeContainer.transform, false);

        return eyeContainer;
    }

    private void Update()
    {
        if (headConstraint == null || leftEye == null || rightEye == null) return;

        // Получаем направление движения головы
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 headPos = snakeGenerator.snakeParts[0].transform.position;
        currentDirection = (mousePos - headPos).normalized;

        // Вычисляем угол поворота
        float angle = Mathf.Atan2(currentDirection.y, currentDirection.x) * Mathf.Rad2Deg;

        // Получаем толщину головы
        float headWidth = snakeGenerator.BodyWidth(0) / 100f;

        // Смещение вперёд
        Vector2 forwardOffset = currentDirection * eyeForwardOffset;

        // Боковое смещение для глаз
        Vector2 sideOffset = new Vector2(-currentDirection.y, currentDirection.x);

        // Позиции глаз
        Vector2 leftEyePos = headPos + forwardOffset + (sideOffset * eyeDistanceFromCenter * headWidth);
        Vector2 rightEyePos = headPos + forwardOffset - (sideOffset * eyeDistanceFromCenter * headWidth);

        // Обновляем позиции и поворот контейнеров глаз
        leftEye.transform.position = leftEyePos;
        rightEye.transform.position = rightEyePos;

        // Поворачиваем контейнеры глаз
        leftEye.transform.rotation = Quaternion.Euler(0, 0, angle);
        rightEye.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private IEnumerator AutoBlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minBlinkInterval, maxBlinkInterval));
            if (!isBlinking)
            {
                StartCoroutine(Blink());
            }
        }
    }

    private IEnumerator Blink()
    {
        if (isBlinking) yield break;

        isBlinking = true;
        float originalScale = eyeSize;

        // Закрываем глаза
        for (float t = 0; t < blinkDuration; t += Time.deltaTime)
        {
            float scaleY = Mathf.Lerp(originalScale, 0, t / blinkDuration);
            SetEyeScale(originalScale, scaleY);
            yield return null;
        }

        // Открываем глаза
        for (float t = 0; t < blinkDuration; t += Time.deltaTime)
        {
            float scaleY = Mathf.Lerp(0, originalScale, t / blinkDuration);
            SetEyeScale(originalScale, scaleY);
            yield return null;
        }

        // Возвращаем исходный размер
        SetEyeScale(originalScale, originalScale);

        isBlinking = false;
    }

    private void SetEyeScale(float scaleX, float scaleY)
    {
        // Устанавливаем масштаб для самих глаз (дочерних объектов)
        leftEye.transform.GetChild(0).localScale = new Vector3(scaleX, scaleY, 1);
        rightEye.transform.GetChild(0).localScale = new Vector3(scaleX, scaleY, 1);
    }

    public void TriggerBlink()
    {
        if (!isBlinking)
        {
            StartCoroutine(Blink());
        }
    }
}