using UnityEngine;
using UnityEngine.Tilemaps;

namespace HorseMoon {

[RequireComponent(typeof(SpriteRenderer))]
public class PlantedCrop : MonoBehaviour {
    private CropData cropData;
    private int age;
    private bool dehydrated;
    private bool dead;

    private CropData.GrowthStage CurrentStage => cropData.GetAgeStage(age);

    public void SetCrop(CropData cropData) {
        this.cropData = cropData;
        UpdateSprite();
    }

    public void OnNextDay(bool isWatered) {
        if (isWatered) {
            dehydrated = false;
            age++;
        } else if (!dehydrated) {
            dehydrated = true;
        } else {
            dead = true;
        }
        UpdateSprite();
    }

    private void UpdateSprite() {
        GetComponent<SpriteRenderer>().sprite = dead ? cropData.deadSprite : CurrentStage.sprite;
    }
}

}