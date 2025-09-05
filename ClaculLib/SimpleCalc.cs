using System.Globalization;

public class UnknownOperatorException : Exception { }
public class DivideByZeroCustomException : Exception { }
public class UnbalancedParenthesesException : Exception { }
public class InvalidExpressionException : Exception { }

public static class SimpleCalc
{
    // Главный метод
    public static string EvaluateOrError(string expr)
    {
        try
        {
            double value = Evaluate(expr);
            return value.ToString(CultureInfo.InvariantCulture);
        }
        catch (UnknownOperatorException)
        {
            return "ошибка: неизвестный оператор";
        }
        catch (DivideByZeroCustomException)
        {
            return "ошибка: деление на ноль";
        }
        catch (UnbalancedParenthesesException)
        {
            return "ошибка: несбалансированные скобки";
        }
        catch (InvalidExpressionException)
        {
            return "ошибка: неверное выражение";
        }
        catch
        {
            return "ошибка: неизвестная ошибка";
        }
    }

    private static double Evaluate(string expr)
    {
        if (expr == null) throw new InvalidExpressionException();

        var values = new Stack<double>();
        var ops = new Stack<char>();

        for (int i = 0; i < expr.Length; i++)
        {
            char c = expr[i];

            if (char.IsWhiteSpace(c)) continue;

        
            if (char.IsDigit(c) || (c == '.' && i + 1 < expr.Length && char.IsDigit(expr[i + 1])))
            {
                int start = i;
                i++;
                while (i < expr.Length && (char.IsDigit(expr[i]) || expr[i] == '.')) i++;
                string num = expr.Substring(start, i - start);
                i--;
                values.Push(double.Parse(num, CultureInfo.InvariantCulture));
                continue;
            }

            if (c == '(') { ops.Push(c); continue; }

            if (c == ')')
            {
                while (ops.Count > 0 && ops.Peek() != '(')
                    ApplyTop(values, ops.Pop());
                if (ops.Count == 0 || ops.Pop() != '(')
                    throw new UnbalancedParenthesesException();
                continue;
            }
 
            if (c == '+' || c == '-' || c == '*' || c == '/')
            {
                while (ops.Count > 0 && ops.Peek() != '(' &&
                       Priority(ops.Peek()) >= Priority(c))
                    ApplyTop(values, ops.Pop());
                ops.Push(c);
                continue;
            }

            throw new UnknownOperatorException();
        }

        while (ops.Count > 0)
        {
            if (ops.Peek() == '(') throw new UnbalancedParenthesesException();
            ApplyTop(values, ops.Pop());
        }

        if (values.Count != 1) throw new InvalidExpressionException();
        return values.Pop();
    }

    private static void ApplyTop(Stack<double> values, char op)
    {
        if (values.Count < 2) throw new InvalidExpressionException();

        double b = values.Pop();
        double a = values.Pop();

        switch (op)
        {
            case '+': values.Push(a + b); break;
            case '-': values.Push(a - b); break;
            case '*': values.Push(a * b); break;
            case '/':
                if (b == 0) throw new DivideByZeroCustomException();
                values.Push(a / b);
                break;
            default:
                throw new UnknownOperatorException();
        }
    }

    private static int Priority(char op) => op switch
    {
        '+' or '-' => 1,
        '*' or '/' => 2,
        _ => 0
    };
}