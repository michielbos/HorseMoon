namespace HorseMoon {

public class TimeController : SingletonMonoBehaviour<TimeController> {

    public void NextDay() {
        CropManager.Instance.OnDayPassed();
        TilemapManager.Instance.OnDayPassed();
    }
}

}