using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Camera camera;
    [SerializeField] ResourcePanel rockPanel;
    [SerializeField] ResourcePanel leafPanel;
    [SerializeField] ResourcePanel flagPanel;
    [SerializeField] GameObject SpeechPanelObject;
    [SerializeField] float DialogueDisplayTime;

    public Dictionary<ItemType, ResourcePanel> itemPanels = new Dictionary<ItemType, ResourcePanel>();

    public List<GameObject> DialogueBoxes;

    public Dictionary<GameObject, GameObject> Speakers = new Dictionary<GameObject, GameObject>();

    private void Awake()
    {
        itemPanels = new Dictionary<ItemType, ResourcePanel>
        {
            { ItemType.LEAF, leafPanel },
            { ItemType.FLAG, flagPanel },
            { ItemType.ROCK, rockPanel }
        };
        foreach(var item in itemPanels.Keys)
        {
            SetResource(item, 0);
        }
    }

    private void Update()
    {
        List<GameObject> deleteList = new List<GameObject>();

        foreach (var dialogueBox in DialogueBoxes) 
        {
            var speechWindow = dialogueBox.GetComponent<SpeechWindow>();
            if (speechWindow.Timer <= 0)
            {
                //Destroy(item.Key);
                deleteList.Add(dialogueBox);
                continue;
            }
            speechWindow.Timer--;
            
            var dispComp = dialogueBox.GetComponent<RectTransform>();
            var offset = new Vector3(0f, 1.5f + dispComp.sizeDelta.y, 0f);
            var targetPos = camera.WorldToScreenPoint(speechWindow.Speaker.transform.position) + offset;

            dispComp.position = targetPos;
        }
        foreach(var item in deleteList)
        {
            DialogueBoxes.Remove(item);
            Speakers.Remove(item);
            Destroy(item);
        }
    }

    public void SetResource(ItemType type, int amount)
    {
        itemPanels[type].SetResource(amount);
    }

    public void DisplayDialogue(string message, GameObject speaker)
    {
        if (Speakers.Values.Contains(speaker)) return;
        
        var obj = Instantiate(SpeechPanelObject);
        DialogueBoxes.Add(obj);
        Speakers.Add(obj, speaker);
        obj.GetComponent<SpeechWindow>().Speaker = speaker;
        obj.GetComponent<SpeechWindow>().Timer = (int)(DialogueDisplayTime / Time.deltaTime);
        obj.transform.SetParent(this.transform);
    }
}
