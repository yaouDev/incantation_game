using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncantationRegister : MonoBehaviour
{
    public IncantationRegisterEntry[] entries;
    public int entriesPerPage = 18;
    public int currentPage { get; private set; }

    private IncantationManager im;

    private void Awake()
    {
        im = IncantationManager.instance;

        currentPage = 1;
        LoadPage();
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

    private void LoadPage()
    {
        int startIndex = entriesPerPage * (currentPage - 1);
        Incantation[] incantations = im.GetAllIncantations(startIndex + entriesPerPage).GetRange(startIndex, startIndex + entriesPerPage).ToArray();

        for (int i = 0; i < entries.Length; i++)
        {
            if(incantations[i] == null)
            {
                entries[i].Set("Empty", "Empty", "Empty", null, true);
                return;
            }

            Incantation entry = incantations[i];
            bool isLocked = true;
            if(im.GetUnlockedIncantations().ContainsValue(entry) || im.GetEquipmentIncantations().ContainsValue(entry))
            {
                isLocked = false;
            }
            entries[i].Set(entry.name.GetLocalizedString(), entry.description.GetLocalizedString(), entry.trigger, entry.faction, isLocked);
            print(entries[i].lockedState.activeSelf);
        }

        Debug.Log("current page: " + currentPage);
    }
}
