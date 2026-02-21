using UnityEngine;
using System.IO;
using System.Text;

public class playermoveE : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f; // سرعة اللفان بالدرجة

    string filePath;
    float yRotation;

    void Start()
    {
        filePath = Application.dataPath + "/serial_output.txt";
        yRotation = transform.eulerAngles.y;
    }

    void Update()
    {
        if (!File.Exists(filePath)) return;

        string data = "";

        try
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
            {
                data = reader.ReadToEnd().Trim().ToLower();
            }
        }
        catch { return; }

        if (string.IsNullOrEmpty(data) || data == "stop") return;

        string[] commands = data.Split(',');

        bool forward = false;
        bool backward = false;
        bool left = false;
        bool right = false;

        foreach (string cmd in commands)
        {
            string c = cmd.Trim();

            if (c == "forward") forward = true;
            if (c == "backward") backward = true;
            if (c == "left") left = true;
            if (c == "right") right = true;
        }

        // 🔄 دوران
        if (left)
            yRotation -= rotationSpeed * Time.deltaTime;

        if (right)
            yRotation += rotationSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        // 🚶‍♂️ حركة
        Vector3 move = Vector3.zero;

        if (forward)
            move += transform.forward;

        if (backward)
            move -= transform.forward;

        if (move != Vector3.zero)
            transform.position += move.normalized * moveSpeed * Time.deltaTime;
    }

}
