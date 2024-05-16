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
            bodyPartsSelector.NextBodyPart(2);
            bodyPartsManager.UpdateBodyParts();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            bodyPartsSelector.NextBodyPart(3);
            bodyPartsManager.UpdateBodyParts();
        }
    }
}
