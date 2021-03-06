using NUnit.Framework;
using System;

namespace EventSamples
{
    class Sender
    {
        private int myInt;
        public int MyInt
        {
            get
            {
                return myInt;
            }
            set
            {
                myInt = value;
                //Whenever we set a new value, the event will fire.
                OnMyIntChanged();
            }
        }

        //EventHandler is a predefined delegate which is used to handle simple events.
        //It has the following signature:
        //delegate void System.EventHandler(object sender, System.EventArgs e)
        //where the sender tells who is sending the event and
        //EventArgs is used to store information about the event.
        public event EventHandler MyIntChanged;
        protected virtual void OnMyIntChanged()
        {
            if (MyIntChanged != null)
            {
                MyIntChanged(this, EventArgs.Empty);
            }
        }

        public void GetNotificationItself(Object sender, System.EventArgs e)
        {
            Console.WriteLine("Sender himself send a notification: I have changed myInt value  to {0} ", myInt);
        }
    }

    class Receiver
    {
        public void GetNotificationFromSender(Object sender, System.EventArgs e)
        {
            Console.WriteLine("Receiver receives a notification: Sender recently has changed the myInt value . ");
        }
    }

    [TestFixture]
    public class EventsTests
    {
        [Test]
        public void TestEvent1()
        {
            Sender sender = new Sender();
            Receiver receiver = new Receiver();
            //Receiver is registering for a notification from sender
            sender.MyIntChanged += receiver.GetNotificationFromSender;
            //sender.MyIntChanged += new EventHandler(receiver.GetNotificationFromSender);

            sender.MyInt = 1;
            sender.MyInt = 2;

            //Unregistering now
            sender.MyIntChanged -= receiver.GetNotificationFromSender;

            //No notification sent for the receiver now.
            sender.MyInt = 3;

            //Sender will receive its own notification now onwards
            sender.MyIntChanged += sender.GetNotificationItself;
            sender.MyInt = 4;
        }
    }
}
