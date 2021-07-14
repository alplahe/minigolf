using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigolf
{
  public class Hole : MonoBehaviour
  {
    private AudioSource audioSource;
    private AudioClip audioClip;

    #region Init
    public void Init()
    {
      audioSource = GetComponent<AudioSource>();

      if(audioSource == null)
      {
        Debug.Log("#Hole# audioSource is NULL");
        return;
      }

      audioClip = audioSource.clip;

      Debug.Log("audioSource: " + audioSource);
      Debug.Log("audioClip: " + audioClip);
    }
    #endregion

    private void OnTriggerEnter(Collider other)
    {
      if(other.CompareTag("Ball"))
      {
        DoBallInHole();
      }
    }

    private void DoBallInHole()
    {
      Messenger.Broadcast(BroadcastName.Ball.OnBallInHole);
      audioSource.PlayOneShot(audioClip);
    }
  }
}