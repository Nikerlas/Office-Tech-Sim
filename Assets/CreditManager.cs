using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditManager : MonoBehaviour
{
    //
    // void  Start is called before the first frame update    
    public void Credit()
    {
        SceneManager.LoadScene(
            "MainMenuScene"
        );
    }
}
