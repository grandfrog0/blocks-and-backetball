using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCinemachine : MonoBehaviour
{
    [SerializeField] List<Transform> states;
    [SerializeField] float footageTime = 0.25f;
    private Coroutine _stateRoutine;

    public void SetState(int index)
    {
        if (_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
        }
        _stateRoutine = StartCoroutine(StateRoutine(index));
    }

    private IEnumerator StateRoutine(int index)
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = states[index].position;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = states[index].rotation;

        for (float t = 0; t <= 1; t += Time.deltaTime / footageTime)
        {
            transform.position = Vector3.Slerp(startPos, endPos, t);
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }
    }
}
