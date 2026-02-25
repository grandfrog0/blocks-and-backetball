using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowOpenAnimation : MonoBehaviour
{
    private Coroutine _openRoutine;
    public void Open()
    {
        gameObject.SetActive(true);

        if (_openRoutine == null)
        {
            _openRoutine = StartCoroutine(OpenRoutine());
        }
    }
    private IEnumerator OpenRoutine()
    {
        RectTransform rect = GetComponent<RectTransform>();
        Vector2 start = rect.anchoredPosition + Vector2.up * Screen.height;
        Vector2 end = rect.anchoredPosition;
        float openTime = 0.25f;

        for (float t = 0; t <= 1; t += Time.deltaTime / openTime)
        {
            rect.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }
        rect.anchoredPosition = end;

        _openRoutine = null;
    }
}
