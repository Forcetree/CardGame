using UnityEngine;

public class PaintMat : FieldMat
{
    protected override void PlayAnimation()
    {
        if (stack.Count == 0)
        {
            // Play the empty mat animation -> not implemented yet
        }
        else if(stack.Count == 1)
        {
            // Play the Appear animation -> not implemented yet
        }
        else //stack.Count always greater than 1
        {
            // Play the combo animation -> not implemented yet
        }
        
    }
}

