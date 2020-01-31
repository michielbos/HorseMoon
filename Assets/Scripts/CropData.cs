using System;
using UnityEngine;

namespace HorseMoon {

[CreateAssetMenu(fileName = "CropData", menuName = "HorseMoon/Crop Data")]
public class CropData : ScriptableObject {
    public GrowthStage[] stages;
    public Sprite deadSprite;
    
    [Serializable]
    public class GrowthStage {
        public Sprite sprite;
        public int days;
    }

    public GrowthStage GetAgeStage(int age) {
        int neededDays = 0;
        foreach (GrowthStage stage in stages) {
            neededDays += stage.days;
            if (neededDays > age)
                return stage;
        }
        return stages[stages.Length - 1];
    }
}

}