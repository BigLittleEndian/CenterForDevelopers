using System;
using System.Collections.Generic;

using Xunit;

namespace CenterForDevelopers
{
    // C#7 - Pattern Matching Is One More Step Forward in Making C# More Concise and Readable
    // http://www.biglittleendian.com/article/104

    public class PatternMatchingTest
    {
        [Fact]
        public void IsPatternTest()
        {
            var valuesToSum = new List<object> { 1, 2, new List<object> { 3, 4 } };

            int LocalSumFunction(IEnumerable<object> numbers)
            {
                var sum = 0;

                foreach (var item in numbers)
                {
                    if (item is int value)
                        sum += value;
                    else if (item is IEnumerable<object> list)
                        sum += LocalSumFunction(list);
                }

                return sum;
            }

            var totalSum = LocalSumFunction(valuesToSum);

            Assert.Equal(1 + 2 + 3 + 4, totalSum);
        }

        [Fact]
        public void SwitchPatternTest()
        {   
            var valuesToSum = new List<object> {1, 2, null, new List<object> {3, 4}};

            int LocalSumFunction(IEnumerable<object> numbers)
            {
                var sum = 0;
                
                foreach(var item in numbers)
                {
                    switch(item)
                    {
                        case 0:
                            break;
                        case int value:
                            sum += value;
                            break;
                        case IEnumerable<object> list:
                            sum += LocalSumFunction(list);
                            break;
                        case null:
                            break;
                        default: // Always evaluated last so put it at the end of switch to avoid confusion
                            throw new InvalidOperationException("Can't work with that!");
                    }
                }

                return sum;
            }

            var totalSum = LocalSumFunction(valuesToSum);

            Assert.Equal(1 + 2 + 3 + 4, totalSum);
        }

        struct Person
        {
            public string Name;
            public int Age;
        }

        [Fact]
        public void WhenPatternTest()
        {   
            IEnumerable<object> nullList = null;

            var valuesToSum = new List<object> { 1, 
                    new Person { Name = "Daniel", Age = 22 }, new Person { Name = "Sofia", Age = 17 },
                    null, nullList,
                    new List<object> {3, 4}};

            int LocalSumFunction(IEnumerable<object> numbers)
            {
                var sum = 0;
                
                foreach(var item in numbers)
                {
                    switch(item)
                    {
                        case 0:                        // Must be before case int value
                        case Person p when p.Age < 18: // Must be before "case Person p"
                            // Don't use p here ! It can be unassigned in case of item == 0 
                            break;
                        case int value:
                            sum += value;
                            break;
                        case IEnumerable<object> list:   // nullList will not match this
                            sum += LocalSumFunction(list);
                            break;
                        case Person p:
                            sum += p.Age;
                            break;
                        case null:
                            break;
                        default: // Always evaluated last so put it at the end of switch to avoid confusion
                            throw new InvalidOperationException("Can't work with that!");
                    }
                }

                return sum;
            }

            var totalSum = LocalSumFunction(valuesToSum);

            Assert.Equal(1 + 22 + 3 + 4, totalSum);
        }

        public class Number
        {
            public int Value { get; }
            public string Text { get; }

            public Number(int value, string text)
            {
                Value = value;
                Text = text;
            }
        }

        [Fact]
        public void VarPatternTest()
        {   
            Number number1 = new Number(1, "1");
            Number number2 = null;

            var valuesToSum = new List<object> { number1, number2, 3 };

            int LocalSumFunction(IEnumerable<object> numbers)
            {
                var sum = 0;
                
                foreach(var item in numbers)
                {
                    switch(item)
                    {
                        case var intOrNumber:
                            if(intOrNumber is int intValue)
                            {
                                sum += intValue;
                            }
                            else if(intOrNumber is Number number)
                            {
                                sum += number.Value;
                            }
                            break;
                        //    Uncommenting next two cases would generate error:
                        //    "The switch case has already been handled by a previous case."
                        //    var is already handling both int and null case !
                        // case int value:
                        // case null:
                        default:
                            throw new InvalidOperationException("var didn't match!");
                    }
                }

                return sum;
            }

            var totalSum = LocalSumFunction(valuesToSum);

            Assert.Equal(1 + 3, totalSum);
        }

        [Fact]
        public void InconsistentScopeTest()
        {
            object variable = "1";

            if(variable is string text)
            {
                Assert.Equal("1", text);
            }
            else if (variable is int number)
            {
                Assert.Equal(1, number);
            }

            // Uncommenting below will genereate error:
            // "Use of unassigned local variable 'text'"
            //Assert.Equal("1", text);

            // Uncommenting below will generate error:
            // "The name 'number' does not exist in the current context"
            //Assert.Equal("1", number);
        }

    }
}

