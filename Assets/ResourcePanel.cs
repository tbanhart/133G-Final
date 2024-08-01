using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePanel : MonoBehaviour
{
    [SerializeField] TMP_Text ResourceText;
    [SerializeField] string ResourceName;
    [SerializeField] ItemType DisplayedItem;

    public void SetResource(int amount)
    {
        ResourceText.text = ResourceName + ": " + amount;
    }
}
