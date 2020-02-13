using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    // Start is called before the first frame update

    public static float landSize;
    public static float foodAmount;
    public static bool Daytime = true;
    public static float dayTimeTimer = 10;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Daytime)
        {
            dayTimeTimer -= Time.deltaTime;
        }
        if (dayTimeTimer<= 0)
        {
            Daytime = false;
        }


    }
}
