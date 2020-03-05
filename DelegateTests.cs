using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace EventsAndDelegates
{
    public class DelegateTests
    {
        //Code can be found at https://github.com/theparticleman/EventsAndDelegates

        //Conceptually, delegates are similar to callbacks or function pointers.
        //A delegate is a managed way to reference a piece of code, rather than a piece of data.
        //Programming languages that can treat variables that reference code just like any other kind of variable are said to have first-class functions.
        //A piece of code that receives as input or returns as output another piece of code is called a higher order function.

        //This is a type, like a class or interface.
        private delegate int TestDelegate(int input);

        [Test]
        public void A_delegate_is_a_type()
        {
            //The compiler can't figure out the type automatically with var.
            //var myDelegateInstance = MyLocalFunction;

            //This is an instance of a type like an object.
            TestDelegate myDelegateInstance = MyLocalFunction;

            Assert.That(myDelegateInstance, Is.Not.Null);
            Assert.That(myDelegateInstance, Is.AssignableTo<TestDelegate>());

            int MyLocalFunction(int input)
            {
                return input * input;
            }
        }

        [Test]
        public void A_delegate_can_be_invoked()
        {
            TestDelegate myDelegateInstance = MyLocalFunction;

            var result = myDelegateInstance(42);
            Assert.That(result, Is.GreaterThan(0));

            //You can also call it like this...if you want to.
            result = myDelegateInstance.Invoke(42);
            Assert.That(result, Is.GreaterThan(0));

            int MyLocalFunction(int someInt) => someInt + 1;
        }

        [Test]
        public void A_delegate_only_references_a_single_instance_at_a_time()
        {
            TestDelegate myDelegateInstance = ZeroFunction;

            Assert.That(myDelegateInstance(42), Is.EqualTo(0));

            myDelegateInstance = IncrementFunction;

            Assert.That(myDelegateInstance(42), Is.GreaterThan(42));

            int ZeroFunction(int someInt) => 0;
            int IncrementFunction(int someInt) => someInt + 1;
        }

        [Test]
        public void Special_types_of_delegates()
        {
            //Actions and Funcs and Predicates. Oh my!

            Action<int> actionDelegate = ActionFunction;
            Assert.That(actionDelegate, Is.Not.Null);

            Func<int, int> funcDelegate = FuncFunction;
            Assert.That(funcDelegate(42), Is.GreaterThan(0));

            Predicate<int> predicateDelegate = PredicateFunction;
            Assert.That(predicateDelegate(42), Is.True);

            void ActionFunction(int someInt) => Console.WriteLine(someInt);
            int FuncFunction(int someInt) => someInt * someInt;
            bool PredicateFunction(int someInt) => someInt == 42;
        }

        [Test]
        public void Different_types_of_delegate_instances()
        {
            var instance = new TestClass();
            TestDelegate instanceMethodDelegate = instance.InstanceMethod;

            //This won't work because we can't do anything with a reference to an instance method without an instance of the class.
            //TestDelegate instanceMethodDelegate = TestClass.InstanceMethod;

            Assert.That(instanceMethodDelegate, Is.Not.Null);

            TestDelegate staticMethodDelegate = TestClass.StaticMethod;
            Assert.That(staticMethodDelegate, Is.Not.Null);

            TestDelegate anonymousMethodDelegate = delegate (int x)
            {
                return x + 1;
            };
            Assert.That(anonymousMethodDelegate, Is.Not.Null);

            TestDelegate lambdaDelegate = x => x + 42;
            Assert.That(lambdaDelegate, Is.Not.Null);
        }

        [Test]
        public void Linq_is_built_on_delegates()
        {
            var listOfWords = new List<string>{
                "This",
                "is",
                "a",
                "sample",
                "list",
                "of",
                "words"
            };

            //We usually see LINQ methods with lambda delegates.
            var shortest = listOfWords.OrderBy(x => x.Length).First();
            Assert.That(shortest.Length, Is.EqualTo(1));


            Func<string, int> orderByDelegate = delegate (string input)
            {
                return input.Length;
            };

            //But you can use any matching delegate instance.
            shortest = listOfWords.OrderBy(orderByDelegate).First();
            Assert.That(shortest.Length, Is.EqualTo(1));
        }

        [Test]
        public void Delegate_instances_are_multicast_delegates()
        {
            var startValue = 42;
            Action delegateInstance;
            Action inc = Increment;
            Action dec = Decrement;

            delegateInstance = inc + dec;
            inc();
            Assert.That(startValue, Is.EqualTo(43));
            dec();
            
            delegateInstance();
            Assert.That(startValue, Is.EqualTo(42));

            delegateInstance += inc;
            delegateInstance();
            Assert.That(startValue, Is.EqualTo(43));

            void Increment() => startValue++;
            void Decrement() => startValue--;
        }

        class TestClass
        {
            public int InstanceMethod(int input)
            {
                return input * 2;
            }

            public static int StaticMethod(int input)
            {
                return input * input;
            }
        }


    }
}