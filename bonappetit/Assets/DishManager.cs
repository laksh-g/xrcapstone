using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishManager : MonoBehaviour
{
    public Transform steakFrites;
    public Transform tableBread;
    public Transform onionSoup;
    public Transform chicken;
    public Transform crabCakes;
    // Start is called before the first frame update
    void Start()
    {
       steakFrites.gameObject.SetActive(true); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
