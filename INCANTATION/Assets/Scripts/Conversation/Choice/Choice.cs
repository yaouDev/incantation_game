using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Choice : MonoBehaviour
{
    [SerializeField] private Text[] slots;
    public Color selectionColor;
    public Color unselectedColor;

    private int selectedIndex;
    private float selectionTimer;
    private float selectionInterval = 0.25f;

    private DialogManager dm;

    private void Start()
    {
        dm = DialogManager.instance;
    }

    public void SetChoice(string[] str)
    {
        selectedIndex = 0;
        selectionTimer = selectionInterval;
        RefreshColor();

        for(int i = 0; i < str.Length; i++)
        {
            slots[i].text = str[i];
        }
    }

    private void Update()
    {
        if(selectionTimer > 0f)
        {
            selectionTimer -= Time.deltaTime;
        }

        if (dm.isChoosing && selectionTimer <= 0f)
        {
            //selection
            if (Input.GetAxis("Horizontal") > 0)
            {
                //right
                if (selectedIndex < slots.Length - 1 && slots[selectedIndex + 1].text != "")
                {
                    selectedIndex++;
                    RefreshColor();
                    selectionTimer = selectionInterval;
                }
            }
            else if (Input.GetAxis("Horizontal") < 0)
            {
                //left
                if (selectedIndex > 0)
                {
                    selectedIndex--;
                    RefreshColor();
                    selectionTimer = selectionInterval;
                }
            }

            if (Input.GetButtonDown("Interact"))
            {
                dm.Choose(selectedIndex);
            }

            //print("choosing");
        }
    }

    private void RefreshColor()
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if(i == selectedIndex)
            {
                slots[i].color = selectionColor;
            }
            else
            {
                slots[i].color = unselectedColor;
            }
        }
    }
}
