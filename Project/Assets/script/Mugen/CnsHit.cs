using System.Collections;
using System.Collections.Generic;

namespace Mugen {

    // 攻击标识
    // 对应attr第二个参数第一个字母
    public enum Cns_Hit1 {
        none = 0,
        N, // 普通技
        S, // 特殊技
        H  // 无敌
    }

    // 攻击载体
    // 对应attr第二个参数第二个字母
    public enum Cns_Hit2 {
        none,
        A, // 直接攻击
        P, // 发射道具
        T  // 抓投
    }

    //  这个参数用于控制对手被攻击以后的动作（受创后仰姿势）和能否被追打.
    public enum Cns_HitFlag {
        none = 0,
        H, // 上段攻击
        L, // 下段攻击
        A, // 空中攻击
        M, // 中段攻击 相当与“HL”
        F, // 坠落追打, 如果有这个字母将可以追打到被攻击后飞到天上的对手
        D, // 地面追, 如果有这个字母将可以追打到被攻击后倒地的对手.
    }

    //  这个参数用于检测对手的姿势能否防御住这个攻击.
    public enum Cns_HitGuardFlag {
        none = 0,
        H, // 上段
        L, // 下段 
        A, // 空中
        M, // 中段 相当于"HL"
    }

    public enum Cns_HitFlagEx {
        none = 0,
        Add, // 如果有"+"那么攻击只在对手处于gethit state时有效.
             // 这常用于第一下没有击中后面的都不会影响对手的连续动作.

        Sub, // 如果有"-"那么攻击只在对手不在gethit state时候有效
             //  这常用于抓投和其他动作（下面这句不明白什么意思）
             // 和其他你在攻击状态时不希望对手被其他攻击打到的时候用。
             //例如自己在抓投对手的准备姿势中防止被对手打到而破坏了抓投.
             //Add 和 Sub 不能同时用
    }

    // 对哪些人攻击起效 默认为 E.
    public enum Cns_Affectteam {
        B, // 两队（全部人物）
        E, // 敌方
        F, // 友方（你的队友）.
    }

    /*
     * 
  hit_type, 说明双方在同样的hit_prior优先级同时击中对手的时候如何反应，可以是：
  Dodge,Hit,Miss

  效果如下:
  Hit vs. Hit: 双方都被打
  Hit vs. Miss: 是Hit的被打,是Miss的没打中
  Hit vs. Dodge: 都不被打
  Dodge vs. Dodge: 都不被打
  Dodge vs. Miss: 都不被打
  Miss vs. Miss: 都不被打
  （注意miss和dodge的不同）

  注意即使都不被打, 各自的HitDefs仍然是有效的.
  */
    public enum Cns_HitType {
        Dodge,
        Hit,
        Miss
    }

    public class CnsHitDef {
        // 击中伤害
        public int hit_damage = 0;
        // 防御伤害
        public int guard_damage = 0;
        // 站立状态(bit位)，确定哪些状态会遭受攻击
        // 对应 attr 第一个参数
        public byte targetStandType = 0;
        // 对应attr第二个参数第一个字母
        public Cns_Hit1 attr2_1 = Cns_Hit1.none;
        // 对应attr第二个参数第二个字母
        public Cns_Hit2 attr2_2 = Cns_Hit2.none;
        // Cns_HitFlag 的组合，bit位。默认值MAF,意思为：中段和空中都可能会受到攻击，也可以打击坠落的对手
        public byte hitflags = (1 << (int)Cns_HitFlag.M) | (1 << (int)Cns_HitFlag.A) | (1 << (int)Cns_HitFlag.F); 
        public Cns_HitFlagEx hitflagEx = Cns_HitFlagEx.none;
        // Cns_HitGuardFlag的BIT组合
        public byte hitGuardFlags = 0;
        // 对哪些人攻击起效 默认为 E.
        public Cns_Affectteam targetTeam = Cns_Affectteam.E;
        public int hit_prior = 4; // hit_prior说明攻击的优先次序.同时出手的优先高的攻击会比优先低的攻击先打到对手.
                                  // 有效的hit_prior值为1-7.默认是4.
        public Cns_HitType hit_type = Cns_HitType.Hit;

        public int hit_p1_pausetime = 0; // 是自己的人物攻击打中时的停顿时间,单位是game-ticks. AppConfig.GetInstance().DeltaTick
        public int hit_p2_pausetime = 0; // 是对手被被打中后，向后滑移前的震动时间
        public int guard_p1_pausetime = 0; // 防御时
        public int guard_p2_pausetime = 0; // 防御时

        public bool isCommSparkNo = true; // 是否从Common.sff中
        public int sparkno = (int)PlayerState.psNone; // 火花编号  这个参数说明攻击成功的话显示的火花anim号

        public bool isCommGuardSparkNo = true;  // 是否从Common.sff中
        public int guard_sparkno = (int)PlayerState.psNone; //   同sparkno，指定攻击被防御时候出现的火花.

        //  这个参数矫正播放火花的位置到打击点的距离.
        // spark_x 和对手的位置有关，负数会更深入对手，正数远离对手。
        //  spark_y is 和自己位置有关，负数偏上，正数偏下。
        public int spark_x = 0;
        public int spark_y = 0;

        // 击中声音
        public bool isCommonHitSound = true; // 是否从common.snd中读取声音
        public int hitsound_Group = -1;
        public int hitSound_Image = -1;

        // 防御声音
        public bool isCommonGuardHitSound = true;
        public int guardsound_Group = -1;
        public int guardsound_Image = -1;

        public int numhits = 1; //  hit_count 设定这次攻击会在显示出的连击数字上增加几. 默认为1.

        public bool kill = true; // 设为false则攻击命中的话即使对手血再少也不会被这个攻击KO掉（没血的时候被攻击不死）.
        public bool guard_kill = true;  // 设为false则攻击被防御的话即使对手血再少也不会被这个攻击KO掉（没血的时候被攻击不死）.
        public bool fall_kill = true; // 设为false则在FALL状态的对手血再少也不会被这次攻击KO掉（没血的时候被攻击不死）.

        public bool hitonce = false;  //  如果设为true,则攻击只对一个目标起作用（多人战）。如果击中一个，其他目标就不会被攻击到.
                                      //一般默认为false，只有在"attr"参数里设定此HITDEF是throw type（发射道具）, 默认是1。

        //   如果是 -1 (默认值),那么不明确指定从哪个ID的HIT连击会无效。
        // 如果对手被当前攻击和前一个攻击之间的另一部分击中这个参数没有影响。
        public int nochain_1 = -1;
        public int nochain_2 = -1;

        //  攻击给对手造成的竖直方向的加速度。
        public float yaccel = 0;
    }
}
