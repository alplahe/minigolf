using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
  public float maxPower;
  public float lineLength;
  public float maxAngularVelocity;
  public float changeAngleSpeed;
  public float fastChangeAngleSpeedMultiplier;
  public float changeForceMagnitudeSpeed;
  public float minSpeedIsConsideredMoving = 0.01f;
  public float forceMultiplier = 0.5f;
  public float firstLinePositionDistanceGap;

  private Rigidbody ballRigidbody;
  private LineRenderer lineRenderer;
  private float angle;
  private float currentChangeAngleSpeed;
  private float ballRadius;
  private float ballWithCursorAngle;
  private bool isMouseControl;
  private bool isKeyboardControl;
  private Vector3 worldPosition;
  private bool isPuttCanceled = false;
  private bool isLineShowedAfterBallStop = false;
  private bool isBallPutted = false;

  private const float MIN_ANGLE = 0.0f;
  private const float MAX_ANGLE = 360.0f;
  private const float MIN_FORCE_MAGNITUDE = 0.05f;
  private const float MAX_FORCE_MAGNITUDE = 0.6f;
  private const float STARTING_FORCE_MAGNITUDE = 0.25f;
  private const int FORCE_DIRECTION = -1;                 // Inverse the direction to set the line as the golf club.
                                                          // TODO 07-07-2021: consider this design as may be confusing.

  [SerializeField] private BallMovementControlSet controlSet;
  [SerializeField, Range(0, 5)] private float forceMagnitude;

  public bool IsMouseControl { get => isMouseControl; set => isMouseControl = value; }
  public bool IsKeyboardControl { get => isKeyboardControl; set => isKeyboardControl = value; }
  public bool IsPuttCanceled { get => isPuttCanceled; set => isPuttCanceled = value; }

  #region Init
  private void Start()
  {
    Init();
  }

  private void Init()
  {
    ballRigidbody = GetComponent<Rigidbody>();
    lineRenderer = GetComponent<LineRenderer>();
    ballRigidbody.maxAngularVelocity = maxAngularVelocity;
    currentChangeAngleSpeed = changeAngleSpeed;
    ballRadius = GetComponent<SphereCollider>().radius;
    forceMagnitude = STARTING_FORCE_MAGNITUDE;

    controlSet.Init(this);

    ShowLineRenderer();
  }
  #endregion

  private void Update()
  {
    UpdateLinePositions();
  }

  #region Direction line
  private void ChangeAngle(int direction)
  {
    angle += currentChangeAngleSpeed * Time.deltaTime * direction;
    ClampAngle();
  }

  void ClampAngle()
  {
    if (angle >= MAX_ANGLE) angle -= MAX_ANGLE;
    if (angle < MIN_ANGLE) angle += MAX_ANGLE;
  }

  private void ShowLineRenderer()
  {
    ClampForceMagnitude();
    SetLinePositions(angle, firstLinePositionDistanceGap, lineLength, forceMagnitude);
  }

  private void UpdateLinePositionsWithKeyboard()
  {
    if (IsMouseControl) return;
    UpdateLinePositions();
  }

  private void UpdateForceMagnitudeWithKeyboard(int positiveGrown)
  {
    if (IsMouseControl) return;

    forceMagnitude += changeForceMagnitudeSpeed * Time.deltaTime * positiveGrown;
    ClampForceMagnitude();
  }

  private void UpdateLinePositionsWithMouse()
  {
    if (IsPuttCanceled) return;
    AssignBallWithCursorAngle();
    UpdateLinePositions();
  }

  private void UpdateLinePositions()
  {
    if (!IsLineRendererShowable())
    {
      Debug.Log("DO NOT show line renderer");
      HideLinePositions();
      return;
    }

    if (!IsMouseControl && !IsKeyboardControl && isLineShowedAfterBallStop) return;

    ClampForceMagnitude();
    SetLinePositions(angle, firstLinePositionDistanceGap, lineLength, forceMagnitude);
    isLineShowedAfterBallStop = true;
  }

  private void HideLinePositions()
  {
    Debug.Log("HideLinePositions");

    lineRenderer.SetPosition(0, Vector3.zero);
    lineRenderer.SetPosition(1, Vector3.zero);

    isLineShowedAfterBallStop = false;
  }

  private void SetLinePositions(float angle, float firstLinePositionDistanceGap, float lineLength, float forceMagnitude)
  {
    Debug.Log("SetLinePositions");

    Vector3 lineDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;

    lineRenderer.SetPosition(0, transform.position +
                                lineDirection * firstLinePositionDistanceGap);
    lineRenderer.SetPosition(1, transform.position +
                                lineDirection * firstLinePositionDistanceGap +
                                lineDirection * lineLength * forceMagnitude);
  }

  private bool IsLineRendererShowable()
  {
    return !IsBallMoving();
  }

  private bool IsBallMoving()
  {
    if (ballRigidbody.velocity.magnitude < minSpeedIsConsideredMoving)
    {
      ballRigidbody.velocity = Vector3.zero;
      ballRigidbody.angularVelocity = Vector3.zero;
    }

    //bool isBallMoving = ballRigidbody.velocity.magnitude != 0;
    //Debug.Log("IsBallMoving: " + isBallMoving);

    return ballRigidbody.velocity.magnitude != 0;
  }

  private void ClampForceMagnitude()
  {
    if (forceMagnitude >= MAX_FORCE_MAGNITUDE) forceMagnitude = MAX_FORCE_MAGNITUDE;
    if (forceMagnitude < MIN_FORCE_MAGNITUDE) forceMagnitude = MIN_FORCE_MAGNITUDE;
  }

  private void AssignBallWithCursorAngle()
  {
    Vector3 mousePosition = Input.mousePosition;
    mousePosition.z = Camera.main.transform.position.y - transform.position.y;
    worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

    Vector3 lineDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;

    Vector3 cursorToBallVector = worldPosition - transform.position;
    forceMagnitude = cursorToBallVector.magnitude - firstLinePositionDistanceGap; // Substract starting distance gap to starting
                                                                                  // counting force from that distance
    float cursorToBallAngle = Vector3.Angle(cursorToBallVector, Vector3.forward);

    cursorToBallAngle = ReformatAngle(worldPosition, cursorToBallAngle);
    angle = cursorToBallAngle;
  }

  // Reformat angle to 0-360 degrees. Previously it was 0-180, 180-0.
  private float ReformatAngle(Vector3 worldPosition, float cursorToBallAngle)
  {
    if (worldPosition.x - transform.position.x < 0) cursorToBallAngle = MAX_ANGLE - cursorToBallAngle;
    return cursorToBallAngle;
  }

  #endregion

  #region Force
  private void ApplyForce()
  {
    if (IsBallMoving()) return;
    if (IsPuttCanceled) return;

    ballRigidbody.AddForce(Quaternion.Euler(0, angle, 0) * Vector3.forward * 
                           lineLength * forceMagnitude * FORCE_DIRECTION *
                           forceMultiplier, 
                           ForceMode.Impulse);

    HideLinePositions();
    isBallPutted = true;
  }

  void CancelPutt()
  {
    if (!IsMouseControl) return;
    IsPuttCanceled = true;
    forceMagnitude = STARTING_FORCE_MAGNITUDE;
    ShowLineRenderer();
  }
  #endregion

  #region Control set
  public void OnUpdateLinePositionsWithMouse()
  {
    UpdateLinePositionsWithMouse();
  }

  public void OnApplyForce()
  {
    ApplyForce();
  }

  public void OnCancelPutt()
  {
    CancelPutt();
  }
  
  public void OnRegularDirectionSpeed()
  {
    currentChangeAngleSpeed = changeAngleSpeed;
  }
  
  public void OnFastDirectionSpeed()
  {
    currentChangeAngleSpeed = fastChangeAngleSpeedMultiplier;
  }

  public void OnLeft()
  {
    ChangeAngle(-1);
    UpdateLinePositionsWithKeyboard();
  }

  public void OnRight()
  {
    ChangeAngle(1);
    UpdateLinePositionsWithKeyboard();
  }
  public void OnUp()
  {
    UpdateForceMagnitudeWithKeyboard(-1);
    UpdateLinePositionsWithKeyboard();
  }

  public void OnDown()
  {
    UpdateForceMagnitudeWithKeyboard(1);
    UpdateLinePositionsWithKeyboard();
  }
  #endregion
}
