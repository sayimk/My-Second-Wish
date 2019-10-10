using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this class represents a single text line or a sentense that a character will speak.
public class TextLine {

    private string line;
    private string speaker;

    public TextLine() {
        line = "";
        speaker = "";
    }

    public TextLine(string speaker, string line) {
        this.line = line;
        this.speaker = speaker;
    }

    public string getSpeaker() {
        return speaker;
    }

    public void setSpeaker(string speaker) {
        this.speaker = speaker;
    }

    public string getLine() {
        return line;
    }

    public void setLine() {
        this.line = line;
    }
}
