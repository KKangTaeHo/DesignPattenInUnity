using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 탬플릿 메소드 패턴
// 어떤 같은 형식을 지닌 특정 작업들의 세부 방식을 다양화 하고자 할 때 사용하는 패턴
// - 어떤 일을 수행 하는 몇가지 방법이 있는데, 그 전반적인 과정에 공통적인 절차가 있을 때 사용한다.
// - 단순한 부모 자식의 오버라이딩이라고 생각 할 수 있겟지만, 부모단에서 작업과정을 설정해놓고,
//   각각의 자식에서 그 부분부분의 작업들을 세분화 한다고 생각하면 된다.
public class TemplateMethodEx : MonoBehaviour
{
    // ex. 맵 뷰를 노출시키는 코드를 작성한다고 할 때, 처음 초기화에서는
    // 1. 현재 맵 연결
    // 2. 맵 노출
    // 3. 현재 자신의 위치 노출
    // 이렇게 3가지는 공통된 형식으로 이루어지는데, 세부적인 과정 절차는 각각의 서비스 api 마다 다를
    // 수 있다. 이럴 때 탬플릿 메소드 패턴을 사용하면 된다.

    public abstract class MapView
    {
        protected abstract void ConnectMapService();
        protected abstract void ShowMapOnScreen();
        protected abstract void MoveToCurrentLocation();

        public void InitMap()
        {
            // 기반클래스에서 알고리즘을 정의
            ConnectMapService();
            ShowMapOnScreen();
            MoveToCurrentLocation();
        }
    }

    public class NaverMapView : MapView
    {
        public NaverMapView()
        {

        }

        protected override void ConnectMapService()
        {
            
        }

        protected override void MoveToCurrentLocation()
        {

        }

        protected override void ShowMapOnScreen()
        {

        }
    }

    public class KakaoMapView : MapView
    {
        public KakaoMapView()
        {
        }

        protected override void ConnectMapService()
        {
        }

        protected override void MoveToCurrentLocation()
        {
        }

        protected override void ShowMapOnScreen()
        {
        }
    }
}
