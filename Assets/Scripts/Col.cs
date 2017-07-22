using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Col : MonoBehaviour
{
    public SPlayerMove playerScrp = null;
    [SerializeField]
    SpriteRenderer colColor = null;

    public void SetColor() { colColor.color = new Color(0.69f, 0.56f, 0.51f, 1f); }
    public void ColorReset() { colColor.color = new Color(1f, 1f, 1f, 0f); }
}
