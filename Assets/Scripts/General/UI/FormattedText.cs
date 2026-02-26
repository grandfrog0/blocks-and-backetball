using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FormattedText : MonoBehaviour
{
    public string format = "{0}";
    [SerializeField] TMP_Text text;

    public void SetValue(float value)
    {
        text.text = string.Format(format, value);
    }
    public void SetValue(int value)
    {
        text.text = string.Format(format, value);
    }
    public void SetValue(int value1, int value2)
    {
        text.text = string.Format(format, value1, value2);
    }
    public void SetValues(params object[] values)
    {
        text.text = string.Format(format, values);
    }
    public void Refresh()
    {
        text.text = format;
    }
}
