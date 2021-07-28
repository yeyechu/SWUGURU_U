using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject myRoomPos;
    public GameObject livingRoomPos;
    public Vector3 currentPos;

    public enum CamPos
    {
        myRoom,
        livingRoom
    }

    public static CamPos cam;

    // Start is called before the first frame update
    void Start()
    {
        goMyRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void goMyRoom()
    {
        cam = CamPos.myRoom;
        currentPos.Set(myRoomPos.transform.position.x, myRoomPos.transform.position.y, this.transform.position.z);
        this.transform.position = currentPos;
    }

    public void goLivingRoom()
    {
        cam = CamPos.livingRoom;
        currentPos.Set(livingRoomPos.transform.position.x, livingRoomPos.transform.position.y, this.transform.position.z);
        this.transform.position = currentPos;
    }
}
