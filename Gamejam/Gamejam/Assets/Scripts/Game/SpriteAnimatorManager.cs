using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpriteAnimatorManager : MonoBehaviour {

    static public SpriteAnimatorManager instance = null;
   
    [System.Serializable]
    public class CharacterAnim
    {
        public List<Sprite> back = new List<Sprite>();
        public List<Sprite> front = new List<Sprite>();
        public List<Sprite> left = new List<Sprite>();
        public List<Sprite> right = new List<Sprite>();
    }
    public CharacterAnim red;
    public CharacterAnim green;

    void Awake()
    {
        instance = this;
    }
}
