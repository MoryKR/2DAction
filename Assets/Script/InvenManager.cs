using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvenManager : MonoBehaviour
{
    public GameObject CharInfo;
    public GameObject Training;
    public GameObject Shop;
    
    void Start()
    {
        CharInfo.SetActive(true);
        Training.SetActive(false);
        Shop.SetActive(false);
    }

    
    void Update()
    {
        
    }

    public void OnClickChar()
    {
        CharInfo.SetActive(true);
        Training.SetActive(false);
        Shop.SetActive(false);
    }
    public void OnClickTraining()
    {
        CharInfo.SetActive(false);
        Training.SetActive(true);
        Shop.SetActive(false);
    }
    public void OnClickShop()
    {
        CharInfo.SetActive(false);
        Training.SetActive(false);
        Shop.SetActive(true);
    }

}
