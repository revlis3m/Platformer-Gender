using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectiblesCount : MonoBehaviour
{
    TextMeshProUGUI text;
    int count = 0;

    void Start() => UpdateCount();

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void OnEnable() => CollectibleBehaviour.OnCollected += OnCollectibleCollected;
    void OnDiseable() => CollectibleBehaviour.OnCollected -= OnCollectibleCollected;

    void OnCollectibleCollected()
    {
        count++;
        UpdateCount();
    }

    void UpdateCount()
    {
        text.text = $"{count}/{CollectibleBehaviour.total}";
    }
}
