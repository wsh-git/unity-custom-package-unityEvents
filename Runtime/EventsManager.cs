using System.Collections.Generic;
using Wsh.Singleton;

namespace Wsh.Events {

    public delegate void EventsDelegate(params object[] args);

    public class EventsManager : NonMonoSingleton<EventsManager> {

        private Dictionary<string, List<EventsDelegate>> m_eventsDic;

        protected override void OnInit() {
            m_eventsDic = new Dictionary<string, List<EventsDelegate>>();
        }
        
        public void Subscribe(string eventName, EventsDelegate handler) {
            if(!m_eventsDic.ContainsKey(eventName)) {
                m_eventsDic.Add(eventName, new List<EventsDelegate>());
            }
            m_eventsDic[eventName].Add(handler);
        }

        public void Unsubscribe(string eventName, EventsDelegate handler) {
            if(m_eventsDic.ContainsKey(eventName) && m_eventsDic[eventName].Contains(handler)) {
                m_eventsDic[eventName].Remove(handler);
            }
        }

        public void UnsubscribeAllHandlers(string eventName) {
            if(m_eventsDic.ContainsKey(eventName)) {
                m_eventsDic[eventName].Clear();
            }
        }

        public void TriggerEvent(string eventName, params object[] args) {
            if(m_eventsDic.TryGetValue(eventName, out var handlers)) {
                for(int i = 0; i < handlers.Count; i++) {
                    handlers[i]?.Invoke(args);
                }
            }
        }

        protected override void OnDeinit() {
            foreach(var key in m_eventsDic.Keys) {
                m_eventsDic[key].Clear();
            }
            m_eventsDic.Clear();
        }
    }

}