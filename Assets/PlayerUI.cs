using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] ResourcePanel rockPanel;
    [SerializeField] ResourcePanel leafPanel;
    [SerializeField] ResourcePanel flagPanel;

    public Dictionary<ItemType, ResourcePanel> itemPanels = new Dictionary<ItemType, ResourcePanel>();

    private void Awake()
    {
        itemPanels = new Dictionary<ItemType, ResourcePanel>
        {
            { ItemType.LEAF, leafPanel },
            { ItemType.FLAG, flagPanel },
            { ItemType.ROCK, rockPanel }
        };
    }

    public void SetResource(ItemType type, int amount)
    {
        itemPanels[type].SetResource(amount);
    }
}
