using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideUI : MonoBehaviour
{
    public GameObject CanvasObject;
    bool isActive = true;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isActive)
        {
            CanvasObject.SetActive(false);
            isActive = false;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !isActive)
        {
            CanvasObject.SetActive(true);
            isActive = true;
        }
    }
}