using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UINoteInventory : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textItemCount;

    private int itemCount;


   public int ItemCount
    {
        set
        {
            itemCount = Mathf.Clamp(value, 0, 1);
            textItemCount.text = itemCount.ToString();
        }

    }



    private void Awake()
    {
        
    }
}
