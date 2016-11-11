using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

    [SerializeField] TwoDCameraFollower cameraFollower;
    [SerializeField] LevelContainer levelContainer;
    [SerializeField] CharacterControl characterPrefab;

    void Awake()
    {
        cameraFollower.enabled = false;
    }

    void Start()
    {
        SpawnCharacterOnMap();
    }

    void SpawnCharacterOnMap()
    {
        CharacterControl character = Object.Instantiate<CharacterControl>(characterPrefab);
        character.onConsumableAmountUpdated += OnConsumableAmountUpdated;
        Transform spawnPoint = levelContainer.spawnPoints[Random.Range(0, levelContainer.spawnPoints.Length-1)].transform;
        character.gameObject.transform.SetParent(levelContainer.transform, false);
        character.gameObject.transform.position = spawnPoint.position;
        cameraFollower.target = character.gameObject.transform;
        cameraFollower.enabled = true;
    }

    void OnConsumableAmountUpdated(ConsumableController.ConsumableType type, int amount)
    {
        Debug.LogFormat("New Amount: {0} - {1}", type, amount);
    }
}
