using System;
using NUnit.Framework;

namespace EventsAndDelegates
{
    public class DelegateTests
    {
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

            TestDelegate anonymousMethodDelegate = delegate(int x)
            {
                return x + 1;
            };
            Assert.That(anonymousMethodDelegate, Is.Not.Null);

            TestDelegate lambdaDelegate = x => x + 42;
            Assert.That(lambdaDelegate, Is.Not.Null);
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