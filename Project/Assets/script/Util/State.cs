using System;
using System.Collections.Generic;

namespace Utils
{
    public interface IState<U, V> {
        bool CanEnter(V target);
        bool CanExit(V target);
        void Enter(V target);
        void Exit(V target);
        void Process(V target);

        U Id {
            get;
            set;
        }
    }

	public interface IStateMgrListener<U, V>
	{
		bool CanEnter(V target, U id, U newId, ref bool isDone);
		bool CanExit(V target, U id, ref bool isDone);
		void Enter(V target, U id);
		void Exit(V target, U id);
		void Process(V target, U id, ref bool isDone);
	}

    public class StateMgr<U, V> where V : class {

		private static IStateMgrListener<U, V> m_Listener = null;

		public static void SetListener(IStateMgrListener<U, V> listener)
		{
			m_Listener = listener;
		}

		public IStateMgrListener<U, V> Listener
		{
			get {
				return m_Listener;
			}
		}

        public StateMgr(V target) {
            m_Target = target;
        }

        public virtual void Process(V target) {

			if (m_Listener != null)
			{
				bool isDone = false;
				m_Listener.Process (target, m_CurrKey, ref isDone);
				if (isDone)
					return;
			}

            IState<U, V> state = CurrState;
            if (state != null)
                state.Process(target);
        }

        public virtual bool ChangeState(U id) {
            if (m_Target == null)
                return false;

			bool isDone = false;
			if (m_Listener != null)
			{
				bool r = m_Listener.CanExit (m_Target, this.m_CurrKey, ref isDone);
				if (isDone && !r)
					return false;
			}

			IState<U, V> now = null;
			if (!isDone) {
				now = m_CurrStatus;
				if (now != null) {
					if (!now.CanExit (m_Target))
						return false;
				}
			}

			isDone = false;
			if (m_Listener != null) {
				bool r = m_Listener.CanEnter (m_Target, this.m_CurrKey, id, ref isDone);
				if (isDone && !r)
					return false;
			}

			IState<U, V> news = null;
			if (isDone && m_Listener != null) {
				m_Listener.Exit (m_Target, m_CurrKey);
			} else {
				
				if (m_StateMap.TryGetValue (id, out news) && news != null) {
					if (!news.CanEnter (m_Target))
						return false;
				} else
					return false;

				if (now != null)
					now.Exit (m_Target);
			}

            m_CurrKey = id;
            m_CurrStatus = news;

			if (isDone && m_Listener != null) {
				m_Listener.Enter (m_Target, m_CurrKey);
			} else {
				if (news != null)
					news.Enter (m_Target);
			}

            return true;
        }

        public U CurrStateKey {
            get {
                return m_CurrKey;
            }
        }

        public IState<U, V> CurrState {
            get {
                return m_CurrStatus;
            }
        }

        public static void Register(U id, IState<U, V> state) {
            IState<U, V> now;
            if (m_StateMap.TryGetValue(id, out now))
                m_StateMap[id] = state;
            else {
                m_StateMap.Add(id, state);
            }
        }

        public static bool FindState(U id, out IState<U, V> target) {
            bool ret = m_StateMap.TryGetValue(id, out target);
            return ret;
        }

        protected V m_Target;
        protected static Dictionary<U, IState<U, V>> m_StateMap = new Dictionary<U, IState<U, V>>();
        protected U m_CurrKey;
        protected IState<U, V> m_CurrStatus = null;
    }
}

