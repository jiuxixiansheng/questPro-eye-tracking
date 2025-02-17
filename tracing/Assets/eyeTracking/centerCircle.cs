using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerCircle : MonoBehaviour
{
    public Laser laser1;
    public Laser2 laser2;
    public GameObject circle;
    public Camera main_camera;
    private Vector4 hit1;
    private Vector4 hit2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hit1 = laser1.HitPos;
        hit2 = laser2.HitPos_right;
        if (hit1.w == 1 && hit2.w == 1)
        {
            Vector3 pos = (hit1+hit2)/2;
            circle.transform.position = pos;
            circle.transform.LookAt(main_camera.transform.position);
            circle.SetActive(true);
        }
        else { circle.SetActive(false); }
    }
}
