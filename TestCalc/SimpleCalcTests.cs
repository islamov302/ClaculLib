using Xunit;

public class SimpleCalcTests
{
    [Theory]
    [InlineData("1 + 2 * 3", "7")]
    [InlineData("(2 + 3) * 4", "20")]
    [InlineData("10 / (5 - 3)", "5")]
    [InlineData("3 + 4 * 2 / (1 - 5)", "1")]
    public void Evaluate_ReturnsCorrect(string expr, string expected)
    {
        var result = SimpleCalc.EvaluateOrError(expr);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DivideByZero_ReturnsError()
    {
        var result = SimpleCalc.EvaluateOrError("10 / 0");
        Assert.Equal("ошибка: деление на ноль", result);
    }

    [Fact]
    public void UnknownOperator_ReturnsError()
    {
        var result = SimpleCalc.EvaluateOrError("2 & 3");
        Assert.Equal("ошибка: неизвестный оператор", result);
    }

    [Fact]
    public void UnbalancedParentheses_ReturnsError()
    {
        var result = SimpleCalc.EvaluateOrError("(2 + 3");
        Assert.Equal("ошибка: несбалансированные скобки", result);
    }

    [Fact]
    public void InvalidExpression_ReturnsError()
    {
        var result = SimpleCalc.EvaluateOrError("2 + * 3");
        Assert.Equal("ошибка: неверное выражение", result);
    }
}