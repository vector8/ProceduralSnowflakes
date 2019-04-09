using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSystem : MonoBehaviour
{
    private struct State
    {
        public State(Vector3 point, Vector3 dir)
        {
            currentPoint = point;
            direction = dir;
        }

        public Vector3 currentPoint;
        public Vector3 direction;
    }

    public string axiom = "F[[---F][--F][-F]]++F[[---F][--F][-F]]++F[[---F][--F][-F]]++F[[---F][--F][-F]]++F[[---F][--F][-F]]++F[[---F][--F][-F]]";
    public Dictionary<char, string> rules = new Dictionary<char, string>();
    public float angle = 30f;

    private string current;
    private float length = 1f;
    private Stack<State> stack = new Stack<State>();

    // Start is called before the first frame update
    void Start()
    {
        rules.Add('F', "F[-F][F][+F]");
        current = axiom;
        stack.Push(new State(Vector3.zero, new Vector3(0, length, 0)));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            iterate();
            print(current);
            //length = length * 0.5f;
        }

        State initialState = stack.Pop();
        initialState.direction = new Vector3(0, length, 0);
        stack.Push(initialState);

        for (int i = 0; i < current.Length; i++)
        {
            switch (current[i])
            {
                case 'F':
                    {
                        State currentState = stack.Pop();
                        Debug.DrawLine(currentState.currentPoint, currentState.currentPoint + currentState.direction);
                        currentState.currentPoint += currentState.direction;
                        stack.Push(currentState);
                    }
                    break;
                case '-':
                    {
                        State currentState = stack.Pop();
                        currentState.direction = Quaternion.Euler(0, 0, -angle) * currentState.direction;
                        stack.Push(currentState);
                    }
                    break;
                case '+':
                    {
                        State currentState = stack.Pop();
                        currentState.direction = Quaternion.Euler(0, 0, angle) * currentState.direction;
                        stack.Push(currentState);
                    }
                    break;
                case '[':
                    {
                        State newState = new State(stack.Peek().currentPoint, stack.Peek().direction);
                        stack.Push(newState);
                    }
                    break;
                case ']':
                    stack.Pop();
                    break;
                default:
                    break;
            }
        }
    }

    void iterate()
    {
        foreach (KeyValuePair<char, string> r in rules)
        {
            current = current.Replace(r.Key.ToString(), r.Value);
        }
    }
}
