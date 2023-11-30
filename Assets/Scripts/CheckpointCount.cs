using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheckpointCount : MonoBehaviour
{
    TextMeshProUGUI text;
    int count = 0;

    void Start() => UpdateCount();

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() => CheckpointBehaviour.OnCollected += OnCollectibleCollected;
    void OnDiseable() => CheckpointBehaviour.OnCollected -= OnCollectibleCollected;

    void OnCollectibleCollected()
    {
        count++;
        UpdateCount();
    }

    void UpdateCount()
    {
        text.text = $"{count}/{CheckpointBehaviour.total}";
    }
}
