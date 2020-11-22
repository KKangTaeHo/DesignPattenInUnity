using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 빌더패턴
// - 개체를 생성하는 과정의 약속과, 구체적인 알고리즘을 구현
//   >> 뭔말인지 모름..
// - 개인적으로 느꼈을 땐, 객체의 생성시 생성자 부분에서 매개변수로 넘겨줘야할 케이스가 다양할 경우,
//   이 패턴을 사용하면서 효율적으로 처리하는 것 같음.
public class BuilderEx : MonoBehaviour
{
    // ex. 사람들의 정보를 처리하는 클래스를 작성한다고 했을 때,
    public class Person
    {
        private string _name;
        private int _age;

        public Person(string name , int age) => (_name, _age) = (name, age);
    }

    // 1. 빌더를 생성해서 처리
    public class PersonBuilder{
        private string _name;
        private int _age;

        public PersonBuilder SetName(string name){
            _name = name;
            return this;
        }
        public PersonBuilder SetAge(int age){
            _age = age;
            return this;
        }

        public Person Build()=> new Person(_name,_age);
    }

    private void Start()
    {
        // 빌더를 만들어 사용
        PersonBuilder pb = new PersonBuilder();
        
        Person person = pb.SetAge(15)
                          .SetName("주니어")
                          .Build();
    }

    // 정리
    // - 체인형태로 변수를 설정해주는 느낌이 강함.
    // - c#에서는 '선택적 매개변수' 가 있으니깐, 딱히 빌더를 만들지 않고도 생성자에 다 때려박은 후
    //   객체 생성시 선택주면 되서, 별로 필요 없어보임.
    // - 어쨋든 이런게 있다고 알고 있으면 될거 같음.
}
