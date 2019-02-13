using UnityEngine;
using System.Collections;
using LuaInterface;

namespace Mugen
{

    public enum CnsStateType
    {
        none = 0,
        // 残影
        AfterImage,
        // 残影时间
        AfterImageTime,
        AllPalFX,
        // 角度加
        AngleAdd,
        // 角度绘制
        AngleDraw,
        // 角度乘
        AngleMul,
        // 角度设置
        AngleSet,
    }

    public class CNSState
    {
        private string m_Name = string.Empty;
        
    }
}