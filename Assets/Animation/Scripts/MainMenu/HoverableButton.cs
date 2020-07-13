using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HoverableButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Material hoverMaterial;
    private Image m_image;
    private Material m_materialInstance;

    private float _hovered;

    // Start is called before the first frame update
    void Start()
    {
        m_image = GetComponent<Image>();
        m_materialInstance = Instantiate(hoverMaterial);
        m_image.material = m_materialInstance;

        _hovered = 0.0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _hovered = 1.0f;
        m_materialInstance.SetFloat("_Hovered", 1.0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _hovered = 0.0f;
        m_materialInstance.SetFloat("_Hovered", 0.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
