using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Deserialized : MonoBehaviour
{

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
}