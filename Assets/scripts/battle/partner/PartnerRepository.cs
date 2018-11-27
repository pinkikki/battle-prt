using System.Collections.Generic;
using db.dao;
using script.db.entity;

namespace battle.partner
{
    public class PartnerRepository
    {
        private static PartnerRepository _instance;

        public static readonly string YUSUKE_GAME_OBJECT_KEY = "yusuke";
        public static readonly string MASAKI_GAME_OBJECT_KEY = "masaki";
        public static readonly string AKO_GAME_OBJECT_KEY = "ako";

        public Dictionary<string, PartnerBattleObject> Partners { get; } =
            new Dictionary<string, PartnerBattleObject>();

        private PartnerRepository()
        {
        }

        public static PartnerRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PartnerRepository();
                    var partners = PartnerDao.Instance.SelectAll();
                    partners.ForEach(partner =>
                    {
                        _instance.Partners.Add(partner.SystemName, CreatePartnerBattleObject(partner));
                    });
                }

                return _instance;
            }
        }

        public static void Destroy()
        {
            _instance = null;
        }

        private static PartnerBattleObject CreatePartnerBattleObject(PartnerEntity partner)
        {
            return new PartnerBattleObject
            {
                Id = partner.Id,
                SystemName = partner.SystemName,
                DisplayName = partner.DisplayName,
                Mind = TeamDao.Instance.SelectByPrimaryKey(1).Level * 10,
                CurrentHp = 1,
                Skill1 = SkillDao.Instance.SelectByPrimaryKey(partner.Skill1),
                CharacterType = CharacterType.Partner
            };
        }
    }
}
