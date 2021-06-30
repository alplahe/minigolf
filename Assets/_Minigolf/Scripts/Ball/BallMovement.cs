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
  }

  private void ChangeAngle(int direction)
  {
    angle += currentChangeAngleSpeed * Time.deltaTime * direction;
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
    mousePos.z = Camera.main.nearClipPlane;
    worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

    Debug.Log("worldPosition: " + worldPosition);
    Debug.Log("Input.mousePosition: " + Input.mousePosition);
    Vector3 angleVector = new Vector3(worldPosition.x, transform.position.y, worldPosition.z); 
    lineRenderer.SetPosition(1, worldPosition);
  }
}
