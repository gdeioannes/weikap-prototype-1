using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class TwoDCameraFollower : Camera2DFollow
{
    [SerializeField] LevelContainer levelContainer;
    Camera camera;

    void Awake()
    {
        camera = this.GetComponent<Camera>();
    }

    public Vector2 CameraSize
    {
        get
        {
            Vector2 toReturn = Vector2.one;
            toReturn.x = camera.orthographicSize * Screen.width / Screen.height;
            toReturn.y = camera.orthographicSize;
            return toReturn;
        }
    }

    override protected void Update()
    {
        base.Update();

        Vector3 cameraPosition = camera.transform.position;
        Vector3 levelPosition = levelContainer.transform.position;

        if (cameraPosition.x < (levelPosition.x + CameraSize.x))
        {
            cameraPosition.x = levelPosition.x + CameraSize.x;
        }

        if (cameraPosition.x > (levelPosition.x + levelContainer.Size.x - CameraSize.x))
        {
            cameraPosition.x = levelPosition.x + levelContainer.Size.x - CameraSize.x;
        }

        if (cameraPosition.y < levelPosition.y + CameraSize.y)
        {
            cameraPosition.y = levelPosition.y + CameraSize.y;
        }

        if (cameraPosition.y > levelPosition.y + levelContainer.Size.y - CameraSize.y)
        {
            cameraPosition.y = levelPosition.y + levelContainer.Size.y - CameraSize.y;
        }

        camera.transform.position = cameraPosition;
    }
}
