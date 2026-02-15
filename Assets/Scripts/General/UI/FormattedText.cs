using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FormattedText : MonoBehaviour
{
    [SerializeField] string format = "{0}";
    [SerializeField] TMP_Text text;

    public void SetValue(float value)
    {
        text.text = string.Format(format, value);
    }
    public void SetValues(params object[] values)
    {
        text.text = string.Format(format, values);
    }
}
