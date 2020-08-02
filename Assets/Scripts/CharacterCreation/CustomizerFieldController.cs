using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CustomizerFieldController : MonoBehaviour
{

    public Button leftButton;
    public Button rightButton;
    public Text text;

    private List<CustomizerData> m_customizerDatas;

    CustomizerFieldController()
    {
        m_customizerDatas = new List<CustomizerData>();
    }

    // Start is called before the first frame update
    void Start()
    {
        leftButton.onClick.AddListener(decrement);
        rightButton.onClick.AddListener(increment);

        foreach (var customizerData in m_customizerDatas) {
            if (customizerData != null) {
                customizerData.meshDataChanged().AddListener(updateText);
            }
        }
        updateText();
    }

    public void addCustomizerData(CustomizerData data)
    {
        m_customizerDatas.Add(data);
        updateText();
    }

    private void increment()
    {
        foreach (var customizerData in m_customizerDatas)
        {
            if (customizerData != null)
            {
                customizerData.increment();
            }
        }
    }

    private void decrement()
    {
        foreach (var customizerData in m_customizerDatas)
        {
            if (customizerData != null)
            {
                customizerData.decrement();
            }
        }
    }

    private void updateText()
    {
        string currentText = "";
        if (m_customizerDatas.Count > 0)
        {
            var customizerData = m_customizerDatas[0];
            if (customizerData != null)
            {
                currentText = customizerData.getCurrentMeshId();
            }
        }
        text.text = currentText;
    }
}
