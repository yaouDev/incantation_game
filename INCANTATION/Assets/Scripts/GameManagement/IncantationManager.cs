using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncantationManager : MonoBehaviour
{
    public static IncantationManager instance;

    public List<Incantation> startingIncantations = new List<Incantation>();

    //Same thing basically VVV
    public List<Incantation> allIncantations = new List<Incantation>();

    public Dictionary<string, Incantation> unlockedIncantations = new Dictionary<string, Incantation>();
    public Dictionary<string, Incantation> equipmentIncantations = new Dictionary<string, Incantation>();

    private Message message = new Message();
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;

        foreach (Incantation i in startingIncantations)
        {
            if (!allIncantations.Contains(i))
            {
                allIncantations.Add(i);
            }

            AddIncantation(i);
        }

        //for dev use?
        /*
        foreach (Incantation i in allIncantations)
        {
            AddIncantation(i);
        }*/
    }

    private void Awake()
    {
        #region Singleton

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("More than one instance of Incantation exists");
        }

        #endregion
    }

    public void checkMessages()
    {
        message = gameManager.GetLatestMessage();
        print(message.text);

        bool wasFound = FindTrigger(message.text);

        if (message.messageType == Message.MessageType.playerInput)
        {
            if (wasFound)
            {
                if (unlockedIncantations.ContainsKey(message.text) || equipmentIncantations.ContainsKey(message.text))
                {
                    ExecuteIncantation(message.text);
                }
                else
                {
                    Debug.Log("Not unlocked yet");
                }
            }
            else
            {
                Debug.Log("Not an incantation");
            }
        }
    }

    //ADD/REMOVE INCANTATION
    public void AddIncantation(Incantation incantation)
    {
        if (unlockedIncantations.ContainsKey(incantation.trigger))
        {
            Debug.Log("You already know that incantation");
            return;
        }

        if (FindTrigger(incantation.trigger))
        {
            unlockedIncantations.Add(incantation.trigger, incantation);
            Debug.Log("Added " + incantation.name.GetLocalizedString());
        }
        else
        {
            Debug.Log("No such incantation in database");
        }

        GameManager.instance.TextPopUp(incantation.name.GetLocalizedString());
    }

    public void AddEquipmentIncantation(string trigger)
    {
        foreach (Incantation i in allIncantations)
        {
            if (i.trigger == trigger)
            {
                equipmentIncantations.Add(i.trigger, i);
                return;
            }
        }

        Debug.Log("Incantation not found");
    }

    public void RemoveIncantation(Incantation incantation)
    {
        if (unlockedIncantations.ContainsKey(incantation.trigger))
        {
            unlockedIncantations.Remove(incantation.trigger);
            Debug.Log("Removed " + incantation);
        }
        else
        {
            Debug.Log("Tried to remove locked or non-existent incantation");
        }
    }

    public void RemoveEquipmentIncantation(string trigger)
    {
        foreach (KeyValuePair<string, Incantation> entry in equipmentIncantations)
        {
            if (entry.Key == trigger)
            {
                equipmentIncantations.Remove(entry.Key);
                return;
            }
        }

        Debug.Log("Incantation not found");
    }

    private void ExecuteIncantation(string trigger)
    {
        if (unlockedIncantations.ContainsKey(trigger) || equipmentIncantations.ContainsKey(trigger))
        {
            if (unlockedIncantations.TryGetValue(trigger, out Incantation unlocked))
            {
                unlocked.Cast();
            }
            else if(equipmentIncantations.TryGetValue(trigger, out Incantation equipment))
            {
                equipment.Cast();
            }
        }
        else if (!unlockedIncantations.ContainsKey(trigger) && FindTrigger(trigger))
        {
            //Push to chatPanel?
            Debug.Log("Not unlocked yet!");
        }
        else
        {
            Debug.LogWarning("Tried to execute incantation not currently in the database");
        }
    }

    public Dictionary<string, Incantation> GetUnlockedIncantations()
    {
        return unlockedIncantations;
    }

    public Dictionary<string, Incantation> GetEquipmentIncantations()
    {
        return equipmentIncantations;
    }

    public List<Incantation> GetAllIncantations()
    {
        List<Incantation> fetching = new List<Incantation>();
        allIncantations.ForEach(fetching.Add);
        IncantationComparator ic = new IncantationComparator();
        fetching.Sort(ic);
        return fetching;
    }

    public List<Incantation> GetAllIncantations(int forceLength)
    {
        List<Incantation> fetching = GetAllIncantations();

        print("forceLength: " + forceLength);

        while(fetching.Count < forceLength)
        {
            fetching.Add(null);
        }

        return fetching;
    }

    public bool FindTrigger(string str)
    {
        foreach (Incantation i in allIncantations)
        {
            if (str == i.trigger)
            {
                return true;
            }
        }

        return false;
    }

    public List<string> GetUnlockedTriggers()
    {
        List<string> result = new List<string>(unlockedIncantations.Keys);
        result.AddRange(equipmentIncantations.Keys);

        return result;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, 5f);
    }
}
