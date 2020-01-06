using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;

public class ObserverPatten : MonoBehaviour
{
    
    // 관찰자 패턴(Observer Patten)
    // 한 객체의 상태 변화에 따라 다른 객체의 상태도 연동되도록 일대다 객체 의존 관계를 구성하는 패턴 (무슨말..?)
    // 데이터의 변경이 발생 했을 경우 '상대 클래스나 객체에 의존하지 않으면서 데이터 변경을 통보'하고자 할 때 유용하다.

    // 예를 들어, 퀘스트를 만든다고 가정했을때
    // '괴물 100마리 죽이기', '강화 3회하기', '스킬 3단계 도달하기' 등의 퀘스트를 성공했을 때 휘장을 받는다고 한다면
    // 퀘스트 종류가 광범위하고 달성하는 방법도 다양하다 보니 깔끔하게 구현하기 어렵다.
    // 특정 기능을 담당하는 코드는 항상 모아두는게 좋은데... 문제는 퀘스트 요소가 여러군데에서 발생할 수 있다는 것이다.
    // 이럴 때 '관찰자 패턴'을 쓰면 좋음.

    // 관찰자 패턴을 사용하면 어떤 코드에서 흥미로운 일이 생겼을 때,
    // 누가 받든 상관없이 알림을 보낼 수 있다.

    // 관찰자 패턴을 사용하는 이유는 '두 코드간의 결합을 최소화 하기위해서' 이다.
    // 다른 관찰자와 정적으로 묶이지 않고 간접적인 상호작용을 할 수 있다.
    // 하지만 단점은, 프로그램이 재대로 동작 하지 않는 경우 버그가 여려 관찰자에 퍼저 있다면 상호작용 흐름을 추론하기가 훨씬 어렵다.

    // 관찰자 패턴은 대상(Subject)가 어떤일이 발생했을 경우, 관찰자(Observer)의 이벤트를 발생하게 하는 패턴이다.

    // C#에는 언어자체에 event가 있어서 메서드를 참조하는 delegate으로 관찰자를 등록할 수 있으며, 자바스크립트 이벤트 시스템에서는
    // EventListener 프로토콜을 지원하는 객체가 관찰자가 되는데, 이것 역시 함수로 할 수 있고 다들 그렇게 쓴다.

    // 결론 : 관찰자 패턴을 요런식으로 C#에서 쓰면 ㅄ임

    void Start()
    {
        // 1. 관찰자 생성
        CubeActor ca = new CubeActor();
        SphereActor sa = new SphereActor();
        // 2. 구독자? 생성(대상자)
        Subject subject = new Subject();
        // 3. 관찰자 등록
        subject.AddObserver(ca);
        subject.AddObserver(sa);
        // 4. 테스트 실행
        subject.StartThread();

        // 관찰되고 있는 대상이 어떤 조건을 만족하면, 관찰자의 메소드를 호출시키는 개념이다.
        // 관찰자 패턴은 특정 코드를 다른 코드와 섞이거나 엮이지 않고 '디커플링' 시킬 때 아주 많이 쓰인다.
    }
    
    // A. 관찰자 클래스 생성
    public abstract class Observer
    {
        public abstract void OnNotify();
    }

    // B. 관찰자 등록을 위한 인터페이스 생성
    public interface IObserverSubject
    {
        void Notify();
        void AddObserver(Observer ob);
        void RemoveObserver(Observer ob);
    }

    // C. 관찰자를 상속 받는 실체가 있는 클래스 생성
    // 어떤 클래스든 Observer를 상속 받으면 관찰자가 될 수 있음.
    public class CubeActor : Observer
    {
        public override void OnNotify()=> Debug.Log("CubeActor의 OnNotify 호출!");
    }

    public class SphereActor:Observer
    {
        public override void OnNotify() => Debug.Log("SphereActor의 OnNotify 호출!");
    }

    // D. Subject 클래스 생성
    // 관찰 당하고 있는 객체
    public class Subject : IObserverSubject
    {
        List<Observer>  _observerList = new List<Observer>();

        public void AddObserver(Observer ob) => _observerList.Add(ob);
        public void Notify() => _observerList.ForEach(o => o.OnNotify());
        public void RemoveObserver(Observer ob) => _observerList.Remove(ob);

        // 테스트를 위한 스레드 생성
        public void StartThread()
        {
            Thread mainThread = new Thread(() =>
            {
                int cnt = 0;
                Thread subThread = new Thread(() =>
                {
                    while (true)
                    {
                        if (cnt > 5)
                            break;

                        Notify();
                        cnt++;

                        Thread.Sleep(1000);
                    }
                });
                subThread.Start();
            });

            mainThread.Start();
        }
    }

    // E. Delegate를 활용한 Subject 클래스 생성
    // GoF 디자인 패턴은 94년도에 나와서 많이 구식일 수 있다.
    // C#의 Delegate를 활용하면 좀더 깔끔하게 구현 할 수 있다.
    public class SubjectInDelegate
    {
        delegate void MoveHandler();
        MoveHandler _moveHandler;

        public void AddHandler(Observer ob) => _moveHandler += new MoveHandler(ob.OnNotify);
    }

    // F. event를 활용해서 Subject 클래스 생성
    public class SubjectInEvent
    {
        public event EventHandler eventHandler;

        public void CallbackEventParamNULL()
        {
            eventHandler(this, null);
        }

        public void CallbackEventParam()
        {
            eventHandler(this, new EventArgInSubject("옹옹"));
        }
    }

    public class EventArgInSubject:EventArgs
    {
        private string _name;

        public EventArgInSubject(string inName)
        {
            _name = inName;
        }
    }
}
