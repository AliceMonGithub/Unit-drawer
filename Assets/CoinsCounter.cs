using System;
using System.Collections;
using TMPro;
using UnityEngine;

internal class CoinsCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text _counter;

    private int _value;

    public void AddCoin(int value)
    {
        StopCoroutine(SmoothCounter(_value + value, _value, 2f));
        StartCoroutine(SmoothCounter(_value + value, _value, 2f));

        _value += value;
    }

    private IEnumerator SmoothCounter(int endValue, int startValue, float duration)
    {
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime / duration;

            _counter.text = Convert.ToInt32(Mathf.Lerp(startValue, endValue, time)).ToString();

            yield return new WaitForEndOfFrame();
        }

        _counter.text = endValue.ToString();
    }
}
