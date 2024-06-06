using UnityEngine;

[CreateAssetMenu(fileName = "New Movement Config", menuName = "Movement")]
public class MovementConfig : ScriptableObject
{
    public float MaxSpeed => maxSpeed;
    public float Acceleration => acceleration;
    
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
}