using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HSTable : MonoBehaviour
{

    private Transform entryContainer;
    private Transform entryTemplate;
    public GameObject HST;

    private List<Transform> highscoreEntryTransformList;
    public Text HighscoreN;
    public string highscores;
    public Text Names;
    private Highscorelist highscoreEntryList;
    private GameManager_V2 GM;
    private NameSaver NS;
    public int MaxHighscorecount = 5;
    public int MINHighscorecount = 1;

    private void Awake()
    {
        GM = FindObjectOfType<GameManager_V2>();
        NS = FindObjectOfType<NameSaver>();

        entryContainer = transform.Find("HSContainer");
        entryTemplate = entryContainer.Find("HSTemplate");

        if (highscoreEntryList == null)
        {

            Debug.Log("Initializing table with default values...");
            AddHighscoreEntry(125, "CAT");
            AddHighscoreEntry(300, "DOG");
            AddHighscoreEntry(4000, "LAD");
            AddHighscoreEntry(1250, "BAT");
            AddHighscoreEntry(6000, "WII");
            highscoreEntryList = new Highscorelist();
        }

        if (SaveSystem.LoadScore() == null)
        {
            //Do nothing lol, there is no score to load
        }
        else
        {
            highscoreEntryList = new Highscorelist();

            TempHSData tempData = FindObjectOfType<TempHSData>();

            tempData.highscores = SaveSystem.LoadScore().highscores;
            foreach (KeyValuePair<string, int> pair in SaveSystem.LoadScore().highscores)
            {
                HighscoreEntry HS = new HighscoreEntry(pair.Value, pair.Key);
                highscoreEntryList.HighscoreEntries.Add(HS);
            }
        }

        RefreshScores();
    }

    public void NewScore(string name, int value)
    {
        if (name != null)
        {
            HighscoreEntry HS = new HighscoreEntry(value, name);
            highscoreEntryList.HighscoreEntries.Add(HS);
            SortHighScores();

            highscoreEntryTransformList.Clear();

            foreach (Transform child in entryContainer)
            {
                if (child.gameObject.tag == "Score")
                { GameObject.Destroy(child.gameObject); }
            }
        }
        else
        {
            HighscoreEntry HS = new HighscoreEntry(GameManager_V2.scoreValue, NS.PlayerName);
            highscoreEntryList.HighscoreEntries.Add(HS);
            SortHighScores();
            if (highscoreEntryList.HighscoreEntries.Count >= MaxHighscorecount)
            {
                highscoreEntryList.HighscoreEntries.RemoveRange(MaxHighscorecount, highscoreEntryList.HighscoreEntries.Count - MINHighscorecount);
            }

            highscoreEntryTransformList.Clear();

            foreach (Transform child in entryContainer)
            {
                if (child.gameObject.tag == "Score")
                { GameObject.Destroy(child.gameObject); }
            }
        }
        RefreshScores();
    }

    public void RefreshScores()
    {
        SortHighScores();

        if (highscoreEntryTransformList != null)
        {
            foreach (Transform highscoreEntryTransform in highscoreEntryTransformList)
            {
                Destroy(highscoreEntryTransform.gameObject);
            }
        }

        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList.HighscoreEntries)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }

    public void SortHighScores()
    {
        for (int i = 0; i < highscoreEntryList.HighscoreEntries.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.HighscoreEntries.Count; j++)
            {
                if (highscoreEntryList.HighscoreEntries[j].score > highscoreEntryList.HighscoreEntries[i].score)
                {

                    HighscoreEntry tmp = highscoreEntryList.HighscoreEntries[i];
                    highscoreEntryList.HighscoreEntries[i] = highscoreEntryList.HighscoreEntries[j];
                    highscoreEntryList.HighscoreEntries[j] = tmp;
                }
            }
        }

        TempHSData temp = FindObjectOfType<TempHSData>();

        if (highscoreEntryList.HighscoreEntries.Count >= MaxHighscorecount)
        {
            for (int i = 6; i < highscoreEntryList.HighscoreEntries.Count; i++)
            {
                temp.highscores.Remove(highscoreEntryList.HighscoreEntries[i].name.ToString());
                highscoreEntryList.HighscoreEntries.RemoveAt(i);
            }
        }
    }

    public void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformList)
    {
        float templateHeight = 31f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        entryTransform.gameObject.tag = "Score"; 
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);

        int score = highscoreEntry.score;

        entryTransform.Find("scoreText").GetComponent<Text>().text = score.ToString();

        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<Text>().text = name;

        transformList.Add(entryTransform);
        
    }

    public void AddHighscoreEntry(int score, string name)
    {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry(score, name);

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        highscoreEntryList = JsonUtility.FromJson<Highscorelist>(jsonString);

        if (highscoreEntryList == null)
        {
            highscoreEntryList = new Highscorelist();
            highscoreEntryList.HighscoreEntries = new List<HighscoreEntry>();
        }
        // Add new entry to Highscores
        highscoreEntryList.HighscoreEntries.Add(highscoreEntry);


        // Save updated Highscores
        string json = JsonUtility.ToJson(highscoreEntryList);
        
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        RefreshScores();
    }


    public void ClearHighScoreList()
    {
        for (int i = 0; i < highscoreEntryList.HighscoreEntries.Count; i++)
        {
            highscoreEntryList.HighscoreEntries.RemoveAt(i);
        }

        SortHighScores();
    }
    
}



[System.Serializable]
public class HighscoreEntry
{
    public int score;
    public string name;

    public HighscoreEntry()
    {

    }

    public HighscoreEntry(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
}

[System.Serializable]
public class Highscorelist
{
    public List<HighscoreEntry> HighscoreEntries;

   public Highscorelist()
    {
        HighscoreEntries = new List<HighscoreEntry>(); 
    }

}




