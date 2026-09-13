using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PrometeoCarController))]
[System.Serializable]
public class PrometeoEditor : Editor
{
    private SerializedObject SO;

    // CAR SETUP
    private SerializedProperty maxSpeed;
    private SerializedProperty maxReverseSpeed;
    private SerializedProperty accelerationMultiplier;
    private SerializedProperty maxSteeringAngle;
    private SerializedProperty steeringSpeed;
    private SerializedProperty brakeForce;
    private SerializedProperty decelerationMultiplier;
    private SerializedProperty bodyMassCenter;

    // WHEELS
    private SerializedProperty frontLeftMesh;
    private SerializedProperty frontLeftCollider;
    private SerializedProperty frontRightMesh;
    private SerializedProperty frontRightCollider;
    private SerializedProperty rearLeftMesh;
    private SerializedProperty rearLeftCollider;
    private SerializedProperty rearRightMesh;
    private SerializedProperty rearRightCollider;

    // UI
    private SerializedProperty useUI;
    private SerializedProperty carSpeedText;

    // SOUNDS
    private SerializedProperty useSounds;
    private SerializedProperty carEngineSound;
    private SerializedProperty tireScreechSound;

    private void OnEnable()
    {
        SO = new SerializedObject(target);

        // CAR SETUP
        maxSpeed = SO.FindProperty("maxSpeed");
        maxReverseSpeed = SO.FindProperty("maxReverseSpeed");
        accelerationMultiplier = SO.FindProperty("accelerationMultiplier");
        maxSteeringAngle = SO.FindProperty("maxSteeringAngle");
        steeringSpeed = SO.FindProperty("steeringSpeed");
        brakeForce = SO.FindProperty("brakeForce");
        decelerationMultiplier = SO.FindProperty("decelerationMultiplier");
        bodyMassCenter = SO.FindProperty("bodyMassCenter");

        // WHEELS
        frontLeftMesh = SO.FindProperty("frontLeftMesh");
        frontLeftCollider = SO.FindProperty("frontLeftCollider");
        frontRightMesh = SO.FindProperty("frontRightMesh");
        frontRightCollider = SO.FindProperty("frontRightCollider");
        rearLeftMesh = SO.FindProperty("rearLeftMesh");
        rearLeftCollider = SO.FindProperty("rearLeftCollider");
        rearRightMesh = SO.FindProperty("rearRightMesh");
        rearRightCollider = SO.FindProperty("rearRightCollider");

        // UI
        useUI = SO.FindProperty("useUI");
        carSpeedText = SO.FindProperty("carSpeedText");

        // SOUNDS
        useSounds = SO.FindProperty("useSounds");
        carEngineSound = SO.FindProperty("carEngineSound");
        tireScreechSound = SO.FindProperty("tireScreechSound");
    }

    public override void OnInspectorGUI()
    {
        SO.Update();

        // CAR SETUP
        GUILayout.Space(25);
        GUILayout.Label("CAR SETUP", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        maxSpeed.intValue = EditorGUILayout.IntSlider("Max Speed:", maxSpeed.intValue, 20, 190);
        maxReverseSpeed.intValue = EditorGUILayout.IntSlider("Max Reverse Speed:", maxReverseSpeed.intValue, 10, 120);
        accelerationMultiplier.intValue = EditorGUILayout.IntSlider("Acceleration Multiplier:", accelerationMultiplier.intValue, 1, 10);
        maxSteeringAngle.intValue = EditorGUILayout.IntSlider("Max Steering Angle:", maxSteeringAngle.intValue, 10, 45);
        steeringSpeed.floatValue = EditorGUILayout.Slider("Steering Speed:", steeringSpeed.floatValue, 0.1f, 1f);
        brakeForce.intValue = EditorGUILayout.IntSlider("Brake Force:", brakeForce.intValue, 100, 600);
        decelerationMultiplier.intValue = EditorGUILayout.IntSlider("Deceleration Multiplier:", decelerationMultiplier.intValue, 1, 10);
        EditorGUILayout.PropertyField(bodyMassCenter, new GUIContent("Mass Center of Car: "));

        // WHEELS
        GUILayout.Space(25);
        GUILayout.Label("WHEELS", EditorStyles.boldLabel);
        GUILayout.Space(10);

        EditorGUILayout.PropertyField(frontLeftMesh, new GUIContent("Front Left Mesh: "));
        EditorGUILayout.PropertyField(frontLeftCollider, new GUIContent("Front Left Collider: "));
        EditorGUILayout.PropertyField(frontRightMesh, new GUIContent("Front Right Mesh: "));
        EditorGUILayout.PropertyField(frontRightCollider, new GUIContent("Front Right Collider: "));
        EditorGUILayout.PropertyField(rearLeftMesh, new GUIContent("Rear Left Mesh: "));
        EditorGUILayout.PropertyField(rearLeftCollider, new GUIContent("Rear Left Collider: "));
        EditorGUILayout.PropertyField(rearRightMesh, new GUIContent("Rear Right Mesh: "));
        EditorGUILayout.PropertyField(rearRightCollider, new GUIContent("Rear Right Collider: "));

        // UI
        GUILayout.Space(25);
        GUILayout.Label("UI", EditorStyles.boldLabel);
        GUILayout.Space(10);

        useUI.boolValue = EditorGUILayout.BeginToggleGroup("Use UI (Speed text)?", useUI.boolValue);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(carSpeedText, new GUIContent("Speed Text (UI): "));
        EditorGUILayout.EndToggleGroup();

        // SOUNDS
        GUILayout.Space(25);
        GUILayout.Label("SOUNDS", EditorStyles.boldLabel);
        GUILayout.Space(10);

        useSounds.boolValue = EditorGUILayout.BeginToggleGroup("Use sounds (car sounds)?", useSounds.boolValue);
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(carEngineSound, new GUIContent("Car Engine Sound: "));
        EditorGUILayout.PropertyField(tireScreechSound, new GUIContent("Tire Screech Sound: "));
        EditorGUILayout.EndToggleGroup();

        // END
        GUILayout.Space(10);
        SO.ApplyModifiedProperties();
    }
}