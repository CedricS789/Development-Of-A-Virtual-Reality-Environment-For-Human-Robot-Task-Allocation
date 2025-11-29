using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;

/// <summary>
/// A simple "Hello World" script for the Augment-X Setup.
/// It sends the position of this object to ROS 2 times per second.
/// </summary>
public class SourceDestinationPublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "pos_rot";

    // Publish the cube's position and rotation every 0.5 seconds
    public float publishMessageFrequency = 0.5f;

    // Used to determine how much time has elapsed since the last message was published
    private float timeElapsed;

    void Start()
    {
        // Get the connection handle
        ros = ROSConnection.GetOrCreateInstance();
        
        // Tell ROS that we will be sending "Pose" messages on this topic
        ros.RegisterPublisher<PoseMsg>(topicName);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed > publishMessageFrequency)
        {
            // Create a standard ROS "Pose" message (Position + Orientation)
            // We convert the Unity coordinate system (Left Handed) to ROS (Right Handed) automatically
            PoseMsg pose = new PoseMsg
            {
                position = transform.position.To<FLU>(), // FLU = Forward-Left-Up coordinate conversion
                orientation = transform.rotation.To<FLU>()
            };

            // Send the message to Linux!
            ros.Publish(topicName, pose);

            timeElapsed = 0;
        }
    }
}