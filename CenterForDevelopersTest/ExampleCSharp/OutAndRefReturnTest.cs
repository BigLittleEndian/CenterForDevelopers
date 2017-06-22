using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;

namespace CenterForDevelopers
{
    // C#7 - Out, Ref Local and Ref Return Are Bringing Some Cool Tricks
    // http://www.biglittleendian.com/article/103

    public class OutAndRefReturnTest
    {
        /************************************   

            Out example

         ************************************/

        public bool ReadSensors(out int s1, out int s2, out int s3)
        {
            s1 = 111;
            s2 = 222;
            s3 = 333;

            return true;
        }

        // Unit tests
        [Fact]
        public void ShortOutSyntaxTest()
        {
            // Declaration of "out" variables moved to the function call and therefore we can use "var"
            // _ is wildcard: we don't care for second parameter 
            if(ReadSensors(out var value1, out _, out int value3))
            {
                Assert.Equal(111, value1);
                Assert.Equal(333, value3);
            }
            else
            {
                Assert.True(false);
            }

            // Values still in scope:
            Assert.Equal(444, value1 + value3);
        }

        /************************************   

            Ref Return examples

         ************************************/

        public class Machine
        {
            public int Id { get; set; }
            public double[] Sensors = { 1.1, 2.2, 3.3 };

            public ref double SecondSensor()
            {
                return ref Sensors[1];
            }
        }

        // Unit Tests

        [Fact]
        public void RefReturnTest()
        {
            var machine = new Machine();

            Assert.Equal(2.2, machine.Sensors[1]);

            // Get a reference to a second sensor & directly change sensor value
            ref var secondSensor = ref machine.SecondSensor();
            secondSensor = 2.0;
            Assert.Equal(2.0, machine.Sensors[1]);

            // You can still get by value but change will be local
            var secondSensorVal = machine.SecondSensor();
            Assert.Equal(2.0, secondSensorVal);
            secondSensorVal = 1.9;
            Assert.Equal(2.0, machine.Sensors[1]); // Still 2.0
        }

        [Fact]
        public void MethodOnTheLeftTest()
        {
            var machine = new Machine();

            machine.SecondSensor() = 2.3;

            Assert.Equal(2.3, machine.Sensors[1]); 
        }

        [Fact]
        public void RefLocalTest()
        {
            var local = 555;

            ref var referenceToLocal = ref local;

            referenceToLocal = 444;

            Assert.Equal(444, local);
        }

        /************************************  
        
           Can and Can't "return ref"
        
        ************************************/

        // Can't return local variable
        /*
        public ref int CantReturnLocal()
        {
            int local = 555;
            return ref local; // ERROR
        }
        */

        // Can return local array since it is on a heap
        public ref int StillReturnLocal()
        {
            int[] localArray = { 555 };
            return ref localArray[0];
        }

        // Can't return immutable 
        /*
        public readonly string X = "immutable";
        public ref string CantReturnImmutable()
        {
            return ref X; // ERROR
        }
        */
    }
}

