using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public GameObject joystick;
    public GameObject suspect;
    public Text scalaText;
    // Start is called before the first frame update
    void Start()
    {
        suspect.GetComponent<CharacterController>();
        suspect.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        DebugMovement();
    }
    public void DebugMovement(){
        float horizontal = Input.GetAxisRaw("Horizontal");  //horizontal 입력 벡터값
        float vertical = Input.GetAxisRaw("Vertical");      //vertical 입력 벡터값
        Vector3 direction = new Vector3(horizontal, vertical,0f).normalized;
        joystick.transform.localPosition = Vector3.Slerp(joystick.transform.localPosition, direction * 50f, Time.deltaTime*5f);
        float scala = joystick.transform.localPosition.magnitude/50;
        scala = parseDot(scala);
        scalaText.text = scala.ToString();
    }
    private float parseDot(float val){
        var str = val.ToString("0.00");
        return float.Parse(str);
    }
}
