using System.Collections.Generic;
using System.Linq;

namespace battle.action
{
    public class ActionPipeline
    {
        private readonly Queue<Action> _normalActions = new Queue<Action>();
        private readonly Queue<Action> _skillActions = new Queue<Action>();
        private Action _currentAction;

        public void PutForNormal(Action action)
        {
            _normalActions.Enqueue(action);
        }

        public void PutForSkill(Action action)
        {
            _skillActions.Enqueue(action);
        }

        public Action Next()
        {
            _currentAction = _skillActions.Any() ? DequeueFromSkillActions() : DequeueFromNormalActions();
            return Get();
        }

        public Action Get()
        {
            return _currentAction;
        }

        public bool Exist()
        {
            return _skillActions.Any() || _normalActions.Any();
        }

        private Action DequeueFromSkillActions()
        {
            var action = _skillActions.Dequeue();
            return action.BattleObject.CurrentHp == 0 ? Next() : action;
        }
        
        private Action DequeueFromNormalActions()
        {
            var action = _normalActions.Dequeue();
            return action.BattleObject.CurrentHp == 0 ? Next() : action;
        }
    }
}
