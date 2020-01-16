using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterpreterPattern : MonoBehaviour
{
    // 해석자 패턴(Interpreter Pattern)
    // 행동 패턴중의 하나
    // 언어의 문법 또는 표현을 평가하는 패턴
    // 사용자가 표현하기 쉬운 표현을 사용하게 하고 이를 해석하는 개체를 통해 약속된 알고리즘을 수행할 수 있게 해 주는 패턴이다.

    // 해석자 패턴을 사용하면 사용자가 명령을 쉬운 표현방법으로 전달 할 수 있다.

    // 예를 들어, 실행하고 싶은 프로그래밍 언어가
    // (1+2)*(3-4)
    // 로 되어있을때, 해석자 패턴을 사용하면 깔끔하게 표현 할 수 있다.

    // 1. 최상위 인터페이스 생성
    public interface Expression
    {
        double Evaluate();
    }

    // 2. 숫자를 표현하는 클래스 생성
    public class NumberExpression : Expression
    {
        private double _value;
        public NumberExpression(double inVar)
        {
            _value = inVar;
        }
        public double Evaluate() => _value;
    }

    // 3. 덧샘을 표현하는 클래스(곱샘은 따로구현하면 됨)
    public class AddExpression : Expression
    {
        private Expression _left;
        private Expression _right;
        public AddExpression(Expression inLeft, Expression inRight)
        {
            _left = inLeft;
            _right = inRight;
        }
        public double Evaluate() => _left.Evaluate() + _right.Evaluate();
    }

    // 반면에, 해석자 패턴에도 단점이 있는데
    // 1. 너무 많은 명령에 대한 조합을 해석자 패턴표현하게 되면 정규화 과정에 들어가는 비용이 기하급수적으로 커질 수 있다.
    // 2. 반복적인 참조를 통해 하위표현식에 접근해야하기 때문에 어려움이 있따.
    // 그래서 많이 느리다.
}
