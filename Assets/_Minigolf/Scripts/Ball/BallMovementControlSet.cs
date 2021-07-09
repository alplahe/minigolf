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
    OnLeftMouseButtonUp();

    OnAccept();
    OnCancel();
    OnLeftShift();
    OnLeft();
    OnRight();
    OnUp();
    OnDown();
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
      parent.IsKeyboardControl = false;
      parent.OnUpdateLinePositionsWithMouse();
    }
    else
    {
      //parent.IsMouseControl = false;
    }
  }

  public void OnLeftMouseButtonUp()
  {
    if (Input.GetMouseButtonUp(0))
    {
      Debug.Log($"#BallMovementControlSet# pressed OnLeftMouseButtonUp!");
      parent.IsMouseControl = false;
      //parent.IsKeyboardControl = true;
      Putt();
    }
    else
    {
      //parent.IsMouseControl = false;
    }
  }
  #endregion

  #region Keyboard
  public void OnAccept()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      Debug.Log($"#BallMovementControlSet# pressed accept!");
      parent.IsKeyboardControl = true;
      Putt();
    }
  }

  public void OnCancel()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      Debug.Log($"#BallMovementControlSet# pressed cancel!");
      parent.OnCancelPutt();
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
      parent.IsKeyboardControl = true;
      parent.OnLeft();
    }
  }

  public void OnRight()
  {
    if (Input.GetKey(KeyCode.RightArrow) ||
        Input.GetKey(KeyCode.D))
    {
      Debug.Log($"#BallMovementControlSet# pressed OnRight!");
      parent.IsKeyboardControl = true;
      parent.OnRight();
    }
  }

  public void OnUp()
  {
    if (Input.GetKey(KeyCode.UpArrow) ||
        Input.GetKey(KeyCode.W))
    {
      Debug.Log($"#BallMovementControlSet# pressed OnUp!");
      parent.IsKeyboardControl = true;
      parent.OnUp();
    }
  }

  public void OnDown()
  {
    if (Input.GetKey(KeyCode.DownArrow) ||
        Input.GetKey(KeyCode.S))
    {
      Debug.Log($"#BallMovementControlSet# pressed OnDown!");
      parent.IsKeyboardControl = true;
      parent.OnDown();
    }
  }
  #endregion

  #region Logic
  private void Putt()
  {
    if(!parent.IsPuttCanceled) parent.OnApplyForce();
    parent.IsPuttCanceled = false;
  }

  #endregion
  #endregion
}
