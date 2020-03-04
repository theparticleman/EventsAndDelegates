using System;
using NUnit.Framework;

namespace EventsAndDelegates
{
    public class EventTests
    {
        //Events must be of a delegate type.
        private event Action MyTestEvent;

        [Test]
        public void Events_can_reference_multiple_delegate_instances()
        {
            //Events are basically a language level implementation of the observer pattern.

            var myValue = 1;

            MyTestEvent += IncrementMyValue;
            MyTestEvent += IncrementMyValue;
            MyTestEvent += DecrementMyValue;

            MyTestEvent();

            Assert.That(myValue, Is.EqualTo(2));

            //If you don't unsubscribe from an event it can lead to memory leaks
            MyTestEvent -= IncrementMyValue;
            MyTestEvent -= DecrementMyValue;

            Assert.That(MyTestEvent, Is.Not.Null);
            
            MyTestEvent -= IncrementMyValue;

            Assert.That(MyTestEvent, Is.Null);

            void IncrementMyValue() => myValue++;
            void DecrementMyValue() => myValue--;
        }

        [Test]
        public void Events_have_special_scoping_rules()
        {
            var myValue = 1;
            var obj = new ClassWithEvent();
            obj.MyEvent += () => myValue++;

            //The event can only be published inside the scope where it is owned
            // obj.MyEvent();

            obj.PublishMyEvent();

            Assert.That(myValue, Is.EqualTo(2));
        }

        [Test]
        public void Invoking_an_event_when_there_are_no_subscribers_results_in_an_error()
        {
            Assert.Throws<NullReferenceException>(() => MyTestEvent.Invoke());

            Assert.DoesNotThrow(() => MyTestEvent?.Invoke());

            MyTestEvent += DummyAction;
            MyTestEvent.Invoke();

            MyTestEvent -= DummyAction;

            void DummyAction() { };
        }

        [Test]
        public void It_is_possible_to_create_an_event_with_custom_add_and_remove_accessors()
        {
            //If you find yourself needing to do this, please stop and re-examine your life choices.
            Assert.Pass();
        }
    }

    class ClassWithEvent
    {
        public event Action MyEvent;
        public Action MyDelegateInstance;

        public void PublishMyEvent()
        {
            MyEvent();
        }
    }
}