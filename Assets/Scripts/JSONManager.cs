using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
//using System.Diagnostics;

public class JSONManager: MonoBehaviour
{
     public string fileName = "WorkoutInfoJSONAssignment.json";
     public TMP_Text titleText;
     public  GameObject buttonPrefab;
    public Transform buttonParent;
    public GameObject ballPrefab;

    private Root jsonData;
    private List<GameObject> workoutButtons = new List<GameObject>();

     void Start()
    {
        LoadJson();
        if (jsonData != null)
        {
            LoadJson();
            Display();
        }
    }

     void LoadJson()
    {
        string path = Path.Combine(Application.streamingAssetsPath, fileName);  
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            jsonData = JsonUtility.FromJson<Root>(json);
            Debug.Log("JSON Data Loaded: " + jsonData.ProjectName);
        }
        else
        {
            Debug.LogError("JSON file not found!");
        }
    }

    void Display()
    {
        if (jsonData != null)
        {
            titleText.text = jsonData.ProjectName;
            //UnityEngine.Debug.Log(titleText);

            float buttonSpacing = 60.0f;
            float startY = 100.0f;

            for (int i = 0; i < jsonData.workoutInfo.Count; i++)
            {
                var workout = jsonData.workoutInfo[i];
                GameObject buttons = Instantiate(buttonPrefab, buttonParent);
                TMP_Text buttonText = buttons.GetComponentInChildren<TMP_Text>();
                buttonText.text = workout.workoutName + "\n" + workout.description;

                RectTransform buttonRect = buttons.GetComponent<RectTransform>();
                buttonRect.anchoredPosition = new Vector2(0, startY - (i * buttonSpacing));

                int index = i; 
                Button button = buttons.GetComponent<Button>();
                button.onClick.AddListener(() => CreateButtons(index));

                workoutButtons.Add(buttons);
            }
        }
    }


    void CreateButtons(int index)
    {
        var workout = jsonData.workoutInfo[index];

        // Hide all buttons
        foreach (var btn in workoutButtons)
        {
            btn.SetActive(false);
        }

        SpawnBalls(workout.workoutDetails);

        // Delay showing buttons again
        Invoke("ShowButtons", 2.0f);
    }

    void SpawnBalls(List<WorkoutDetail> workoutDetails)
    {
        foreach (var detail in workoutDetails)
        {
            GameObject ball = Instantiate(ballPrefab);
            ball.transform.position = new Vector3(GetBallDirection(detail.ballDirection), 1, 0);
            Rigidbody rb = ball.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(0, 0, detail.speed);
        }
    }

    float GetBallDirection(float ballDirection)
    {
        if (ballDirection == 0.5f)
        {
            return 0.5f;
        }
        else if (ballDirection == 0f)
        {
            return 0f;
        }
        else if (ballDirection == -0.5f)
        {
            return -0.5f;
        }
        return 0f; 
    }

    void ShowButtons()
    {
        foreach (var btn in workoutButtons)
        {
            btn.SetActive(true);
        }
    }
}
[System.Serializable]
public class Root
{
    public string ProjectName;
    public int numberOfWorkoutBalls;
    public List<WorkoutInfo> workoutInfo;
}

[System.Serializable]
public class WorkoutDetail
{
    public int ballId;
    public float speed;
    public float ballDirection;
}

[System.Serializable]
public class WorkoutInfo
{
    public int workoutID;
    public string workoutName;
    public string description;
    public string ballType;
    public List<WorkoutDetail> workoutDetails;
}
