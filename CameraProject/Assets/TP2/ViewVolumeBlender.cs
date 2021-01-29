using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVolumeBlender : MonoBehaviour
{
    private List<AViewVolume> activeViewVolumes = new List<AViewVolume>();
    private Dictionary<AView, List<AViewVolume>> volumesPerView = new Dictionary<AView, List<AViewVolume>>();

    #region Singleton

    private static ViewVolumeBlender instance;

    public static ViewVolumeBlender Instance
    {
        get => instance;
    }

    #endregion
    private void Awake()
    {
        if (instance)
            Destroy(gameObject);
        else
            instance = this;
    }

    private void Update()
    {
        int maxPrio = 0;
        float weightSum = 0;

        foreach (var volume in activeViewVolumes)
        {
            volume.view.weight = 0;
            if (volume.priority > maxPrio)
            {
                maxPrio = volume.priority;
                weightSum = volume.ComputeSelfWeight();
            }
            else if (volume.priority == maxPrio)
                weightSum += volume.ComputeSelfWeight();
        }

        foreach (var volume in activeViewVolumes)
        {
            if (volume.priority == maxPrio)
                volume.view.weight = Mathf.Max(volume.view.weight, volume.ComputeSelfWeight() / weightSum);
        }
    }

    public void AddVolume(AViewVolume volume)
    {
        activeViewVolumes.Add(volume);
        if (!volumesPerView.ContainsKey(volume.view))
        {
            volumesPerView.Add(volume.view, new List<AViewVolume>());
            volume.view.SetActive(true);
        }

        volumesPerView[volume.view].Add(volume);

    }

    public void RemoveVolume(AViewVolume volume)
    {
        activeViewVolumes.Remove(volume);

        if (volumesPerView.ContainsKey(volume.view))
        {
            volumesPerView[volume.view].Remove(volume);

            if (volumesPerView[volume.view].Count == 0)
            {
                volumesPerView.Remove(volume.view);
                volume.view.SetActive(false);
            }
        }
    }

    void OnGUI()
    {
        Rect rect = new Rect(0, 0, 300, 50);
        foreach (var volume in activeViewVolumes)
        {
            GUI.color = Color.red;
            GUI.Label(rect, volume.name);
            rect.position += Vector2.up * 50;
        }
    }
}
