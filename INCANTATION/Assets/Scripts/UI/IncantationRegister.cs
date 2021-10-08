using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncantationRegister : MonoBehaviour
{
    public IncantationRegisterEntry[] entries;
    public int entriesPerPage = 18;

    public Text pageCounter;

    public int currentPage { get; private set; }

    private IncantationManager im;

    private void Awake()
    {
        im = IncantationManager.instance;

        currentPage = 1;
        //LoadPage();
    }

    public void NextPage()
    {
        if(im.GetAllIncantations().Count > entriesPerPage * currentPage)
        {
            currentPage++;
            LoadPage();
        }
    }

    public void PreviousPage()
    {
        if(currentPage > 1)
        {
            currentPage--;
            LoadPage();
        }
    }

    public void LoadPage()
    {
        int startIndex = entriesPerPage * (currentPage - 1);
        Incantation[] incantations = im.GetAllIncantations(startIndex + entriesPerPage).GetRange(startIndex, startIndex + entriesPerPage).ToArray();

        pageCounter.text = currentPage.ToString();

        Debug.Log("current page: " + currentPage);

        for (int i = 0; i < entries.Length; i++)
        {
            if(incantations[i] == null)
            {
                entries[i].Set("TEST", "", "", null, true);
                return;
            }

            Incantation entry = incantations[i];
            bool isLocked = true;
            if(im.GetUnlockedIncantations().ContainsValue(entry) || im.GetEquipmentIncantations().ContainsValue(entry))
            {
                isLocked = false;
            }
            entries[i].Set(entry.name.GetLocalizedString(), entry.description.GetLocalizedString(), entry.trigger, entry.faction, isLocked);
        }
    }
}
