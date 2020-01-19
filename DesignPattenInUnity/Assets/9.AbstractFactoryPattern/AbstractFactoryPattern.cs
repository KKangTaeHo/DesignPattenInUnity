using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractFactoryPattern : MonoBehaviour
{

    // 추상 펙토리 패턴
    // - 구체적인 클래스를 명시하지 않고도 연관되어 있거나 의존적인 객체 패밀리 생성을 위한 인터페이스 제공
    // - 즉 비슷한 기능을 하지만 종류가 다른 클래스들을 생성하는 인터페이스이다.

    // 사용 용도및 장점
    // 1. 펙토리 메서드 패턴의 장점을 거의 동일하게 가짐
    // (추가 되더라도 소스 수정 거의없음, 객체 생성을 한곳에서 관리 등)
    // 2. 연관된 객체를 생성 할 때 실수로 다른 종류의 클래스가 생성이 되는 것을 방지한다.

    // 언뜻 보기에는 펙토리 메소드패턴이랑 같아보인다.. 펙토리 메서드 패턴이랑 무슨차이가 있을까?
    // 펙토리 메서드는 단일 메소드(싱글 메소드)가 여러 종류의 객체를 생성하는 것이고.
    // 추상 펙토리는 오브젝트별 한 종류의 객체를 생성해서, 그 객체를 통해 여러종류의 객체를 생성하는 Factory 효과를 내는 것이다.

    // 추상 팩토리 패턴은 어떻게 보면, 팩토리 메서드 패턴을 좀더 캡슐화한 방식이라 볼 수 있음.

    // ex. 컴퓨터 제조를 예로 들었을 경우
    public interface Keyboard
    {
        void Create();
    }

    public class AppleKeyboard : Keyboard
    {
        public void Create()
        {
            Debug.Log("Apple 키보드 생성");
        }
    }

    public class LGKeyboard : Keyboard
    {
        public void Create()
        {
            Debug.Log("LG 키보드 생성");
        }
    }

    public interface Mouse
    {
        void Create();
    }

    public class AppleMouse : Mouse
    {
        public void Create()
        {
            Debug.Log("Apple 마우스 생성");
        }
    }

    public class LGMouse : Mouse
    {
        public void Create()
        {
            Debug.Log("LGMouse 마우스 생성");
        }
    }

    // 2. 키보드 마우스를 관리하는 Factory 클래스 생성
    public class KeyboardFactory
    {
        public Keyboard CrateKeyboard(string inType)
        {
            switch (inType)
            {
                case "LG": return new LGKeyboard();
                case "Apple": return new AppleKeyboard();
            }

            return null;
        }
    }
    public class MouseFactory
    {
        public Mouse CrateMouse(string inType)
        {
            switch (inType)
            {
                case "LG": return new LGMouse();
                case "Apple": return new AppleMouse();
            }

            return null;
        }
    }

    // 3. 컴퓨터 FactoryClass를 구현함.
    // 여기서 해당 브렌드에 맞는 마우스와 키보드가 호출됨.
    public class ComputerFactory
    {
        public void CreateComputer(string inType)
        {
            KeyboardFactory kf = new KeyboardFactory();
            MouseFactory mf = new MouseFactory();

            kf.CrateKeyboard(inType);
            mf.CrateMouse(inType);
        }

        // 추상 팩토리 패턴을 위한 마우스 키보드 생성 메소드 작성
        public virtual Keyboard CreateKeyboard() { return null; }
        public virtual Mouse CreateMouse() { return null; }
    }

    // 위의 패턴이 '팩토리 메서드 패턴'이다.
    // 제품마다 펙토리를 생성하고, 그 팩토리를 전체 관리하는 거대 클래스에서(Computer 클래스)에서
    // 타입이 정해지면 그 타입에 맞도록 객체들이 생성되는 것을 볼 수 있다.

    // 그런데, 컴퓨터에는 키보드, 마우스 외에도 여러가지(스피커, 프린터 등등)가 있어서, 제품의 구성품이 늘어날 때 마다,
    // 계속 객체를 만들어주면 된다.

    // 그런데, LG나 Apple 컴퓨터라면, 컴퓨터 구성품 자체가 모두 자기 회사의 모델이기 때문에
    // 매번 구성품을 생성할 때마다 type로 구별할 필요 없이, 일관된 방식으로 구성품을 생성하면 된다.

    // 따라서 '추상 팩토리 패턴을 적용'하여 구성품이 모두 동일한 제조사가 되도록 제작한다.

    // 4. 제조사 컴퓨터 Factory 정의
    public class AppleComputer : ComputerFactory
    {
        public override Keyboard CreateKeyboard()
        {
            return new AppleKeyboard();
        }

        public override Mouse CreateMouse()
        {
            return new AppleMouse();
        }
    }

    public class LGComputer : ComputerFactory
    {
        public override Keyboard CreateKeyboard()
        {
            return new LGKeyboard();
        }

        public override Mouse CreateMouse()
        {
            return new LGMouse();
        }
    }

    // 5. 패턴 적용전의 'ComputerFactory' 클래스와 같은 기능을 하는 클래스 작성
    public class Factory
    {
        public void CreateComputer(string inType)
        {
            ComputerFactory cf = null;  // 단일 오브젝트
            switch (inType)
            {
                case "LG": cf = new LGComputer(); break;
                case "Apple": cf = new AppleComputer(); break;
            }

            // 제조품 생성
            cf.CreateKeyboard();
            cf.CreateMouse();
        }
    }

    // 포인트는 '추상 메서드 패턴'의 반복적인 타입 선택을 하도록 하지 않고, 그 타입과 연관되어있는 그룹을 따로 만들어
    // 해당 타입을 선택했을 때, 일괄적으로 모든 오브젝트(여기서는 부품)이 생성되도록 하는 것

}