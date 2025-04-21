using UnityEngine;
using System;

public class DangerZone : MonoBehaviour
{
    public Renderer zoneRenderer;
    private float timer, duration;
    private Action onCharged;

    void Awake()
    {
        if (zoneRenderer == null)
            zoneRenderer = GetComponent<Renderer>();

        if (GetComponent<BoxCollider>() == null)
            gameObject.AddComponent<BoxCollider>().isTrigger = true;
    }

    public void StartCharging(float timeToCharge, Action onComplete)
    {
        timer = 0f;
        duration = timeToCharge;
        onCharged = onComplete;
        if (zoneRenderer != null)
            zoneRenderer.material.color = Color.white;
        Debug.Log("DangerZone: Charging start");
    }

    void Update()
    {
        if (onCharged == null) return;

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / duration);
        if (zoneRenderer != null)
            zoneRenderer.material.color = Color.Lerp(Color.white, Color.red, t);

        if (timer >= duration)
        {
            onCharged.Invoke();
            onCharged = null;
            Debug.Log("DangerZone: Charging complete");
        }
    }
}