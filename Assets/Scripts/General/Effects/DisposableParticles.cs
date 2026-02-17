using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableParticles : MonoBehaviour
{
    public void Play()
    {
        gameObject.SetActive(true);
        transform.parent = transform.parent.parent;
        transform.localScale = Vector3.one;

        ParticleSystem particles = GetComponent<ParticleSystem>();
        particles.Play();
        StartCoroutine(DestroyRoutine(particles.main.duration));
    }

    private IEnumerator DestroyRoutine(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}
