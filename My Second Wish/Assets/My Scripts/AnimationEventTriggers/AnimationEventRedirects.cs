using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventRedirects : MonoBehaviour {


    //calls method to indicate end of animation on parent object
    public void completedAttack() {
        gameObject.transform.GetComponentInParent<CharacterInstanceDataContainer>().animationEndEvent();
    }
}
