using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class AnimationDatas : MonoBehaviour{

    public static AnimationDatas instance;

    public enum AnimationStates {
        Idle,
        Walk,
        Attack,
        Die,
        Born
    }

    [System.Serializable]
    public class AnimationData {
        public AnimationStates state;
        public AnimationClip clip;
        public float animLength;
    }

    public AnimationData[] animationDatas;
        
    void Awake() {
        if (instance == null)
            instance = this;
    }

    void Update() {
        foreach (var animationData in animationDatas)
        {
            animationData.animLength = animationData.clip.length;
            Debug.Log("Running");
        }
    }

    float GetAnimationLength(AnimationStates state) {
        return animationDatas.Where(clip => clip.state == state).SingleOrDefault().animLength;
    }

}
