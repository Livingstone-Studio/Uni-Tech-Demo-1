using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Room
{
    public Vector2 centerPos;

    public float leftBound;
    public float rightBound;
    public float upBound;
    public float downBound;


    public bool CheckLeftBound(float x)
    {
        if (x < leftBound)
        {
            return true;
        }

        return false;
    }

    public bool CheckRightBound(float x)
    {
        if (x > rightBound)
        {
            return true;
        }

        return false;
    }

    public bool CheckUpBound(float y)
    {
        if (y > upBound)
        {
            return true;
        }

        return false;
    }

    public bool CheckDownBound(float y)
    {
        if (y < downBound)
        {
            return true;
        }

        return false;
    }
}

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { set; get; }

    [Header("Components")]

    private Camera cam;

    [Header("Room Configuration")]

    [Tooltip("Defines the rooms the camera bounds to.")]
    [SerializeField] private List<Room> rooms;

    private Room currentRoom;

    [Header("Variables/Settings")]

    [SerializeField] private float moveSpeed = 30f;

    private Transform playerTransform;
    private Rigidbody2D playerRB;
    private Animator playerAnimator;

    private float height;
    private float width;

    internal bool moving = false;

    private Vector3 camPosNew;

    [SerializeField] private float offset = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;

        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform != null)
        {
            playerRB = playerTransform.GetComponent<Rigidbody2D>();
            playerAnimator = playerTransform.GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) return;

        #region New Code
        /*
        if (!moving)
        {
            // Bound check + track code.

            if (currentRoom.CheckLeftBound(playerTransform.position.x))
            {
                moving = true;
            }
            else if (currentRoom.CheckRightBound(playerTransform.position.x))
            {
                moving = true;
            }
            else if (currentRoom.CheckUpBound(playerTransform.position.y))
            {
                moving = true;
            }
            else if (currentRoom.CheckDownBound(playerTransform.position.y))
            {
                moving = true;
            }

            if (moving)
            {
                FreezePlayerRB();
            }
        }
        else if (moving && camPosNew != Vector3.zero)
        {
            // Transition code.
            cam.transform.position += (camPosNew - cam.transform.position).normalized * Time.deltaTime * moveSpeed;

            if (Mathf.Abs(camPosNew.x - cam.transform.position.x) < 0.1f && Mathf.Abs(camPosNew.y - cam.transform.position.y) < 0.1f)
            {
                cam.transform.position = camPosNew;
                moving = false;

                UnFreezePlayerRB();
            }
        }
        */
        #endregion

        #region Old Code.

        if (!moving)
        {
            camPosNew = BoundsCheck();

            if (camPosNew != Vector3.zero)
            {
                moving = true;

                FreezePlayerRB();
            }
        }
        else if (moving && camPosNew != Vector3.zero)
        {
            cam.transform.position += (camPosNew - cam.transform.position).normalized * Time.deltaTime * moveSpeed;

            if (Mathf.Abs(camPosNew.x - cam.transform.position.x) < 0.1f && Mathf.Abs(camPosNew.y - cam.transform.position.y) < 0.1f)
            {
                cam.transform.position = camPosNew;
                moving = false;

                UnFreezePlayerRB();
            }
        }

        #endregion
    }

    private Vector3 BoundsCheck()
    {
        if (playerTransform.position.x > cam.transform.position.x + offset + width / 2)
        {
            // Move right

            return new Vector3(cam.transform.position.x + width, cam.transform.position.y, cam.transform.position.z);
        }
        else if (playerTransform.position.x < cam.transform.position.x - offset - width / 2)
        {
            // Move left

            return new Vector3(cam.transform.position.x - width, cam.transform.position.y, cam.transform.position.z);
        }

        if (playerTransform.position.y > cam.transform.position.y + offset + height / 2)
        {
            // Move up

            return new Vector3(cam.transform.position.x, cam.transform.position.y + height, cam.transform.position.z);
        }
        else if (playerTransform.position.y < cam.transform.position.y - offset - height / 2)
        {
            // Move down

            return new Vector3(cam.transform.position.x, cam.transform.position.y - height, cam.transform.position.z);
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void FreezePlayerRB()
    {
        if (playerRB == null) return;

        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;

        if (playerAnimator == null) return;

        playerAnimator.speed = 0;
    }

    private void UnFreezePlayerRB()
    {
        if (playerRB == null) return;

        playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (playerAnimator == null) return;

        playerAnimator.speed = 1;
    }
}