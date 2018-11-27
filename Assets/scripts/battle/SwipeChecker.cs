using UnityEngine;

namespace battle
{
    public class SwipeChecker
    {
        
        private static SwipeChecker _instance;

        public static SwipeChecker Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SwipeChecker();
                }
                return _instance;
            }
        }

        public bool CheckStartPosition(Transform partnerTransform, Vector2 vector2)
        {
            var partnerName = partnerTransform.name;
            var x = vector2.x;
            var y = vector2.y;
            const float iconHeight = 1.4f;
            const float yusukeIconTop = -1.2f;
            const float yusukeIconBottom = -2.5f;
            const float masakiIconTop = yusukeIconTop - iconHeight;
            const float masakiIconBottom = yusukeIconBottom - iconHeight;
            const float akoIconTop = yusukeIconTop - iconHeight * 2;
            const float akoIconBottom = yusukeIconBottom - iconHeight * 2;
            const float partnersIconRight = -1.4f;
            const float partnersIconLeft = -2.3f;
            if (partnerName == "yusuke")
            {
                if (x < partnersIconLeft || partnersIconRight < x)
                {
                    return false;
                }

                if (y < yusukeIconBottom || yusukeIconTop < y)
                {
                    return false;
                }
            }
            else if (partnerName == "masaki")
            {
                if (x < partnersIconLeft || partnersIconRight < x)
                {
                    return false;
                }

                if (y < masakiIconBottom || masakiIconTop < y)
                {
                    return false;
                }
            }
            else if (partnerName == "ako")
            {
                if (x < partnersIconLeft || partnersIconRight < x)
                {
                    return false;
                }

                if (y < akoIconBottom || akoIconTop < y)
                {
                    return false;
                }
            }

            return true;
        }

        public bool CheckEndPosition(Vector2 touchStartPos, Vector2 touchEndPos)
        {
            return 100 < touchEndPos.y - touchStartPos.y &&
                   Mathf.Abs(touchEndPos.x - touchStartPos.x) < 50;
            
        }
            
    }
}
