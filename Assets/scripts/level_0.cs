using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class level_0 : MonoBehaviour
{
    public Button devam_butonu;
     public void sahne_degis()
    {
        // SceneManager ile hedef sahneye geçiş yapılır
        SceneManager.LoadScene(1);
    }
}
