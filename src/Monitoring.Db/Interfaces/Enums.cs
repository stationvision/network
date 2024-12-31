namespace Monitoring.Db.Interfaces
{
    public class Enums
    {
        //This is used to define the status of the message to track the processing of the message
        public enum MessageStatus
        {
            Pending,
            Processed,
            Error
        }


        //This one is shows device status
        public enum PulsStatus
        {
            Stop,
            Start,
            None
        }
    }
}
