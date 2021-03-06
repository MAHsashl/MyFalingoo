﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestionMode { Easy, Intermed, Diffy }
public enum QuestionType { Animals, Actions, Colors, Food, Fruits, BodyParts, Weather, Toys, Sports, Clothes, Jobs, Transport, SchoolThings, Objects}
public enum QuestionStruct {Choice, Pic}


[CreateAssetMenu(fileName = "Question", menuName = "", order = 1)]
public class Question : ScriptableObject {
    
    public int QuestionNum;
    public string Title;
    public string Que;
    public QuestionMode Mode;
    public QuestionType type;
    public QuestionStruct structure;
    public int correctAnswer;
    public List<string> options;
    public List<string> picoptions;
    public List<Sprite> pics;
    public int UserGaveRightAnswer;
}


