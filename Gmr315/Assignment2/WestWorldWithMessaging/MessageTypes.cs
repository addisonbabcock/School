namespace WestWorldWithMessaging
{
    public enum message_type
    {
        Msg_HiHoneyImHome,
        Msg_StewReady,
    }

    public class MessageTypes
    {
        public static string MsgToStr(int msg)
        {
            switch (msg)
            {
                case 1:

                    return "HiHoneyImHome";

                case 2:

                    return "StewReady";

                default:

                    return "Not recognized!";
            }
        }
    }
}