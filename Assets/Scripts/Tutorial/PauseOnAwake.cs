using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseOnAwake : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0;
    }
}
