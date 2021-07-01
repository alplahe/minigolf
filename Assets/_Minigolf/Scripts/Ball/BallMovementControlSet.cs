using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BallMovement))]
public class BallMovementControlSet : MonoBehaviour
{
  public BallMovement parent;

  #region Init
  private void Awake()
  {
    parent = GetComponent<BallMovement>();
  }

  public void Init(BallMovement _parent)
  {
    parent = _parent;
  }
  #endregion

  #region Listen for key pressed
  private void Update()
  {
    OnLeftMouseButton();

    OnAccept();
    OnLeftShift();
    OnLeft();
    OnRight();
  }
  #endregion

  #region Control set

  #region Mouse
  public void OnLeftMouseButton()
  {
    if (Input.GetMouseButton(0))
    {
      Debug.Log($"#BallMovementControlSet# pressed OnLeftMouseButton!");
      parent.IsMouseControl = true;
      parent.OnUpdateLinePositionsWithMouse();
    }
    else
    {
      parent.IsMouseControl = false;
    }
  }
  #endregion

  #region Keyboard
  public void OnAccept()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      Debug.Log($"#BallMovementControlSet# pressed accept!");
      parent.OnApplyForce();
    }
  }

  public void OnLeftShift()
  {
    if (Input.GetKey(KeyCode.LeftShift))
    {
      Debug.Log($"#BallMovementControlSet# pressed OnLeftShift!");
      parent.OnFastDirectionSpeed();
    }
    else
    {
      parent.OnRegularDirectionSpeed();
    }
  }

  public void OnLeft()
  {
    if (Input.GetKey(KeyCode.LeftArrow) ||
        Input.GetKey(KeyCode.A))
    {
      Debug.Log($"#BallMovementControlSet# pressed OnLeft!");
      parent.OnLeft();
    }
  }

  public void OnRight()
  {
    if (Input.GetKey(KeyCode.RightArrow) ||
        Input.GetKey(KeyCode.D))
    {
      Debug.Log($"#BallMovementControlSet# pressed OnRight!");
      parent.OnRight();
    }
  }
  #endregion
  #endregion
}
