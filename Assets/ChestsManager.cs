using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestsManager : MonoBehaviour
{

    public List<Chest> chests;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            chests.Add(transform.GetChild(i).GetComponent<Chest>());
            chests[i]._manager = this;
        }
    }

    public void CloseAllChests()
    {
        foreach (Chest chest in chests)
        {
            if (chest.isOpen)
            {
                chest.CloseChest();
            }
        }
    }



}
