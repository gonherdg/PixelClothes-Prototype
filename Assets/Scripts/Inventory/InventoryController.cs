using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] UI_InventoryPage inventoryPage;

    public int inventorySize = 10;

    private void Start()
    {
        inventoryPage.InitializeInventoryUI(inventorySize);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryPage.isActiveAndEnabled == false)
            {
                inventoryPage.Show();
            }
            else
            {
                inventoryPage.Hide();
            }

        }
    }
}
