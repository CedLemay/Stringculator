using Calculatrice_Texte;
using System;
using Xunit;


namespace Calculatrice_Texte.Test
{
    public class DecimalTest
    {
        public string[] binaryTestArray = { "0", "9", "18", "27", "36", "45", "54", "63", "72", "81" };


        [Fact]
        public void TestValidateTrue()
        {
            Assert.True(Calculator.Validate("123,456"));
        }


        [Theory]
        [InlineData("123,")] 
        [InlineData("123,456,789")]  
        public void TestValidateFalse(string value)
        {
            Assert.False(Calculator.Validate(value));
        }


        [Theory]
        [InlineData("35,42", "3,725", new string[] { "35,420", "03,725" })]  // Inside
        [InlineData("0,72753", "42", new string[] { "00,72753", "42,00000" })]  // Equal
        //[InlineData("-1", 0)]      // Under
        public void Test_AddZeroes(string n1, string n2, string[] expected)
        {
            Calculator.AdaptNumbers(ref n1, ref n2);
            Assert.Equal(expected, new string[] { n1, n2 });
        }

        

        [Theory]
        [InlineData("352,5", "6,78966", "359,28966")]
        [InlineData("5,45", "0,75", "6,2")]
        public void Test_AddNumbers(string n1, string n2, string expected)
        {
            string result = Calculator.AddNumbers(n1, n2);
            Assert.Equal(expected, result);
        }



        [Theory]
        [InlineData("50,3", "6,3", "44")]
        [InlineData("40,1", "27,5", "12,6")]
        [InlineData("10,1", "2,5", "7,6")]

        public void Test_SubtractNumbers(string n1, string n2, string expected)
        {
            string result = Calculator.SubstractNumbers(n1, n2);
            Assert.Equal(expected, result);
        }



        [Theory]
        [InlineData("2,5", "3", "7,5")]
        [InlineData("1,025", "4", "4,1")]
        [InlineData("0,1234", "56,78", "7,006652")]
        public void Test_MultiplyNumbers(string n1, string n2, string expected)
        {
            string result = Calculator.MultiplyNumbers(n1, n2);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("8,1", "9", "0,9")]
        [InlineData("81", "30", "2,7")]
        [InlineData("0,5", "0,05", "10")]
        public void Test_DivideNumbers(string n1, string n2, string expected)
        {
            string result = Calculator.DivideNumbers(n1, n2);
            Assert.Equal(expected, result);
        }


        //[Theory]
        //[InlineData("38", 4)]  // Inside
        //[InlineData("54", 6)]  // Equal
        //[InlineData("100", 9)]  // Over
        ////[InlineData("-1", 0)]      // Under
        //public void Test_BinarySearch(string n, int expected)
        //{
        //    int result = Calculator.BinaryGet(n, binaryTestArray);
        //    Assert.Equal(expected, result);
        //}





    }
}
