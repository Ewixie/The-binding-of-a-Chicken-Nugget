using UnityEngine;

public class BillBoardSprite : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
        
    }


    void Update()
    {
        transform.LookAt(_camera.transform.position, -Vector3.up);
    }
}