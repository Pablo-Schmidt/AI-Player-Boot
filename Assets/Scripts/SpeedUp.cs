using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.L))
        {
            Time.timeScale = 10;
        }
        if (Input.GetKeyUp(KeyCode.N))
        {
            Time.timeScale = 1;
        }
    }
}
