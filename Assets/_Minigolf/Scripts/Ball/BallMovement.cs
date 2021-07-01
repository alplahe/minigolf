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

  private Rigidbody ballRigidbody;
  private LineRenderer lineRenderer;
  private float angle;
  private float currentChangeAngleSpeed;
  private float ballRadius;
  private float ballWithCursorAngle;
  private bool isMouseControl;

  private const float MIN_ANGLE = 0.0f;
  private const float MAX_ANGLE = 360.0f;

  [SerializeField] private BallMovementControlSet controlSet;

  public bool IsMouseControl { get => isMouseControl; set => isMouseControl = value; }

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

    controlSet.Init(this);
  }
  #endregion

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

  private void UpdateLinePositionsWithKeyboard()
  {
    if (IsMouseControl) return;

    lineRenderer.SetPosition(0, transform.position);
    lineRenderer.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
  }

  private void UpdateLinePositionsWithMouse()
  {
    lineRenderer.SetPosition(0, transform.position);

    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector3 mousePosition = Input.mousePosition;
    mousePosition.z = Camera.main.transform.position.y - transform.position.y;
    worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

    Debug.Log("worldPosition: " + worldPosition);
    Debug.Log("Input.mousePosition: " + Input.mousePosition);
    Vector3 angleVector = new Vector3(worldPosition.x, transform.position.y, worldPosition.z); 
    lineRenderer.SetPosition(1, worldPosition);

    AssignBallWithCursorAngle();
  }

  private void AssignBallWithCursorAngle()
  {
    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector3 mousePosition = Input.mousePosition;
    mousePosition.z = Camera.main.transform.position.y - transform.position.y;
    worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

    Debug.Log("worldPosition: " + worldPosition.ToString("F6"));
    Debug.Log("Input.mousePosition: " + Input.mousePosition);

    Vector3 vectorWithoutYAxis = new Vector3(1.0f, 0.0f, 1.0f);
    Vector3 cursorToBallVector = new Vector3((worldPosition.x - transform.position.x),
                                             (worldPosition.y - transform.position.y),
                                             (worldPosition.z - transform.position.z));
    //Vector3 cursorToBallVector = (worldPosition - transform.position) * vectorWithoutYAxis;
    Debug.Log("cursorToBallVector: " + cursorToBallVector.ToString("F6"));

    float cursorToBallAngle = Vector3.Angle(cursorToBallVector, Vector3.forward);
    if (worldPosition.x - transform.position.x < 0) cursorToBallAngle = MAX_ANGLE - cursorToBallAngle;
    angle = cursorToBallAngle;
    Debug.Log("cursorToBallAngle: " + cursorToBallAngle);

    Debug.Log("GetWorldPositionOnPlane: " + GetWorldPositionOnPlane(worldPosition, 0.13f));
  }

  public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float y)
  {
    Ray ray = Camera.main.ScreenPointToRay(screenPosition);
    Plane xz = new Plane(Vector3.up, new Vector3(0, y, 0));
    float distance;
    xz.Raycast(ray, out distance);
    return ray.GetPoint(distance);
  }
  #endregion

  #region Control set
  public void OnUpdateLinePositionsWithMouse()
  {
    UpdateLinePositionsWithMouse();
  }

  public void OnApplyForce()
  {
    ballRigidbody.AddForce(0.01f, 0, 0, ForceMode.Impulse);
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
  #endregion
}
