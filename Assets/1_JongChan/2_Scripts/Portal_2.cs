using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal_2 : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Unit unit) && unit.kind == Unit.Kind.Player)
        {
            SceneManager.LoadScene("Scenes/Ending");
        }
    }
}
