using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeechWindow : MonoBehaviour
{
    public GameObject Speaker;
    public int Timer;
    public string Text { get => text.text; set => text.text = value; }
    [SerializeField] public TMP_Text text;

    public void SetVisible()
    {
        GetComponent<Image>().enabled = true;
        text.gameObject.SetActive(true);
    }
}
