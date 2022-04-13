using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventController : MonoBehaviour
{
    public PlatformerController platformerController ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack1_End()
    {
        if(platformerController == null)
        {
            return;
        }

        platformerController.AttackEnd(1);
        Debug.Log("Attack1_End");
    }

    void Attack2_End()
    {
        if (platformerController == null)
        {
            return;
        }

        platformerController.AttackEnd(2);
        Debug.Log("Attack2_End");
    }
}
