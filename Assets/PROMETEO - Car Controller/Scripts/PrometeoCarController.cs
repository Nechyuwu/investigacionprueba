using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PrometeoCarController : MonoBehaviour
{
    // CAR SETUP
    [Space(20)]
    [Space(10)]
    [Range(20, 190)]
    public int maxSpeed = 90; 
    [Range(10, 120)]
    public int maxReverseSpeed = 45; 
    [Range(1, 10)]
    public int accelerationMultiplier = 2; 
    [Space(10)]
    [Range(10, 45)]
    public int maxSteeringAngle = 27; 
    [Range(0.1f, 1f)]
    public float steeringSpeed = 0.5f; 
    [Space(10)]
    [Range(100, 600)]
    public int brakeForce = 350; 
    [Range(1, 10)]
    public int decelerationMultiplier = 2; 
    [Space(10)]
    public Vector3 bodyMassCenter; 

    // WHEELS
    [Space(20)]
    public GameObject frontLeftMesh;
    public WheelCollider frontLeftCollider;
    [Space(10)]
    public GameObject frontRightMesh;
    public WheelCollider frontRightCollider;
    [Space(10)]
    public GameObject rearLeftMesh;
    public WheelCollider rearLeftCollider;
    [Space(10)]
    public GameObject rearRightMesh;
    public WheelCollider rearRightCollider;

    // SPEED TEXT (UI)
    [Space(20)]
    public bool useUI = true; // Cambiado a true por defecto
    public Text carSpeedText; 

    // SOUNDS
    [Space(20)]
    public bool useSounds = true; // Cambiado a true por defecto
    public AudioSource carEngineSound; 
    public AudioSource tireScreechSound; 
    float initialCarEngineSoundPitch; 

    // CAR DATA
    [HideInInspector]
    public float carSpeed; 

    // PRIVATE VARIABLES
    Rigidbody carRigidbody; 
    float steeringAxis; 
    float throttleAxis; 
    float localVelocityZ;
    float localVelocityX;
    bool deceleratingCar;

    void Start()
    {
        carRigidbody = gameObject.GetComponent<Rigidbody>();
        carRigidbody.centerOfMass = bodyMassCenter;

        if(carEngineSound != null){
            initialCarEngineSoundPitch = carEngineSound.pitch;
        }

        if(useUI){
            InvokeRepeating("CarSpeedUI", 0f, 0.1f);
        }else if(!useUI && carSpeedText != null){
            carSpeedText.text = "0";
        }

        if(useSounds){
            InvokeRepeating("CarSounds", 0f, 0.1f);
        }else if(!useSounds){
            if(carEngineSound != null) carEngineSound.Stop();
            if(tireScreechSound != null) tireScreechSound.Stop();
        }
    }

    void Update()
    {
        // CAR DATA
        carSpeed = (2 * Mathf.PI * frontLeftCollider.radius * frontLeftCollider.rpm * 60) / 1000;
        localVelocityX = transform.InverseTransformDirection(carRigidbody.linearVelocity).x;
        localVelocityZ = transform.InverseTransformDirection(carRigidbody.linearVelocity).z;

        // CONTROLS (Input System)
        if(Keyboard.current.wKey.isPressed){
            CancelInvoke("DecelerateCar");
            deceleratingCar = false;
            GoForward();
        }
        if(Keyboard.current.sKey.isPressed){
            CancelInvoke("DecelerateCar");
            deceleratingCar = false;
            GoReverse();
        }

        if(Keyboard.current.aKey.isPressed){
            TurnLeft();
        }
        if(Keyboard.current.dKey.isPressed){
            TurnRight();
        }

        if (!Keyboard.current.sKey.isPressed && !Keyboard.current.wKey.isPressed){
            ThrottleOff();
        }

        if (!Keyboard.current.sKey.isPressed && !Keyboard.current.wKey.isPressed && !deceleratingCar){
            InvokeRepeating("DecelerateCar", 0f, 0.1f);
            deceleratingCar = true;
        }

        if (!Keyboard.current.aKey.isPressed && !Keyboard.current.dKey.isPressed && steeringAxis != 0f){
            ResetSteeringAngle();
        }

        AnimateWheelMeshes();
    }

    public void CarSpeedUI(){
        if(useUI){
            try{
                float absoluteCarSpeed = Mathf.Abs(carSpeed);
                carSpeedText.text = Mathf.RoundToInt(absoluteCarSpeed).ToString();
            }catch(Exception ex){
                Debug.LogWarning("UI Error: " + ex);
            }
        }
    }

    public void CarSounds(){
        if(useSounds){
            try{
                if(carEngineSound != null){
                    float engineSoundPitch = initialCarEngineSoundPitch + (Mathf.Abs(carRigidbody.linearVelocity.magnitude) / 25f);
                    carEngineSound.pitch = engineSoundPitch;
                }
                
                // Reproduce el sonido de las llantas si el carro hace un giro muy brusco (deslizamiento lateral)
                bool isSlipping = Mathf.Abs(localVelocityX) > 2.5f;
                if(isSlipping && Mathf.Abs(carSpeed) > 12f){
                    if(!tireScreechSound.isPlaying){
                        tireScreechSound.Play();
                    }
                }else{
                    tireScreechSound.Stop();
                }
            }catch(Exception ex){
                Debug.LogWarning("Sound Error: " + ex);
            }
        }
    }

    // STEERING METHODS
    public void TurnLeft(){
        steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
        if(steeringAxis < -1f) steeringAxis = -1f;
        ApplySteering();
    }

    public void TurnRight(){
        steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
        if(steeringAxis > 1f) steeringAxis = 1f;
        ApplySteering();
    }

    public void ResetSteeringAngle(){
        if(steeringAxis < 0f){
            steeringAxis = steeringAxis + (Time.deltaTime * 10f * steeringSpeed);
        }else if(steeringAxis > 0f){
            steeringAxis = steeringAxis - (Time.deltaTime * 10f * steeringSpeed);
        }
        if(Mathf.Abs(frontLeftCollider.steerAngle) < 1f){
            steeringAxis = 0f;
        }
        ApplySteering();
    }

    void ApplySteering(){
        var steeringAngle = steeringAxis * maxSteeringAngle;
        frontLeftCollider.steerAngle = Mathf.Lerp(frontLeftCollider.steerAngle, steeringAngle, steeringSpeed);
        frontRightCollider.steerAngle = Mathf.Lerp(frontRightCollider.steerAngle, steeringAngle, steeringSpeed);
    }

    void AnimateWheelMeshes(){
        try{
            Vector3 pos;
            Quaternion rot;

            frontLeftCollider.GetWorldPose(out pos, out rot);
            frontLeftMesh.transform.position = pos;
            frontLeftMesh.transform.rotation = rot;

            frontRightCollider.GetWorldPose(out pos, out rot);
            frontRightMesh.transform.position = pos;
            frontRightMesh.transform.rotation = rot;

            rearLeftCollider.GetWorldPose(out pos, out rot);
            rearLeftMesh.transform.position = pos;
            rearLeftMesh.transform.rotation = rot;

            rearRightCollider.GetWorldPose(out pos, out rot);
            rearRightMesh.transform.position = pos;
            rearRightMesh.transform.rotation = rot;
        }catch(Exception ex){
            Debug.LogWarning("Mesh Animation Error: " + ex);
        }
    }

    // ENGINE AND BRAKING METHODS
    public void GoForward(){
        throttleAxis = throttleAxis + (Time.deltaTime * 3f);
        if(throttleAxis > 1f) throttleAxis = 1f;

        if(localVelocityZ < -1f){
            Brakes();
        }else{
            if(Mathf.RoundToInt(carSpeed) < maxSpeed){
                frontLeftCollider.brakeTorque = 0;
                frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
                frontRightCollider.brakeTorque = 0;
                frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
                rearLeftCollider.brakeTorque = 0;
                rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
                rearRightCollider.brakeTorque = 0;
                rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
            }else{
                ThrottleOff();
            }
        }
    }

    public void GoReverse(){
        throttleAxis = throttleAxis - (Time.deltaTime * 3f);
        if(throttleAxis < -1f) throttleAxis = -1f;

        if(localVelocityZ > 1f){
            Brakes();
        }else{
            if(Mathf.Abs(Mathf.RoundToInt(carSpeed)) < maxReverseSpeed){
                frontLeftCollider.brakeTorque = 0;
                frontLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
                frontRightCollider.brakeTorque = 0;
                frontRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
                rearLeftCollider.brakeTorque = 0;
                rearLeftCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
                rearRightCollider.brakeTorque = 0;
                rearRightCollider.motorTorque = (accelerationMultiplier * 50f) * throttleAxis;
            }else{
                ThrottleOff();
            }
        }
    }

    public void ThrottleOff(){
        frontLeftCollider.motorTorque = 0;
        frontRightCollider.motorTorque = 0;
        rearLeftCollider.motorTorque = 0;
        rearRightCollider.motorTorque = 0;
    }

    public void DecelerateCar(){
        if(throttleAxis != 0f){
            if(throttleAxis > 0f){
                throttleAxis = throttleAxis - (Time.deltaTime * 10f);
            }else if(throttleAxis < 0f){
                throttleAxis = throttleAxis + (Time.deltaTime * 10f);
            }
            if(Mathf.Abs(throttleAxis) < 0.15f){
                throttleAxis = 0f;
            }
        }
        carRigidbody.linearVelocity = carRigidbody.linearVelocity * (1f / (1f + (0.025f * decelerationMultiplier)));
        
        ThrottleOff();

        if(carRigidbody.linearVelocity.magnitude < 0.25f){
            carRigidbody.linearVelocity = Vector3.zero;
            CancelInvoke("DecelerateCar");
        }
    }

    public void Brakes(){
        frontLeftCollider.brakeTorque = brakeForce;
        frontRightCollider.brakeTorque = brakeForce;
        rearLeftCollider.brakeTorque = brakeForce;
        rearRightCollider.brakeTorque = brakeForce;
    }
}