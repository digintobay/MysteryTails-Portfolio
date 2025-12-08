using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmitOnCheck : MonoBehaviour
{
    public static bool Submit = false;

    private void OnEnable()
    {
        Submit = true;  
    }

    private void OnDisable()
    {
        Submit = false;
    }
}
