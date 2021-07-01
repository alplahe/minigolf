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

  private Rigidbody rigidbody;
  private LineRenderer lineRenderer;
  private float angle;
  private float currentChangeAngleSpeed;
  private float ballRadius;
  private float ballWithCursorAngle;

  private const float MIN_ANGLE = 0.0f;
  private const float MAX_ANGLE = 360.0f;

  #region Init
  private void Start()
  {
    Init();
  }

  private void Init()
  {
    rigidbody = GetComponent<Rigidbody>();
    lineRenderer = GetComponent<LineRenderer>();
    rigidbody.maxAngularVelocity = maxAngularVelocity;
    currentChangeAngleSpeed = changeAngleSpeed;
    ballRadius = GetComponent<SphereCollider>().radius;
  }
  #endregion

  private void Update()
  {
    if (Input.GetKey(KeyCode.LeftShift))
    {
      Debug.Log("press LeftShift");
      currentChangeAngleSpeed = fastChangeAngleSpeedMultiplier;
    }
    else
    {
      currentChangeAngleSpeed = changeAngleSpeed;
    }
    if (Input.GetMouseButton(0))
    {
      Debug.Log("press GetMouseButton(0)");
      UpdateLinePositionsWithMouse();
    }
    else
    {
      UpdateLinePositionsWithKeyboard();
    }
    if (Input.GetKey(KeyCode.A))
    {
      Debug.Log("press A");
      ChangeAngle(-1);
    }
    if (Input.GetKey(KeyCode.D))
    {
      Debug.Log("press D");
      ChangeAngle(1);
    }
    if (Input.GetKey(KeyCode.Space))
    {
      Debug.Log("press SPACE");
      rigidbody.AddForce(0.01f, 0, 0, ForceMode.Impulse);
    }

    //AssignBallWithCursorAngle();
  }

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
    lineRenderer.SetPosition(0, transform.position);
    lineRenderer.SetPosition(1, transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * lineLength);
  }

  private void UpdateLinePositionsWithMouse()
  {
    lineRenderer.SetPosition(0, transform.position);

    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector3 mousePos = Input.mousePosition;
    mousePos.z = Camera.main.transform.position.y - transform.position.y;
    worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

    Debug.Log("worldPosition: " + worldPosition);
    Debug.Log("Input.mousePosition: " + Input.mousePosition);
    Vector3 angleVector = new Vector3(worldPosition.x, transform.position.y, worldPosition.z); 
    lineRenderer.SetPosition(1, worldPosition);

    AssignBallWithCursorAngle();
  }

  private void AssignBallWithCursorAngle()
  {
    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

    Vector3 mousePos = Input.mousePosition;
    mousePos.z = Camera.main.transform.position.y - transform.position.y;
    worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

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
}
