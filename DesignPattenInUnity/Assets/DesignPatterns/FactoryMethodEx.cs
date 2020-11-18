using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// 팩토리 메서드 패턴
// - 파생 클래스가 어떤 객체를 인스턴스화 할지를 결정하도록 한다.
// - 기반 클래스가 모든(혹은, 대부분의)일을 하지만 정확히 어떤 객체를 갖고 작업할지에 대해서는
//   실행도중(런타임)로 미룰 때 유용한 패턴임.
public class FactoryMethodEx : MonoBehaviour
{

    // 1. 컴포넌트와 그애 따른 파생 클래스 생성
    public abstract class MyComponent
    {
        protected abstract string GetCompName();
    }

    public class MyButton : MyComponent
    {
        protected override string GetCompName() => "버튼";
    }

    public class Switch : MyComponent
    {
        protected override string GetCompName() => "스위치";
    }

    public class Dropdown : MyComponent
    {
        protected override string GetCompName() => "드롭다운";
    }

    // 2. 컴포넌트 생성을 위한 팩토리 클래스 생성
    public class ComponentFactory{
        public T GetComponent<T>() where T : MyComponent
        {
             T t = default(T);
             if(t is MyButton)
             {
                 t = new MyButton() as T;
             }
             else if(t is Switch)
             {
                 t = new Switch() as T;
             }
             else if(t is Dropdown)
             {
                 t = new Dropdown() as T;
             }
             return t;
        }
    }

    // 3. 객체 생성이 필요한 클래스 생성
    public class MyConsole
    {
        protected ComponentFactory factory = new ComponentFactory();

        MyComponent comp1;
        MyComponent comp2;
        MyComponent comp3;

        // 4. 팩토리메서드 패턴을 사용하는 이유는 크게 2가지로 나눌 수 있는데,
        // 첫째, 컴포넌트를 생성하는 객체가 여러군데에 사용되고 있을 때, 객체 초기화가 변경되거나, 다른 추가 코드를
        //      작성해주어야 한다면, 일일이 사용되는 곳을 찾아서 변경해주어야 한다. 이런 번거로움을 없앨 수 있다.

        public void CreateComponent()
        {
            comp1 = factory.GetComponent<MyButton>();
            comp2 = factory.GetComponent<Switch>();
            comp3 = factory.GetComponent<Dropdown>();
        }

    }
}
