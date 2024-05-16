using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is for debug
public class SwapParts : MonoBehaviour
{
    [SerializeField] private BodyPartsSelector bodyPartsSelector;

    [SerializeField] private BodyPartsManager bodyPartsManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwapPart(2);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SwapPart(3);
        }
    }

    private void SwapPart(int part)
    {
        bodyPartsSelector.NextBodyPart(part);
        bodyPartsManager.UpdateBodyParts();
        AudioManager.instance.PlayFlatSound(AudioManager.instance.audioClips[0]);
    }
}
