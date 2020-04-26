﻿using Anima2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class CustomizerData
{
    public List<string> availableMesheIds;
    public List<SpriteMesh> availableMeshes;

    private UnityEvent meshChanged;

    private int m_currentIndex;

    public CustomizerData(List<string> ids, List<SpriteMesh> spriteMeshes)
    {
        availableMesheIds = ids;
        availableMeshes = spriteMeshes;

        m_currentIndex = 0;
        meshChanged = new UnityEvent();
    }

    public void increment()
    {
        m_currentIndex = m_currentIndex < availableMesheIds.Count - 1 ? m_currentIndex + 1 : 0;
        meshChanged.Invoke();
    }

    public void decrement()
    {
        m_currentIndex = m_currentIndex > 0 ? m_currentIndex - 1 : availableMesheIds.Count - 1;
        meshChanged.Invoke();
    }

    public string getCurrentMeshId()
    {
        return availableMesheIds[m_currentIndex];
    }

    public SpriteMesh getCurrentMesh()
    {
        return availableMeshes[m_currentIndex];
    }

    public UnityEvent meshDataChanged()
    {
        return meshChanged;
    }

}
