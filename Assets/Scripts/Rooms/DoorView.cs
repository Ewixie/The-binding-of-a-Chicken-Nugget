using UnityEngine;

namespace Rooms
{
    public class DoorView : MonoBehaviour
    {
        [SerializeField] private Door door;
        [SerializeField] private SpriteRenderer sr;
        [SerializeField] private Color openColor;
        [SerializeField] private Color closedColor;
        
        private void Start()
        {
            if (door.IsOpen) OnDoorOpened();
            else OnDoorClosed();

            door.Opened += OnDoorOpened;
            door.Closed += OnDoorClosed;
        }

        private void OnDoorOpened()
        {
            sr.color = openColor;
        }

        private void OnDoorClosed()
        {
            sr.color = closedColor;
        }



    }
}