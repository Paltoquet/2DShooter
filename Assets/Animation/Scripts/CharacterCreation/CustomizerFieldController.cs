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

    public CustomizerData customizerData;

    // Start is called before the first frame update
    void Start()
    {
        leftButton.onClick.AddListener(decrement);
        rightButton.onClick.AddListener(increment);

        if (customizerData != null){
            customizerData.meshDataChanged().AddListener(updateText);
        }
        updateText();
    }

    private void increment()
    {
        if (customizerData != null)
        {
            customizerData.increment();
        }
    }

    private void decrement()
    {
        if (customizerData != null)
        {
            customizerData.decrement();
        }
    }

    private void updateText()
    {
        string currentText = "";
        if (customizerData != null)
        {
            currentText = customizerData.getCurrentMeshId();
        }

        text.text = currentText;
    }
}
