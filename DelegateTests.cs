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


    }
}