using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CheatManager : MonoBehaviour {

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(kcode))
                    Debug.Log("KeyCode down: " + kcode);
            }
        }
    }
}