using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Entry", menuName ="Catopedia Entry")]
public class CatopediaEntry : ScriptableObject
{
    public new string name;
    public Image image;

    [TextArea(0,15)]
    public string description;
}
