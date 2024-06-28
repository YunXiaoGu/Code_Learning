using UnityEngine;
using System.Collections;

namespace UI.Component
{
    /// <summary>
    /// 空白点击区域组件
    /// </summary>
    [RequireComponent(typeof(CanvasRenderer))]
    public class EmptyRaycast : MaskableGraphic
    {

        protected EmptyRaycast()
        {
            useLegacyMeshGeneration = false;
        }

        protected override void OnPopulateMesh(VertexHelper toFill)
        {
            toFill.Clear();
        }
    }
}
