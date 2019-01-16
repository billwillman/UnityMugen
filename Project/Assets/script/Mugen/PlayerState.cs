using System;

namespace Mugen
{
	public enum PlayerState
	{
		psNone = -9999,
		// Stand(loop)
		psStand1 = 0,
		psStand2 = 1,
		psStand3 = 2,

		// NoLoop
		psStandTurn1 = 5,
		psStandTurn2 = 6,
		psStandTurn3 = 7,

		// Stand To Down
		psStandToDown1 = 10,
		// Down(loop)
		psDown1 = 11,
		// No Loop
		psDownToStand1 = 12,

		// Stand To Down
		psStandToDown2 = 13,
		// Down(loop)
		psDown2 = 14,
		// No Loop
		psDownToStand2 = 15,

		// Stand To Down
		psStandToDown3 = 16,
		// Down(loop)
		psDown3 = 17,
		// No Loop
		psDownToStand3 = 18,

		psForwardWalk1 = 20,
		psBackWalk1 = 21,

		psForwardWalk2 = 22,
		psBackWalk2 = 23,

		psForwardWalk3 = 24,
		psBackWalk3 = 25,

		psStartJump1 = 40,
		psUpJumpUp1 = 41,
		psForwardJumpUp1 = 42,
		psBackJumpUp1 = 43,
		// Y - velocity > -2
		psUpJumpDown1 = 44,
		// Y - velocity > -2
		psForwardJumpDown1 = 45,
		// Y - velocity > -2
		psBackJumpDown1 = 46,
		psEndJump1 = 47,

		psStartJump2 = 50,
		psUpJumpUp2 = 51,
		psForwardJumpUp2 = 52,
		psBackJumpUp2 = 53,
		// Y - velocity > -2
		psUpJumpDown2 = 54,
		// Y - velocity > -2
		psForwardJumpDown2 = 55,
		// Y - velocity > -2
		psBackJumpDown2 = 56,
		psEndJump2 = 57,

		psStartJump3 = 60,
		psUpJumpUp3 = 61,
		psForwardJumpUp3 = 62,
		psBackJumpUp3 = 63,
		// Y - velocity > -2
		psUpJumpDown3 = 64,
		// Y - velocity > -2
		psForwardJumpDown3 = 65,
		// Y - velocity > -2
		psBackJumpDown3 = 66,
		psEndJump3 = 67,

		psForwardRun1 = 100,
		psForwardRun2 = 101,
		psForwardRun3 = 102,

		psBackStep1 = 105,
		psBackStep2 = 106,
		psBackStep3 = 107,

		psStandDefenseStart1 = 120,
		psDownDefenseStart1 = 121,
		psFlyDefenseStart1 = 122,

		psStandDefenseStart2 = 123,
		psDownDefenseStart2 = 124,
		psFlyDefenseStart2 = 125,

		psStandDefenseStart3 = 126,
		psDownDefenseStart3 = 127,
		psFlyDefenseStart3 = 128,

		psStandDefense1 = 130,
		psDownDefense1 = 131,
		psFlyDefense1 =132,

		psStandDefense2 = 133,
		psDownDefense2 = 134,
		psFlyDefense2 = 135,

		psStandDefense3 = 136,
		psDownDefense3 = 137,
		psFlyDefense3 = 138,

		psStandDefenseEnd1 = 140,
		psDownDefenseEnd1 = 141,
		psFlyDefenseEnd1 = 142,

		psStandDefenseEnd2 = 143,
		psDownDefenseEnd2 = 144,
		psFlyDefenseEnd2 = 145,

		psStandDefenseEnd3 = 146,
		psDownDefenseEnd3 = 147,
		psFlyDefenseEnd3 = 148,

		// loop
		psStandDefenseState1 = 150,
		psDownDefenseState1 = 151,
		psFlyDefenseState1 = 152,

		// loop
		psStandDefenseState2 = 153,
		psDownDefenseState2 = 154,
		psFlyDefenseState2 = 155,

		// loop
		psStandDefenseState3 = 156,
		psDownDefenseState3 = 157,
		psFlyDefenseState3 = 158,

		psFailed = 170,
		psTimeEnd = 175,

		psWinner = 180,
		psWinner1 = 181,
		psWinner2 = 182,

		// 受击
		psStandOrFlyLowStruck = 5000,
		psStandOrFlyMedStruck = 5001,
		psStandOrFlyHardStruck = 5002,

		psFlyDowing1 = 5051,

		psFlyDown1 = 5061,

		// not used
		//psPlayerStateCount 
	}

    public static class PlayerStateEnumValues
    {
        private static Array m_Values = null;
        public static Array GetValues()
        {
            if (m_Values == null)
            {
                m_Values = System.Enum.GetValues(typeof(PlayerState));
            }
            return m_Values;
        }

        public static Array GetEnums(this PlayerState state)
        {
            return PlayerStateEnumValues.GetValues();
        }
    }
}