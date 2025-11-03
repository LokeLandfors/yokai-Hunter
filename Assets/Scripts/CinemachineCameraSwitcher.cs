using UnityEngine;
using Cinemachine;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera targetCamera; //Aktiverar kameran

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) //Säkerställer att Player är i triggerzone
        {
            ActivateCamera();
        }
    }

    void ActivateCamera()
    {
        // Deactivate all other cameras
        CinemachineVirtualCamera[] allCameras = FindObjectsOfType<CinemachineVirtualCamera>();
        foreach (var cam in allCameras)
        {
            cam.Priority = 0; //Sätter alla kameror till låg prioritet
        }

        //Aktiverar målkameran
        targetCamera.Priority = 10; //Sätter målkameran till hög prioritet
    }
}
