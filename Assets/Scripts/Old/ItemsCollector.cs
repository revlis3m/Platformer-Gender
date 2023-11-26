using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemsCollector : MonoBehaviour
{
    private int orangeCount = 0;
    [SerializeField] private TextMeshProUGUI orangeCounter;
    [SerializeField] private AudioSource collectionSoundEffect;

    private void OnTriggerEnter2D(Collider2D collectibles)
    {
        if (collectibles.gameObject.CompareTag("Orange"))
        {
            collectionSoundEffect.Play();
            OrangeDestruction orange = collectibles.gameObject.GetComponent<OrangeDestruction>();
            orange.PlayDeath();
            orangeCount++;
            orangeCounter.text = $"Oranges : {orangeCount}";
        }
    }
}
