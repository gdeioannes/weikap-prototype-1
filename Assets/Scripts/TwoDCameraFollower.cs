using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

[RequireComponent(typeof(Camera))]
public class TwoDCameraFollower : Camera2DFollow
{
    [SerializeField] LevelContainer levelContainer;
    Camera mCamera;

    void OnEnable()
    {
        mCamera = this.GetComponent<Camera>();
    }

    public Vector2 CameraSize
    {
        get
        {
            Vector2 toReturn = Vector2.one;
            toReturn.x = mCamera.orthographicSize * Screen.width / Screen.height;
            toReturn.y = mCamera.orthographicSize;
            return toReturn;
        }
    }

    override protected void Update()
    {
        base.Update();
        RestrictToLevelContainer();
    }

    void RestrictToLevelContainer()
    {
        Vector3 cameraPosition = mCamera.transform.position;
        Vector3 levelPosition = levelContainer.transform.position;
        Vector3 cameraSize = CameraSize;

        if (cameraPosition.x < (levelPosition.x + cameraSize.x))
        {
            cameraPosition.x = levelPosition.x + cameraSize.x;
        }

        if (cameraPosition.x > (levelPosition.x + levelContainer.Size.x - cameraSize.x))
        {
            cameraPosition.x = levelPosition.x + levelContainer.Size.x - cameraSize.x;
        }

        if (cameraPosition.y < levelPosition.y + cameraSize.y)
        {
            cameraPosition.y = levelPosition.y + cameraSize.y;
        }

        if (cameraPosition.y > levelPosition.y + levelContainer.Size.y - cameraSize.y)
        {
            cameraPosition.y = levelPosition.y + levelContainer.Size.y - cameraSize.y;
        }

        this.transform.position = cameraPosition;
    }


}
