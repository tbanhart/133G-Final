using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneStart : MonoBehaviour
{
    private void Awake()
    {
        GameObject.Find("Scene Transition").GetComponent<SceneTransition>().DoStartUp();
    }
}
