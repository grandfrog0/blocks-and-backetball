using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFade : MonoBehaviour
{
    private Coroutine _fadeRoutine;

    [SerializeField] Image image;
    [SerializeField] Color startColor = Color.white;
    [SerializeField] Color endColor = new Color(1, 1, 1, 0);
    [SerializeField] bool activeStateOnEnd = false;

    [SerializeField] float _waitTime = 1f;
    [SerializeField] float _fadeTime = 0.25f;

    public void Fade()
    {
        gameObject.SetActive(true);
        image ??= GetComponent<Image>();

        if (_fadeRoutine != null)
        {
            StopCoroutine(_fadeRoutine);
        }

        _fadeRoutine = StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        image.color = startColor;

        yield return new WaitForSeconds(_waitTime);

        for (float t = 0; t <= 1; t += Time.deltaTime / _fadeTime)
        {
            image.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        image.color = endColor;

        gameObject.SetActive(activeStateOnEnd);
    }
}
