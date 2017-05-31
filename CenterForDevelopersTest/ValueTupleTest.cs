using System;
using Xunit;

namespace CenterForDevelopers
{
    public class ValueTupleTest
    {
        #region C# 7 - ValueTuple
        (bool success, int value) GetValueTuple(int n)
        {
            return (success: n == 5, value: n+1);
        }

        [Fact]
        public void ToupleTest() 
        {
            var result = GetValueTuple(5);
            Assert.True(result.success);
            Assert.Equal(6, result.value);

            result.value++;
            Assert.Equal(7, result.value);

            // Deconstruction
            (var success, var value) = GetValueTuple(10);
            Assert.False(success);
            Assert.Equal(11, value);
        }
        #endregion
    }
}
