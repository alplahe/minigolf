using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
  public class ResetUI : MonoBehaviour
  {
    public void ResetScene()
    {
      Debug.Log("#ResetUI# ResetScene");

      Scene scene = SceneManager.GetActiveScene(); 
      SceneManager.LoadScene(scene.name);
    }
  }
}
