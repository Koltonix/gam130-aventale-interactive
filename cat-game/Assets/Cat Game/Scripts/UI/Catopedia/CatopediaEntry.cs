using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Entry", menuName ="Catopedia Entry")]
public class CatopediaEntry : ScriptableObject
{
    public new string name;
    public Sprite sprite;

    [TextArea(0,15)]
    public string description;
}
