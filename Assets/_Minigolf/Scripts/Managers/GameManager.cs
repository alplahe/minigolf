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
    }
    #endregion
  }
}
