using System;
using System.Linq;
using System.Collections.Generic;

using Xunit;

namespace CenterForDevelopers
{
    // C#7 - New ValueTuple Will Help You Write Less Code
    // http://www.biglittleendian.com/article/101
    public static class ValueTupleTestExtensions
    {
        // If you already have Deconstruct (with same signature) defined in Hero class this extension method will not be used!
        public static void Deconstruct(this ValueTupleTest.Hero h, out string name, out int version)
        {
            name = h.Name + "-EXT";
            version = h.Version;
        }
    }
    public class ValueTupleTest
    {
        [Fact]
        public void PainBeforeTuples()
        {
            var numTxt = "555";
            int num;
            bool res = int.TryParse(numTxt, out num);
            if (res)
            {
                Assert.Equal(555, num);
            }
            else
            {
                Assert.True(false, "Make sure if-true executed");
            }
        }

        public class Hero
        {
            public Hero(string name, int version)
            {
                Name = name;
                Version = version;
            }

            public string Name { get; }
            public int Version { get; }

            public void Deconstruct(out string name, out int version)
            {
                name = Name;
                version = Version;
            }

            public (int version, int nextVersion) GetVersions()
            {
                return (Version, Version + 1);
            }

            // Error. Changing names in tuple is not creating new type ! Same function is already defined. 
            /*
            public (int v, int nextV) GetVersions()
            {
                return (Version, Version + 1);
            }
            */
        }
    
        [Fact]
        public void CreatingValueTupleTest()
        {
            // "Unnamed tuples" - Similar to old System.Tuple
            var hero_1 = ("Big Hero", 6);
            Assert.Equal("Big Hero", hero_1.Item1);
            Assert.Equal(6, hero_1.Item2);

            // "Named Tuple" - Using semantic names for tuple members
            (string name, int version) hero_2 = ("Big Hero", 6);
            Assert.Equal("Big Hero", hero_2.name);
            Assert.Equal(6, hero_2.version);
            // Named tuple still have Item1, Item2 ... fields.
            Assert.Equal("Big Hero", hero_2.Item1);
            Assert.Equal(6, hero_2.Item2);

            // You can specify names on the right side
            var hero_3 = (name: "Big Hero", version: 6);
            Assert.Equal("Big Hero", hero_3.name);
            Assert.Equal(6, hero_3.version);

            // Specifying names on both sides will generate warning: right side names are ignored !
            (string name, int version) hero_4 = (who: "Big Hero", number: 6);
            Assert.Equal("Big Hero", hero_4.name);
            Assert.Equal(6, hero_4.version);

            // Fields are public and mutable
            hero_3.version++;
            Assert.Equal(7, hero_3.version);
        }

        [Fact]
        public void AssignmentTest()
        {
            var hero_unnamed = ("Big Hero", 6);
            var hero_named = (name: "Big Hero - NEW", version: 7);

            // As long as number of fields is same and field types can be converted you can assign
            hero_unnamed = hero_named;
            
            Assert.Equal("Big Hero - NEW", hero_unnamed.Item1); // Names are not assigned !
            Assert.Equal(7, hero_unnamed.Item2);

            // Implicit conversion
            (string, double) hero_converted = hero_named;
            Assert.Equal(7, hero_converted.Item2);
            Assert.True(hero_converted.Item2.GetType() == typeof(double));
        }

        // Next method is returning named tuple but we can just return (bool, int) as unnamed tuple
        (bool success, int value) Divide(int a, int b)
        {
            var bIsNotZero = b != 0;
            return (success: bIsNotZero, value: (bIsNotZero) ? a/b : 0);
        }

        [Fact]
        public void ValueTupleAsFunctionResultTest() 
        {
            // Using real names instead of Item1, Item2
            var result = Divide(5, 2);
            Assert.True(result.success);
            Assert.Equal(2, result.value);

            // Deconstruction into two variables
            (var s, var v) = Divide(12, 3);
            Assert.True(s);
            Assert.Equal(4, v);
        }

        [Fact]
        public void DeconstructionTest()
        {
            var hero = new Hero("Big Hero", 6);

            // Explicitly declare field types
            (string name, int version) = hero;

            Assert.Equal("Big Hero", name);
            Assert.Equal(6, version);

            // Implicitly declare field types by using var
            var (name2, version2) = hero;
            Assert.True(name2.GetType() == typeof(string));
            Assert.True(version2.GetType() == typeof(int));

            // You can even mix explicit and implicit types
            (string name3, var version3) = hero;
            Assert.True(version3.GetType() == typeof(int));

            // Deconstruct to existing variables
            (name2, version3) = hero;

            // Using "discards" - "I don't care for the rest"
            (var name4, _ ) = hero;
            Assert.Equal("Big Hero", name4);
        }

        public IEnumerable<(string name, int version)> GetHeroes()
        {
            var heroes = new List<Hero>()
            {
                new Hero("Big Hero", 6),
                new Hero("Small Hero", 5)
            };

            // LINQ
            return heroes.Select(x => (x.Name, x.Version));
        }

        [Fact]
        public void LinqTest()
        {
            var heroes = GetHeroes().ToList();

            // Names are preserved !
            Assert.Equal("Big Hero", heroes[0].name);
            Assert.Equal(6, heroes[0].version);
            Assert.Equal("Small Hero", heroes[1].name);
            Assert.Equal(5, heroes[1].version);
        }

        [Fact]
        public void RestTest()
        {
            var numbers = (1, 2, 3, 4, 5, 6, 7, 8, 9);
            Assert.Equal(7, numbers.Item7);
            // Rest
            Assert.Equal(8, numbers.Item8); // You still can call Item8 but will be converted to Rest.Item1
            Assert.Equal(8, numbers.Rest.Item1);
            Assert.Equal(9, numbers.Item9);
            Assert.Equal(9, numbers.Rest.Item2);
        }

        [Fact]
        public void NullableTest()
        {
            (int, int)? numbers = null;
            Assert.False(numbers.HasValue);

            numbers = (1, 2);
            Assert.True(numbers.HasValue);
        }
    }
}

