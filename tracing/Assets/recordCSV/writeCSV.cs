using Meta.XR.MRUtilityKit;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class writeCSV : MonoBehaviour
{
    public VideoPlayer _Vplayer;
    public Laser leftRay;
    public Laser2 rightRay;

    private string filePath;
    private string customPath;
    private double currentTime = 0;
    private bool isStart = false;
    private List<string[]> data = new List<string[]>
        {
            new string[] { "Frame", "Left eye direction", "Right eye direction"}
        };

    //输入框
    public InputField inputField; // 输入框组件
    public Text displayText;      // 用于显示用户输入的文本
    private int inputNum = 0;//输入的帧数
    void Start()
    {
        // 设置文件路径 (在项目的 Persistent Data Path 中保存)
        customPath = "D:\\unity project\\tracing\\Assets\\CSV";
        filePath = Path.Combine(customPath, "data.csv");
        //设置输入框
        inputField.contentType = InputField.ContentType.IntegerNumber;
        if (inputField != null)
        {
            inputField.onEndEdit.AddListener(OnInputEndEdit);
        }
    }

    private void Update()
    {
        if (_Vplayer != null && _Vplayer.isPlaying)
        {
            double deltaTime = currentTime == 0 ? _Vplayer.time : _Vplayer.time - currentTime;
            if (deltaTime >= 1 / _Vplayer.frameRate)
            {
                string currentFrame = _Vplayer.frame.ToString();
                string leftDir = leftRay.Direction.ToString();
                string rightDir = rightRay.Direction.ToString();
                string[] newData = new string[] { currentFrame, leftDir, rightDir };
                data.Add(newData);//添加新数据
                currentTime = _Vplayer.time;//更新时间
            }
            
        }
    }

    // 写入 CSV 的方法
    void WriteToCSV(List<string[]> data)
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (var row in data)
            {
                string line = string.Join(",", row); // 将每一行数据以逗号分隔
                writer.WriteLine(line);
            }
        }
    }

    public void writeData()
    {
        WriteToCSV(data);
    }

    public void startRecord()
    {
        if (!isStart)
        {
            Debug.Log("1");
            _Vplayer.frame = inputNum;
            _Vplayer.Play();
            isStart = true;
        }
        else
        {
            _Vplayer.Pause();
            isStart= false;
        }
    }

    public void clearData()
    {
        data.Clear();
        data.Add(new string[] { "Frame", "Left eye direction", "Right eye direction" });
    }

    //输入后执行
    private void OnInputEndEdit(string input)
    {
        Debug.Log("Final input: " + input);

        if (displayText != null)
        {
            displayText.text = "You entered: " + input;
        }
        int.TryParse(input, out inputNum);
        if (inputNum < 0) inputNum = 0;
        else if (inputNum > (int)_Vplayer.frameCount) inputNum = (int)_Vplayer.frameCount;
    }
}
