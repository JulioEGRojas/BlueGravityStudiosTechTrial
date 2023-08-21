using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tutorial Images", menuName = "Tutorial/Images")]
public class InstructionImages : ScriptableObject {
    
    public Sprite moveImage;
    
    public Sprite rotateImage;

    public Sprite selectFurnitureImage;
    
    public Sprite moveFurnitureImage;
    
    public Sprite rotateFurnitureImage;
    
    public Sprite deleteFurniture;
    
    public Sprite selectStructureImage;
    
    public Sprite unselectStructureImage;
}
