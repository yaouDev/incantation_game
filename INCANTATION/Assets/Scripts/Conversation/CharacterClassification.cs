using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClassification : MonoBehaviour
{
    new public string name;
    public List<AudioClip> voiceLines = new List<AudioClip>();

    public AudioClip GetVoiceline()
    {
        if(voiceLines.Count > 0)
        {
            return voiceLines[Random.Range(0, voiceLines.Count)];
        }
        else
        {
            Debug.LogWarning("Returned null voiceline!");
            return null;
        }
    }
}
