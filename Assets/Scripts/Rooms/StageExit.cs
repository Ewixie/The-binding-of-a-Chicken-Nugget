using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public class StageExit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManager.Instance.MoveToNextStage();
    }
}
