#if (UNITY_EDITOR)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemSO), true)]
public class ItemSOEditor : Editor
{
    public bool ThisIsATestBool;

    public ItemDataBaseSO ItemDataBase;
    private bool isInitiated = false;
    private bool currentToggleValue;
    private bool lastToggleValue;


    // https://forum.unity.com/threads/automatically-add-scriptableobject-asset-to-a-list-on-game-start.802458/#post-5331858
    public override void OnInspectorGUI()
    {        
        ItemSO item = (ItemSO)target;

        base.OnInspectorGUI();

        //Debug.Log("We are executing editor script");

        if (!isInitiated)
        {
            ItemDataBase = (ItemDataBaseSO)AssetDatabase.LoadAssetAtPath("Assets/Scripts/Items/DataBase/SpaceMaster2000_ItemDataBase.asset", typeof(ItemDataBaseSO));
            currentToggleValue = item.CheckIfDataBaseAlreadyContainsItem(ItemDataBase);
            lastToggleValue = currentToggleValue;
            isInitiated = true;
        }

        currentToggleValue = GUILayout.Toggle(currentToggleValue, "Item is in DataBase", GUILayout.Height(20));

        if (lastToggleValue != currentToggleValue)
        {
            if (currentToggleValue)
            {
                if (!item.CheckIfDataBaseAlreadyContainsItem(ItemDataBase))
                {
                    item.AddToDataBase(ItemDataBase);
                }
            }                
            
            else
            {
                item.RemoveFromDataBase(ItemDataBase);
            }
                
                EditorUtility.SetDirty(ItemDataBase);

            lastToggleValue = currentToggleValue;
        }
    }
}
#endif