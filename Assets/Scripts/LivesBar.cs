 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesBar : MonoBehaviour
{
 private Character character;
 [SerializeField] private Image[] images;

 private void Awake()
 {
  character = FindObjectOfType<Character>();
 }

 public void Refresh()
 {
  for (int i = 0; i < images.Length; i++)
  {
   if (i < character.Lives)
    images[i].color = Color.white;
   else
    images[i].color = Color.gray;
  }
 }
}
