using UnityEngine;
using System.Collections;

public class KeyMap {

    /* game keys */
    public static KeyCode ToggleGUI = KeyCode.H;


#if UNITY_EDITOR
    /* editor keys */
    public static KeyCode BlockNode = KeyCode.B;
    public static KeyCode UnblockNode = KeyCode.U;
#endif
}