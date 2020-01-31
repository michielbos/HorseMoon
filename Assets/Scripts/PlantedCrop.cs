using UnityEngine;

namespace HorseMoon {

[RequireComponent(typeof(SpriteRenderer))]
public class PlantedCrop : MonoBehaviour {
    private CropData cropData;
    private int age;

    private CropData.GrowthStage CurrentStage => cropData.GetAgeStage(age);

    public void SetCrop(CropData cropData) {
        this.cropData = cropData;
        GetComponent<SpriteRenderer>().sprite = CurrentStage.sprite;
    }

    public void OnNextDay() {
        age++;
        GetComponent<SpriteRenderer>().sprite = CurrentStage.sprite;
    }
}

}