namespace HorseMoon {

public class TimeController : SingletonMonoBehaviour<TimeController> {

    public void NextDay() {
        TilemapManager.Instance.OnDayPassed();
    }
}

}