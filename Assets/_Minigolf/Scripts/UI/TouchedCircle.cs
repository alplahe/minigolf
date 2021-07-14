using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Minigolf;
using System;
using UnityEngine.UI;

namespace UI
{
  public class TouchedCircle : MonoBehaviour
  {
    [SerializeField] private GameObject touchedCircleGO;

    private Image image;

    #region Init
    public void Init()
    {
      image = GetComponent<Image>();
      image.enabled = false;

      AddListeners();
    }
    #endregion

    #region Listeners
    private void AddListeners()
    {
      Messenger.AddListener(BroadcastName.Screen.OnBeingTouched, OnScreenBeingTouched);
      Messenger.AddListener(BroadcastName.Screen.OnTouchedDown, OnScreenTouchedDown);
      Messenger.AddListener(BroadcastName.Screen.OnTouchedReleased, OnScreenTouchedReleased);
    }

    private void RemoveListeners()
    {
      Messenger.RemoveListener(BroadcastName.Screen.OnBeingTouched, OnScreenBeingTouched);
      Messenger.RemoveListener(BroadcastName.Screen.OnTouchedDown, OnScreenTouchedDown);
      Messenger.RemoveListener(BroadcastName.Screen.OnTouchedReleased, OnScreenTouchedReleased);
    }

    private void OnDestroy()
    {
      RemoveListeners();
    }

    private void OnScreenBeingTouched()
    {
      Debug.Log("#TouchedCircle# OnScreenBeingTouched");

      transform.position = Input.mousePosition;
    }

    private void OnScreenTouchedDown()
    {
      Debug.Log("#TouchedCircle# OnScreenTouchedDown");

      ActivateTouchedCircle();

      transform.position = Input.mousePosition;
    }

    private void ActivateTouchedCircle()
    {
      image.enabled = true;
    }

    private void OnScreenTouchedReleased()
    {
      Debug.Log("#TouchedCircle# OnScreenTouchedReleased");

      DeactivateTouchedCircle();
    }

    private void DeactivateTouchedCircle()
    {
      image.enabled = false;
    }
    #endregion
  }
}