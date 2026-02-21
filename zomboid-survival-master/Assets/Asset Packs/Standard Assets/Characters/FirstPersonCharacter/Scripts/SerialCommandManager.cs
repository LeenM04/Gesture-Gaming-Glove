using UnityEngine;
using System.IO;

public class SerialCommandManager : MonoBehaviour
{
    public static SerialCommandManager Instance;

    private string filePath = @"C:\Users\user\Desktop\zomboid-survival-master\zomboid-survival-master\Assets\serial_output.txt";
    private string lastCommand = "";
    private bool commandHandled = false;

    private float shootHoldDuration = 0.5f; // Duration to keep "Shoot" active
    private float shootTimestamp = -1f;

    public float GyroY { get; private set; } // Y-axis value from Gyroscope

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;

            if (transform.parent == null)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning("SerialCommandManager must be on a root GameObject for DontDestroyOnLoad to work.");
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        ReadFromFile(); // Update gyro and commands every frame
        commandHandled = false;
    }

    private void ReadFromFile()
    {
        if (!File.Exists(filePath)) return;

        string[] lines;

        using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var sr = new StreamReader(fs))
        {
            string content = sr.ReadToEnd();
            lines = content.Split(new[] { "\r\n", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        if (lines.Length == 0) return;

        for (int i = lines.Length - 1; i >= 0; i--)
        {
            string line = lines[i].Trim();

            // Parse Gyroscope Y value
            if (line.StartsWith("Gyroscope:"))
            {
                string[] parts = line.Split('|');
                foreach (string part in parts)
                {
                    if (part.Trim().StartsWith("Y"))
                    {
                        string[] kv = part.Trim().Split('=');
                        if (kv.Length == 2 && float.TryParse(kv[1].Trim(), out float yVal))
                        {
                            GyroY = yVal;
                        }
                    }
                }
                // Continue to parse commands in case they appear below
            }

            // Handle commands, with special treatment for "Shoot"
            if (line == "Shoot")
            {
                lastCommand = "Shoot";
                shootTimestamp = Time.time;
                break;
            }
            else if (!line.StartsWith("Gyroscope:") && Time.time - shootTimestamp > shootHoldDuration)
            {
                lastCommand = line;
                break;
            }
        }

        // Clear "Shoot" command after hold duration
        if (lastCommand == "Shoot" && Time.time - shootTimestamp > shootHoldDuration)
        {
            lastCommand = "";
        }
    }

    // Return the last command read
    public string GetLastCommand()
    {
        return lastCommand;
    }

    // Check if the given command is new (not yet handled)
    public bool IsCommandNew(string command)
    {
        return lastCommand == command && !commandHandled;
    }

    // Mark the current command as handled
    public void MarkCommandHandled()
    {
        commandHandled = true;
        // Optional: clear file or reset state here if needed
    }
}