using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoVillage : MonoBehaviour
{
    public void GotoVil()
    {
        SceneManager.LoadScene("Village");
    }
}
