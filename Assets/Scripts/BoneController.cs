using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneController : MonoBehaviour
{
    [SerializeField] AudioClip sfxPickUp = null;
    static private bool bonusBonesRevealed = false;
    [SerializeField] private GameObject[] bonusBones;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!bonusBonesRevealed)
                RevealBonusBones();

            GameManager.instance.Play(sfxPickUp);
            GameManager.instance.AddAmmo(1, 1);
            Destroy(gameObject);
        }
    }

    void RevealBonusBones()
    {
        bonusBonesRevealed = true;

        foreach (var obj in bonusBones)
            obj.SetActive(true);

    }
}
