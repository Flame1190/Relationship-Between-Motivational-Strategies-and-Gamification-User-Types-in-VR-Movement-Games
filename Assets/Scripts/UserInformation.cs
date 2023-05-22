using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class UserInformation : MonoBehaviour
{

    // User height
    public static float height = 1.8f;
    // Max user reach (radius of a sphere)
    public static float reach = 0.5f;

    static UserInformation _instance;

    private void Awake()
    {
        if (_instance == null || _instance == this)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

}
