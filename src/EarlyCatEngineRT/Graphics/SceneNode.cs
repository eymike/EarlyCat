using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;
using EarlyCatEngineRT.Math;

namespace EarlyCatEngineRT.Graphics
{
    public class SceneNode
    {
        private readonly Transform m_transform = new Transform();

        private SceneNode m_parent;
       
        void SetParent(SceneNode node)
        {
            m_parent = node;
        }

        public Transform LocalTransform
        {
            get { return m_transform; }
        }

        public Matrix GlobalTransform
        {
            get
            {
                Matrix parentXForm;
                if(m_parent != null)
                {
                    parentXForm = m_parent.GlobalTransform;
                }
                else
                {
                    return m_transform.TransformMatrix;
                }

                return m_transform.TransformMatrix * parentXForm;
            }
        }
    }
}
