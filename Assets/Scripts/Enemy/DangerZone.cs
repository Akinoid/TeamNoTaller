using UnityEngine;
using System;

public class DangerZone : MonoBehaviour
{
    public Renderer renderer;
    private float timer;
    private float duration;
    private Action onCharged;

    public void StartCharging(float timeToCharge, Action onComplete)
    {
        timer = 0f;
        duration = timeToCharge;
        onCharged = onComplete;
    }

    void Update()
    {
        if (onCharged == null) return;

        timer += Time.deltaTime;
        float t = timer / duration;
        renderer.material.color = Color.Lerp(Color.white, Color.red, t);

        if (timer >= duration)
        {
            onCharged.Invoke();
            onCharged = null;
        }
    }
}
