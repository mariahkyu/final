using System;
using System.Collections.Generic;

class SimpleExpressionEvaluator
{
    static void Main()
    {
        Console.WriteLine("Enter a mathematical expression:");

        string expression = Console.ReadLine();  //يطلب من المستخدم إدخال تعبير رياضي

        try//تقييم التعبير وطباعة النتيجة
        {
            double result = EvaluateExpression(expression);
            Console.WriteLine($"Result: {result}");
        }
        catch (Exception ex)
        {
            //في حالة حدوث خطأ، يتم طباعة رسالة الخطأ
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
    // يقوم بتقييم التعبير الرياضي المعطى
    static double EvaluateExpression(string expression)
    {//يستخدم stack لتخزين الارقام
        Stack<double> operandStack = new Stack<double>();
        //يستخدم stack لتخزين العمليات الرياضية (+, -, *, /)
        Stack<char> operatorStack = new Stack<char>();
        // يمر عبر كل حرف في التعبير
        foreach (char token in expression)
        {
            if (char.IsDigit(token))
            {//إذا كان الحرف رقمًا، يتم إضافته إلى طوق الأرقام
                operandStack.Push(double.Parse(token.ToString()));
            }
            else if (IsOperator(token))
            {
                // إذا كان الحرف عملية رياضية، يتم معالجتها ووضعها في طوق العمليات
                while (operatorStack.Count > 0 && Precedence(operatorStack.Peek()) >= Precedence(token))
                {
                    ProcessOperation(operandStack, operatorStack);
                }
                operatorStack.Push(token);
            }
            else if (token == '(')
            {
                // إذا كان الحرف فتح قوس، يتم وضعه في طوق العمليات
                operatorStack.Push(token);
            }
            else if (token == ')')
            {
                //إذا كان الحرف إغلاق قوس، يتم معالجة العمليات حتى يتم العثور على فتح القوس المناسب
                while (operatorStack.Count > 0 && operatorStack.Peek() != '(')
                {
                    ProcessOperation(operandStack, operatorStack);
                }
                //التحقق من وجود فتح القوس 
                if (operatorStack.Count == 0)
                {
                    throw new Exception("Mismatched parentheses");
                }
                //يتم ازالة فتح القوس
                operatorStack.Pop(); // Pop '('
            }
            else if (!char.IsWhiteSpace(token))
            {
                //اذا لم يكن عملية ولا قوس ولا رقم يظهر استثناء 
                throw new Exception($"Invalid character: {token}");
            }
        }

        while (operatorStack.Count > 0)
        {
            ProcessOperation(operandStack, operatorStack);
        }
        //يتم التحقق من وجود قيمة واحدة ويرجعها
        if (operandStack.Count != 1 || operatorStack.Count > 0)
        {
            throw new Exception("Invalid expression");
        }

        return operandStack.Pop();
    }
    //يتحقق من ان الحرف عملية رياضية

    static bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/';
    }
    //يحدد الاولويه في العمليات
    static int Precedence(char op)
    {
        switch (op)
        {
            case '+':
            case '-':
                return 1;
            case '*':
            case '/':
                return 2;
            default:
                return 0;
        }
    }
    // يعالج العملية الرياضية ويطبقها على الارقام 
    static void ProcessOperation(Stack<double> operandStack, Stack<char> operatorStack)
    {
        char op = operatorStack.Pop();

        if (operandStack.Count < 2)
        {
            throw new Exception("Not enough operands for the operator");
        }

        double operand2 = operandStack.Pop();
        double operand1 = operandStack.Pop();
         //ينفذ العملية ويضعها في طوق الارقام
        switch (op)
        {
            case '+':
                operandStack.Push(operand1 + operand2);
                break;
            case '-':
                operandStack.Push(operand1 - operand2);
                break;
            case '*':
                operandStack.Push(operand1 * operand2);
                break;
            case '/':
                operandStack.Push(operand1 / operand2);
                break;
        }
    }
}
