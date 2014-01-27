using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX;

namespace EarlyCatEngineRT.Math
{
    public class Transform
    {
        private Vector3 m_translation = Vector3.Zero;
        private Quaternion m_rotation = Quaternion.Identity;
        private Vector3 m_scale = Vector3.Zero;

        public void Translate(ref Vector3 translation)
        {
            m_translation += translation;
        }

        public void Rotate(ref Quaternion rotation)
        {
            m_rotation *= rotation;
            m_rotation.Normalize();
        }

        public void Scale(ref Vector3 scaling)
        {
            m_scale += scaling;
        }

        public Matrix TransformMatrix
        {
            get
            {
                return Matrix.Scaling(m_scale) * Matrix.RotationQuaternion(m_rotation) * Matrix.Translation(m_translation);
            }
        }

        public Vector3 Translation
        {
            get { return m_translation; }
            set { m_translation = value; }
        }

        public Quaternion Rotation
        {
            get { return m_rotation; }
            set { m_rotation = value; }
        }

        public Vector3 Scaling
        {
            get { return m_scale; }
            set { m_scale = value; }
        }
    }
}
