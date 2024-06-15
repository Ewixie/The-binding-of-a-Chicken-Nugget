using UnityEngine;

public class PlayerTeleporter : MonoBehaviour
{
    void Start()
    {
        Player.Player.Instance.PlayerTransform.position = transform.position;
    }

    
}
