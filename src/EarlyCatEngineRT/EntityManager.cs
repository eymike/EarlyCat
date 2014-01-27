using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarlyCatEngineRT
{
    public class Component
    {

    }

    public class EntityManager
    {
        private readonly List<ulong> m_entities = new List<ulong>();
        private Dictionary<KeyValuePair<ulong, Type>, Component> m_components = new Dictionary<KeyValuePair<ulong, Type>, Component>();

        static object ENTITY_COUTNER_LOCK = new object();
        private static ulong ENTITY_COUNTER = 0; 

        public UInt64 CreateEntity()
        {
            ulong result;
            lock(ENTITY_COUTNER_LOCK)
            {
                result = ENTITY_COUNTER++;
            }
            m_entities.Add(result);
            return result;
        }

        void KillEntity(ulong entity)
        {
            if(!m_entities.Remove(entity))
            {
                throw new ArgumentException(String.Format("Entity ID does not exist: {0}", entity));
            }

            var toRemove = m_components.Where(item => item.Key.Key == entity);
            foreach(var itemToRemove in toRemove)
            {
                m_components.Remove(itemToRemove.Key);
            }
        }

        void AddComponent<ComponentType>(ulong entity, ComponentType instance) where ComponentType : Component
        {
            var key = new KeyValuePair<ulong, Type>(entity, typeof(ComponentType));
            m_components.Add(key, instance);
        }

        bool HasComponent<ComponentType>(ulong entity)
        {
            var key = new KeyValuePair<ulong, Type>(entity, typeof(ComponentType));
            return m_components.ContainsKey(key);
        }

        void RemoveComponent<ComponentType>(ulong entity)
        {
            var key = new KeyValuePair<ulong, Type>(entity, typeof(ComponentType));
            m_components.Remove(key);
        }

        public IEnumerable<Component> GetComponentsOnEntity(ulong entity)
        {
            return m_components.Where(item => item.Key.Key == entity).Select(value => value.Value);
        }

        public IEnumerable<ComponentType> GetComponents<ComponentType>() where ComponentType : Component
        {
            return from pair in m_components
                   where pair.Key.Value == typeof(ComponentType)
                   select (ComponentType)pair.Value;
        }

        public IEnumerable<ulong> GetEntitiesWithComponent<ComponentType>() where ComponentType : Component
        {
            return from pair in m_components
                   where pair.Key.Value == typeof(ComponentType)
                   select pair.Key.Key;
        }
    }
}
