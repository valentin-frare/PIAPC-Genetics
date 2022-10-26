using UnityEngine;
using UnityEngine.UI;

public class EmpanadaObject : MonoBehaviour 
{
    [SerializeField] private RawImage flavor1UI;
    [SerializeField] private RawImage flavor2UI;
    [SerializeField] private RawImage flavor3UI;
    [SerializeField] private RawImage niceUI;

    public Color flavor1;
    public Color flavor2;
    public Color flavor3;
    public bool nice;

    private void Update() 
    {
        flavor1UI.color = flavor1;
        flavor2UI.color = flavor2;
        flavor3UI.color = flavor3;
    }
}