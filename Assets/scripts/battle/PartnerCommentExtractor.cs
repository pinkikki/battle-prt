namespace battle
{
    public class PartnerCommentExtractor
    {
        private static PartnerCommentExtractor _instance;

        public static PartnerCommentExtractor Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PartnerCommentExtractor();
                }
                return _instance;
            }
        }
        
        public string ExtractWithMiddle(string partnerName)
        {
            switch (partnerName)
            {
                case "yusuke":
                    return "ふー";
                case "masaki":
                    return "むむむ";
                case "ako":
                    return "きゃー";
                default:
                    return "";
            }
        }
        
        public string ExtractWithMax(string partnerName)
        {
            switch (partnerName)
            {
                case "yusuke":
                    return "行けー！";
                case "masaki":
                    return "がんばれー！";
                case "ako":
                    return "大丈夫？";
                default:
                    return "";
            }
        }
    }
}
