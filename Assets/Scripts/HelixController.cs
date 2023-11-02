using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelixController : MonoBehaviour
{
    private Vector2 lastTapPos;
    private Vector3 startRotation;

    public Transform topTransform;
    public Transform goalTransform;
    public GameObject helixLevelPrefab;

    public List<Stage> allStages = new List<Stage>();

    private float helixDistance;
    private List<GameObject> spawnedLevels = new List<GameObject>();

    private void Awake()
    {
        startRotation = transform.localEulerAngles;
        helixDistance = topTransform.localPosition.y - (goalTransform.localPosition.y + 0.1f);
        LoadStage(0);
    }

    private void Update()
    {
        HandleRotationInput();
    }

    private void HandleRotationInput()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 curTapPos = Input.mousePosition;

            if (lastTapPos == Vector2.zero)
            {
                lastTapPos = curTapPos;
            }

            float delta = lastTapPos.x - curTapPos.x;
            lastTapPos = curTapPos;

            transform.Rotate(Vector3.up * delta);
        }

        if (Input.GetMouseButtonUp(0))
        {
            lastTapPos = Vector2.zero;
        }
    }

    public void LoadStage(int stageNumber)
    {
        if (stageNumber < 0 || stageNumber >= allStages.Count)
        {
            Debug.LogError("Invalid stage number: " + stageNumber);
            return;
        }

        Stage stage = allStages[stageNumber];

        Camera.main.backgroundColor = stage.stageBackgroundColor;

        BallController ballController = FindObjectOfType<BallController>();
        if (ballController != null)
        {
            ballController.GetComponent<Renderer>().material.color = stage.stageBallColor;
        }

        transform.localEulerAngles = startRotation;
        DestroyOldLevels();
        CreateNewLevels(stage);
    }

    private void DestroyOldLevels()
    {
        foreach (GameObject go in spawnedLevels)
        {
            Destroy(go);
        }
        spawnedLevels.Clear();
    }

    private void CreateNewLevels(Stage stage)
    {
        float levelDistance = helixDistance / stage.levels.Count;
        float spawnPosY = topTransform.localPosition.y;

        for (int i = 0; i < stage.levels.Count; i++)
        {
            spawnPosY -= levelDistance;
            GameObject level = Instantiate(helixLevelPrefab, transform);
            level.transform.localPosition = new Vector3(0, spawnPosY, 0);
            spawnedLevels.Add(level);

            CreateGapsAndDeathParts(stage, level);
        }
    }

    private void CreateGapsAndDeathParts(Stage stage, GameObject level)
    {
        Transform[] levelParts = level.GetComponentsInChildren<Transform>();
        List<Transform> leftParts = new List<Transform>(levelParts);

        // Remove the parent object
        leftParts.RemoveAt(0);

        int partsToDisable = 12 - stage.levels[levelParts.Length - 1].partCount;

        while (partsToDisable > 0)
        {
            int randomIndex = Random.Range(0, leftParts.Count);
            leftParts[randomIndex].gameObject.SetActive(false);
            leftParts.RemoveAt(randomIndex);
            partsToDisable--;
        }

        List<Transform> deathParts = new List<Transform>(leftParts);
        int deathPartCount = stage.levels[levelParts.Length - 1].deathPartCount;

        while (deathPartCount > 0 && deathParts.Count > 0)
        {
            int randomIndex = Random.Range(0, deathParts.Count);
            deathParts[randomIndex].gameObject.AddComponent<DeathPart>();
            deathParts.RemoveAt(randomIndex);
            deathPartCount--;
        }
    }
}
