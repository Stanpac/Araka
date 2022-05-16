using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSaver : MonoBehaviour
{
    public List<Vector2> SavedInput = new List<Vector2>();
    Vector2 startTouchPos, currentTouchPos, endTouchPos;
    bool clickBool;
    Vector2 directionSwipe;
    float clickTimer;
    public float clickTimerValue;
    public float deadZoneDiameter;
    CameraBehavior cam;
    SceneChange sceneChange;

    private void Awake()
    {
        cam = FindObjectOfType<CameraBehavior>();
        sceneChange = FindObjectOfType<SceneChange>();
    }

    private void Start()
    {
        clickTimer = clickTimerValue;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouchPos = Input.mousePosition;
            clickBool = true;
        }

        if (Input.GetMouseButton(0))
        {
            currentTouchPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && Vector2.Distance(currentTouchPos, startTouchPos) >= deadZoneDiameter /*&& !clickBool*/)
        {
            endTouchPos = Input.mousePosition;
            directionSwipe = -(startTouchPos - endTouchPos).normalized;
            if (sceneChange.Hub)
            {
                directionSwipe = Quaternion.AngleAxis(-45, -Vector3.forward) * directionSwipe;
            }
            if (cam.rotateMode == 0)
            {
                //directionSwipe = directionSwipe;
            }
            else if (cam.rotateMode == 1)
            {
                directionSwipe = Quaternion.AngleAxis(90, -Vector3.forward) * directionSwipe;
            }
            else if (cam.rotateMode == 2)
            {
                directionSwipe = Quaternion.AngleAxis(180, -Vector3.forward) * directionSwipe;
            }
            else if (cam.rotateMode == 3)
            {
                directionSwipe = Quaternion.AngleAxis(270, -Vector3.forward) * directionSwipe;
            }
            else
            {
                Debug.LogError("Prob avce l'angle de swipe, modulo pas correct");
            }
            SavedInput.Add(directionSwipe);
        }
       

      

        if (clickBool)
        {
            clickTimer -= Time.deltaTime;
            if (clickTimer <= 0)
            {
                clickBool = false;
                clickTimer = clickTimerValue;
            }
        }

    }
}
