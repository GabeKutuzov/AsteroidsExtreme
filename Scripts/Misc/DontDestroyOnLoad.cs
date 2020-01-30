using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour {

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


}