using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventory : MonoBehaviour
{        
    [SerializeField] Image inventoryImage;
    public bool isInventoryOn = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            switch (isInventoryOn)
            {
                case true:
                    inventoryImage.gameObject.SetActive(false);
                    isInventoryOn = false;
                    Debug.Log("inventroy ²ô±â");
                    break;
                case false:
                    inventoryImage.gameObject.SetActive(true);
                    isInventoryOn = true;
                    break;
            }
        }

    }
}
