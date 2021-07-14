using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ball;
using UI;

namespace Minigolf
{
  public class GameManager : MonoBehaviour
  {
    [SerializeField] private BallMovement ballMovement;
    [SerializeField] private TouchedCircle touchedCircle;
    [SerializeField] private Hole hole;

    #region Init
    private void Start()
    {
      Init();
    }

    private void Init()
    {
      if (ballMovement == null) return;
      ballMovement.Init();

      if (touchedCircle == null) return;
      touchedCircle.Init();

      if (hole == null) return;
      hole.Init();

      AddListeners();
    }
    #endregion

    #region Listeners
    private void AddListeners()
    {
      Messenger.AddListener(BroadcastName.Ball.OnBallInHole, OnBallInHole);
    }

    private void RemoveListeners()
    {
      Messenger.RemoveListener(BroadcastName.Ball.OnBallInHole, OnBallInHole);
    }

    private void OnDestroy()
    {
      RemoveListeners();
    }

    private void OnBallInHole()
    {
      Debug.Log("#GameManager# OnBallInHole");
    }
    #endregion
  }
}
