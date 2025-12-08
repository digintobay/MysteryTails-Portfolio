using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FinderSystem : MonoBehaviour
{

    public event EventHandler OnFinderPressed; 


    public GameObject pointFinderObjectPanel;
    public GameObject finderObject;


 
    public void Test_OnSpacePressed(object sender, EventArgs eventArgs)
    {
        Debug.Log("커서 변경 활성화");
    }

    public void FinderOnOff()
    {
        pointFinderObjectPanel.SetActive(!pointFinderObjectPanel.activeSelf);
        finderObject.SetActive(!finderObject.activeSelf);

        OnFinderPressed(this, EventArgs.Empty);
    }


}
