using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.UI;

public class ModifierProcessor
{
    string line;
    public void ProcessModificationLine(string a_line)
    {
        Debug.Log("Modification line detected");
        GameObject.Find("DebugText").GetComponent<Text>().text = "Modification Line detected: " + a_line;
        line = a_line.ToLower();

        CheckFontMod();
    }

    void CheckFontMod()
    {
        if (line.Contains("fontsize="))
        {
            Debug.Log(line.IndexOf("fontsize="));
        }
    }
}

public class ScriptParser : MonoBehaviour
{
    List<string> scriptcontents = new List<string>();
    List<string> log = new List<string>();
    int scriptPosition;

    Text characterName;
    Text speakingText;

    string currentText;
    float elapsedTime;
    char modifierKey;
    ModifierProcessor modP;


    public void LoadScriptIntoMemory(string a_filepath)
    {
        ClearScript();
        StreamReader file = File.OpenText(a_filepath);
        scriptcontents = file.ReadToEnd().Split("\n"[0]).ToList();
        file.Close();
    }

    // Use this for initialization
    void Start()
    {
        modP = new ModifierProcessor();
        modifierKey = '#';
        elapsedTime = 0.0f;
        //LoadScriptIntoMemory("Assets/Scripts/testscript.txt");
        LoadScriptIntoMemory(Application.dataPath + "/Scripts/testscript.txt");
        characterName = transform.FindChild("TextPanel").FindChild("NameTag").FindChild("SpeakerName").GetComponent<Text>();
        speakingText = transform.FindChild("TextPanel").FindChild("SpeakerText").GetComponent<Text>();
        characterName.text = speakingText.text = "Null";
        ProcessTextLine();
    }

    // Update is called once per frame
    void Update()
    {
        if (ProcessUserInput())
            if (scriptPosition < scriptcontents.Count() - 1)
            {
                ProcessTextLine();
            }
            else
                Debug.Log("End of file reached");
        UpdateSpeakingText();
    }

    void ProcessTextLine()
    {

        string line = scriptcontents[scriptPosition];
        if (line[0] == modifierKey)
        {          
            line = scriptcontents[scriptPosition];
            if (line[0] == modifierKey)
                modP.ProcessModificationLine(line.Substring(1));
            scriptPosition++;
            return;
        }
        else if (line[0] != modifierKey)
        {
            // Extract name and text content
            string name;
            int nameEnd = GetNameEnd(line);
            name = line.Substring(0, nameEnd);
            line = line.Substring(nameEnd + 1, line.Length - nameEnd - 2);
            AddLineToLog(name, line);
            SetCharacterNameAndSpeechText(name, line);
        }

        speakingText.text = "";
        scriptPosition++;



    }

    int GetNameEnd(string a_line)
    {
        // indices in text where the name begins
        int nameEnd = -1;
        for (int i = 0; i < a_line.Length; i++)
        {
            if (a_line[i] == '-')
            {
                nameEnd = i;
                break;
            }
        }
        return nameEnd;
    }

    void UpdateSpeakingText()
    {
        // Animate the text in a typewriter fashion
        if (elapsedTime > 0.04f)
        {
            if (speakingText.text.Length != currentText.Length)
            {
                speakingText.text = currentText.Substring(0, speakingText.text.Length + 1);
                elapsedTime = 0.0f;
            }
        }
        elapsedTime += Time.deltaTime;
    }

    void SetCharacterNameAndSpeechText(string a_name, string a_text)
    {
        characterName.text = a_name;
        currentText = a_text;
    }

    bool ProcessUserInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // If player pressed a valid key and the speech text hasn't completely outputted a line,
            // then automatically finish the line
            if (speakingText.text != currentText)
            {
                speakingText.text = currentText;
                return false;
            }
            return true;
        }
        return false;
    }

    bool CheckEndOfScript()
    {
        if (scriptPosition >= scriptcontents.Count)
            return true;
        return false;
    }

    void ClearScript()
    {
        scriptPosition = 0;
        scriptcontents.Clear();
    }

    void AddLineToLog(string a_charName, string a_speechText)
    {
        //log.Add (a_charName + ": " + a_speechText);
        this.GetComponent<ShowHideLog>().UpdateLog(a_charName + ": " + a_speechText);
    }




}
