using Calculatrice_Texte;
using System;
using Xunit;

namespace Calculatrice_Texte.Test
{
    public class IntegerTest
    {
        public string[] binaryTestArray = { "0", "9", "18", "27", "36", "45", "54", "63", "72", "81" };

        [Fact]
        public void TestValidateTrue()
        {
            Assert.True(Calculator.Validate("123"));
        }

        [Fact]
        public void TestValidateFalse()
        {
            Assert.False(Calculator.Validate("a23"));
        }

        [Theory]
        [InlineData("38", 4)]  // Inside
        [InlineData("54", 6)]  // Equal
        [InlineData("100", 9)]  // Over
        //[InlineData("-1", 0)]      // Under
        public void Test_BinarySearch(string n, int expected)
        {            
            int result = Calculator.BinaryGet(n, binaryTestArray);
            Assert.Equal(expected, result);
        }



        [Theory]
        [InlineData("352", "876", "1228")]
        [InlineData("5", "2", "7")]
        public void Test_AddNumbers(string n1, string n2, string expected)
        {
            string result = Calculator.AddNumbers(n1, n2);
            Assert.Equal(expected, result);
        }



        [Theory]
        [InlineData("551", "72", "479")]
        [InlineData("2", "5", "-3")]
        [InlineData("21", "50", "-29")]
        [InlineData("50", "50", "0")]
        public void Test_SubtractNumbers(string n1, string n2, string expected)
        {
            string result = Calculator.SubstractNumbers(n1, n2);
            Assert.Equal(expected, result);
        }



        [Theory]
        [InlineData("2", "3", "6")]
        [InlineData("12", "12", "144")]
        [InlineData("625", "42", "26250")]
        public void Test_MultiplyNumbers(string n1, string n2, string expected)
        {
            string result = Calculator.MultiplyNumbers(n1, n2);
            Assert.Equal(expected, result);
        }


        [Theory]
        [InlineData("81", "9", "9")]
        [InlineData("81", "3", "27")]
        [InlineData("625", "625", "1")]
        public void Test_DivideNumbers(string n1, string n2, string expected)
        {
            string result = Calculator.DivideNumbers(n1, n2);
            Assert.Equal(expected, result);
        }

    }
}
