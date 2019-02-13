;------------------------------------------------------------------------------
;==============================================================================
; 入力キーの設定パート
;==============================================================================
;------------------------------------------------------------------------------


;------------------------------------------------------------------------------
;-| 超必殺技 |-----------------------------------------------------------------
:------------------------------------------------------------------------------
;花嵐
[Command]
name = "花嵐"
command = ~D, DF, F, D, DF, F, x
time = 20

:------------------------------------------------------------------------------
;超必殺忍蜂
[Command]
name = "超必殺忍蜂"
command = ~D, ~DB, B, DB, D, DF, F, a
time = 20

:------------------------------------------------------------------------------
;レオタード忍蜂
[Command]
name = "レオタード忍蜂"
command = ~D, ~DB, B, DB, D, DF, F, b
time = 20

:------------------------------------------------------------------------------
;花嵐２
[Command]
name = "花嵐２"
command = ~D, DF, F, D, DF, F, y
time = 20

;------------------------------------------------------------------------------
;-| 必殺技 |-------------------------------------------------------------------
;------------------------------------------------------------------------------
;花蝶扇
[Command]
name = "花蝶扇"
command = ~D, DF, F, x
time = 10

;------------------------------------------------------------------------------
;乱れ花蝶扇
[Command]
name = "乱れ花蝶扇"
command = ~D, DF, F, y
time = 10

;------------------------------------------------------------------------------
;必殺忍蜂・弱
[Command]
name = "必殺忍蜂・弱"
command = ~B, ~DB, D, DF, F, a
time = 12

;------------------------------------------------------------------------------
;必殺忍蜂・強
[Command]
name = "必殺忍蜂・強"
command = ~B, ~DB, D, DF, F, b
time = 12

;------------------------------------------------------------------------------
;龍炎舞・弱
[Command]
name = "龍炎舞・弱"
command = ~D, DB, B, x
time = 10

;------------------------------------------------------------------------------
;龍炎舞・強
[Command]
name = "龍炎舞・強"
command = ~D, DB, B, y
time = 10

;------------------------------------------------------------------------------
;小夜千鳥・弱
[Command]
name = "小夜千鳥・弱"
command = ~D, DB, B, a
time = 10

;------------------------------------------------------------------------------
;小夜千鳥・強
[Command]
name = "小夜千鳥・強"
command = ~D, DB, B, b
time = 10

;------------------------------------------------------------------------------
;陽炎の舞
[Command]
name = "陽炎の舞"
command = ~D, D, y
time = 16

;------------------------------------------------------------------------------
;-| キー２回連続入力 |---------------------------------------------------------
;------------------------------------------------------------------------------
[Command]
name = "FF"     
command = F, F
time = 10

[Command]
name = "BB"     
command = B, B
time = 10

;------------------------------------------------------------------------------
;-| 同時押し |-----------------------------------------------------------------
;------------------------------------------------------------------------------
[Command]
name = "recovery"  
command = x+y
time = 1

[Command]
name = "recovery"
command = a+b
time = 1

[Command]
name = "combo"  
command = y+b
time = 1

;------------------------------------------------------------------------------
;-| 方向キー＋ボタン |---------------------------------------------------------
;------------------------------------------------------------------------------
[Command]
name = "down_a"
command = /$D,a
time = 1

[Command]
name = "down_b"
command = /$D,b
time = 1

-------------------------------------------------------------------------------
;-| ボタン単発 |---------------------------------------------------------------
;------------------------------------------------------------------------------
[Command]
name = "a"
command = a
time = 1

[Command]
name = "b"
command = b
time = 1

[Command]
name = "c"
command = c
time = 1

[Command]
name = "x"
command = x
time = 1

[Command]
name = "y"
command = y
time = 1

[Command]
name = "z"
command = z
time = 1

[Command]
name = "start"
command = s
time = 1

;----------------------------- KOF Seriese only
[Command]
name = "gc_fwd"
command = c
time = 1

;------------------------------------------------------------------------------
;-| 方向キー押しっぱなし |-----------------------------------------------------
;------------------------------------------------------------------------------
[Command]
name = "holdfwd"   
command = /$F
time = 1

[Command]
name = "holdback"  
command = /$B
time = 1

[Command]
name = "holdup"    
command = /$U
time = 1

[Command]
name = "holddown"  
command = /$D
time = 1

[Command]
name = "holdx"  
command = /$x
time = 1

[Command]
name = "holdy"  
command = /$y
time = 1

;------------------------------------------------------------------------------
;==============================================================================
; 技を実行するための条件の設定（ステートエントリーパート）
;==============================================================================
;------------------------------------------------------------------------------

[Statedef -1]

;------------------------------------------------------------------------------
;-| 超必殺技 |-----------------------------------------------------------------
;------------------------------------------------------------------------------
;超必殺忍蜂
[State -1, 超必殺忍蜂]
type = ChangeState
value = 2000
triggerall = command = "超必殺忍蜂" && statetype != A
triggerall = Power >= 1000
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;花嵐
[State -1, 花嵐]
type = ChangeState
value = 2100
triggerall = command = "花嵐" && statetype != A
triggerall = Power >= 1000
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;レオタード忍蜂
[State -1, レオタード忍蜂]
type = ChangeState
value = 2200
triggerall = command = "レオタード忍蜂" && statetype != A
triggerall = Power >= 2000
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;花嵐２
[State -1, 花嵐２]
type = ChangeState
value = 2300
triggerall = command = "花嵐２" && statetype != A
triggerall = Power >= 2000
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;-| 必殺技 |-------------------------------------------------------------------
;------------------------------------------------------------------------------
;ダッシュ必殺忍蜂
[State -1, ダッシュ必殺忍蜂]
type = ChangeState
value = 1700
triggerall = statetype != A
triggerall = command = "必殺忍蜂・弱" || command = "必殺忍蜂・強"
trigger1 = stateno = 101
trigger2 = stateno = 250 || stateno = 260

;------------------------------------------------------------------------------
;花蝶扇
[State -1, 花蝶扇]
type = ChangeState
value = 1000
triggerall = command = "花蝶扇" && statetype != A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;乱れ花蝶扇
[State -1, 乱れ花蝶扇]
type = ChangeState
value = 1100
triggerall = command = "乱れ花蝶扇" && statetype != A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;必殺忍蜂・弱
[State -1, 必殺忍蜂・弱]
type = ChangeState
value = 1400
triggerall = command = "必殺忍蜂・弱" && statetype != A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;必殺忍蜂・強
[State -1, 必殺忍蜂・強]
type = ChangeState
value = 1450
triggerall = command = "必殺忍蜂・強" && statetype != A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;ムササビの舞・弱
[State -1, ムササビの舞・弱]
type = ChangeState
value = 1500
triggerall = command = "龍炎舞・弱" && statetype = A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [300,320])
trigger3 =  MoveContact && (stateno = [600,700])

;------------------------------------------------------------------------------
;ムササビの舞・強
[State -1, ムササビの舞・強]
type = ChangeState
value = 1550
triggerall = command = "龍炎舞・強" && statetype = A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [300,320])
trigger3 =  MoveContact && (stateno = [600,700])

;------------------------------------------------------------------------------
;龍炎舞・弱
[State -1, 龍炎舞・弱]
type = ChangeState
value = 1200
triggerall = command = "龍炎舞・弱" && statetype != A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;龍炎舞・強
[State -1, 龍炎舞・強]
type = ChangeState
value = 1250
triggerall = command = "龍炎舞・強" && statetype != A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;小夜千鳥・弱
[State -1, 小夜千鳥・弱]
type = ChangeState
value = 1300
triggerall = command = "小夜千鳥・弱" && statetype != A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;小夜千鳥・強
[State -1, 小夜千鳥・強]
type = ChangeState
value = 1350
triggerall = command = "小夜千鳥・強" && statetype != A
trigger1 = ctrl 
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;陽炎の舞
[State -1, 陽炎の舞]
type = ChangeState
value = 1600
triggerall = command = "陽炎の舞" && statetype != A
trigger1 = ctrl 
trigger2 = stateno = [10,12]
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 500
trigger5 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;-| 特殊技 |-------------------------------------------------------------------
;------------------------------------------------------------------------------
;ダッシュパンチ
[State -1, ダッシュパンチ]
type = ChangeState
value = 250
triggerall = command = "x" || command = "y"
trigger1 = stateno = 100

;------------------------------------------------------------------------------
;ダッシュキック
[State -1, ダッシュキック]
type = ChangeState
value = 260
triggerall = command = "a" || command = "b"
trigger1 = stateno = 100

;------------------------------------------------------------------------------
;紅鶴の舞
[State -1, 紅鶴の舞]
type = ChangeState
value = 500
triggerall = command = "a" && command = "holdfwd" && command = "holddown"
triggerall = statetype != A
trigger1 = ctrl
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;黒燕の舞
[State -1, 黒燕の舞]
type = ChangeState
value = 300
triggerall = command = "x" && command = "holdfwd"
trigger1 = statetype != A
trigger1 = ctrl

;------------------------------------------------------------------------------
;キャンセル黒燕の舞
[State -1, キャンセル黒燕の舞]
type = ChangeState
value = 310
triggerall = command = "x" && command = "holdfwd"
triggerall = statetype != A
trigger1 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger2 =  MoveContact && (stateno = [350,375])
trigger3 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;椿の舞
[State -1, 椿の舞]
type = ChangeState
value = 320
triggerall = command = "a" && command = "holdfwd"
triggerall = statetype != A
trigger1 = ctrl
trigger2 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger3 =  MoveContact && (stateno = [350,375])
trigger4 =  MoveContact && stateno = 3060

;------------------------------------------------------------------------------
;大輪風車落とし
[State -1, 大輪風車落とし]
type = ChangeState
value = 700
triggerall = command = "x" && command = "holddown"
trigger1 = statetype = A
trigger1 = ctrl
trigger2 =  MoveContact && (stateno = [600,635])

;------------------------------------------------------------------------------
;レイジコンビネーション始動
[State -1, レイジコンビネーション始動]
type = ChangeState
value = 3000
triggerall = NumPartner = 0
triggerall = NumEnemy = 1
triggerall = statetype != A
triggerall = command = "combo"
trigger1 = ctrl

;------------------------------------------------------------------------------
;クイックレイジコンビネーション始動
[State -1, クイックレイジコンビネーション始動]
type = ChangeState
value = 3200
triggerall = NumPartner = 0
triggerall = NumEnemy = 1
triggerall = statetype != A
triggerall = command = "combo"
triggerall = Power >= 1000
trigger1 =  MoveContact && (stateno = [200,235]) || MoveContact && (stateno = [400,420])
trigger2 =  MoveContact && (stateno = [350,375])
trigger3 =  MoveContact && stateno = 500
trigger4 =  stateno = 250

;------------------------------------------------------------------------------
;==============================================================================

;------------------------------------------------------------------------------
;-| 通常動作 |-----------------------------------------------------------------
;------------------------------------------------------------------------------
;ダッシュ
[State -1, Dash]
type = ChangeState
value = 100
trigger1 = command = "FF"
trigger1 = statetype = S
trigger1 = ctrl

;バックステップ
[State -1, Back Step]
type = ChangeState
value = 105
trigger1 = command = "BB"
trigger1 = statetype = S
trigger1 = ctrl

;==============================================================================

;------------------------------------------------------------------------------
;投げ１
[State -1]
type = ChangeState
value = 800
triggerall = command = "y"
triggerall = statetype = S
triggerall = ctrl
triggerall = stateno != 100
trigger1 = command = "holdfwd" || command = "holdback"
trigger1 = p2bodydist X < 10
trigger1 = (p2statetype = S) || (p2statetype = C)
trigger1 = p2movetype != H

;------------------------------------------------------------------------------
;投げ２
[State -1]
type = ChangeState
value = 850
triggerall = command = "b"
triggerall = statetype = S
triggerall = ctrl
triggerall = stateno != 100
trigger1 = command = "holdfwd" || command = "holdback"
trigger1 = p2bodydist X < 10
trigger1 = (p2statetype = S) || (p2statetype = C)
trigger1 = p2movetype != H

;==============================================================================
;------------------------------------------------------------------------------

;------------------------------------------------------------------------------
;-| 立ち通常攻撃 |-------------------------------------------------------------
;------------------------------------------------------------------------------
;近立ち弱パンチ
[State -1, 近立ち弱パンチ]
type = ChangeState
value = 205
triggerall = p2bodydist X < 20
triggerall = command = "x"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;遠立ち弱パンチ
[State -1, 遠立ち弱パンチ]
type = ChangeState
value = 200
triggerall = command = "x"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;------------------------------------------------------------------------------
;近立ち強パンチ
[State -1, 近立ち強パンチ]
type = ChangeState
value = 215
triggerall = p2bodydist X < 20
triggerall = command = "y"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;遠立ち強パンチ
[State -1, 遠立ち強パンチ]
type = ChangeState
value = 210
triggerall = command = "y"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;-----------------------------------------------------------------------------
;近立ち弱キック
[State -1, 近立ち弱キック]
type = ChangeState
value = 225
triggerall = p2bodydist X < 20
triggerall = command = "a"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;遠立ち弱キック
[State -1, 遠立ち弱キック]
type = ChangeState
value = 220
triggerall = command = "a"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;------------------------------------------------------------------------------
;近立ち強キック
[State -1, 近立ち強キック]
type = ChangeState
value = 235
triggerall = p2bodydist X < 20
triggerall = command = "b"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;遠立ち強キック
[State -1, 遠立ち強キック]
type = ChangeState
value = 230
triggerall = command = "b"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;---------------------------------------------------------------------------
;ぶっ飛ばし
[State -1, ぶっ飛ばし]
type = ChangeState
value = 240
triggerall = command = "z"
triggerall = command != "holddown"
trigger1 = statetype = S && ctrl

;------------------------------------------------------------------------------
;挑発
[State -1, 挑発]
type = ChangeState
value = 160
triggerall = command = "start"
trigger1 = statetype != A
trigger1 = ctrl

;==============================================================================
;------------------------------------------------------------------------------

;------------------------------------------------------------------------------
;-| しゃがみ通常攻撃 |---------------------------------------------------------
;------------------------------------------------------------------------------
;しゃがみ弱パンチ
[State -1, しゃがみ弱パンチ]
type = ChangeState
value = 400
triggerall = command = "x"
triggerall = command = "holddown"
trigger1 = statetype = C
trigger1 = ctrl

;しゃがみ強パンチ
[State -1, しゃがみ強パンチ]
type = ChangeState
value = 410
triggerall = command = "y"
triggerall = command = "holddown"
trigger1 = statetype = C
trigger1 = ctrl

;------------------------------------------------------------------------------
;しゃがみ弱キック
[State -1, しゃがみ弱キック]
type = ChangeState
value = 420
triggerall = command = "a"
triggerall = command = "holddown"
trigger1 = statetype = C
trigger1 = ctrl

;しゃがみ強キック
[State -1, しゃがみ強キック]
type = ChangeState
value = 430
triggerall = command = "b"
triggerall = command = "holddown"
trigger1 = statetype = C
trigger1 = ctrl

;==============================================================================
;------------------------------------------------------------------------------

;------------------------------------------------------------------------------
;-| ジャンプ通常攻撃 |---------------------------------------------------------
;------------------------------------------------------------------------------
;垂直ジャンプ弱パンチ
[State -1, 垂直ジャンプ弱パンチ]
type = ChangeState
value = 600
triggerall = command = "x"
triggerall = vel X = 0
trigger1 = statetype = A
trigger1 = ctrl

;------------------------------------------------------------------------------
;ジャンプ弱パンチ
[State -1, ジャンプ弱パンチ]
type = ChangeState
value = 605
triggerall = command = "x"
triggerall = vel X != 0
trigger1 = statetype = A
trigger1 = ctrl

;------------------------------------------------------------------------------
;垂直ジャンプ強パンチ
[State -1, 垂直ジャンプ強パンチ]
type = ChangeState
value = 610
triggerall = command = "y"
triggerall = vel X = 0
trigger1 = statetype = A
trigger1 = ctrl

;------------------------------------------------------------------------------
;ジャンプ強パンチ
[State -1, ジャンプ強パンチ]
type = ChangeState
value = 615
triggerall = command = "y"
triggerall = vel X != 0
trigger1 = statetype = A
trigger1 = ctrl

;------------------------------------------------------------------------------
;垂直ジャンプ弱キック
[State -1, 垂直ジャンプ弱キック]
type = ChangeState
value = 620
triggerall = command = "a"
triggerall = vel X = 0
trigger1 = statetype = A
trigger1 = ctrl

;------------------------------------------------------------------------------
;ジャンプ弱キック
[State -1, ジャンプ弱キック]
type = ChangeState
value = 625
triggerall = command = "a"
triggerall = vel X != 0
trigger1 = statetype = A
trigger1 = ctrl

;------------------------------------------------------------------------------
;垂直ジャンプ強キック
[State -1, 垂直ジャンプ強キック]
type = ChangeState
value = 630
triggerall = command = "b"
triggerall = vel X = 0
trigger1 = statetype = A
trigger1 = ctrl

;------------------------------------------------------------------------------
;ジャンプ強キック
[State -1, ジャンプ強キック]
type = ChangeState
value = 635
triggerall = command = "b"
triggerall = vel X != 0
trigger1 = statetype = A
trigger1 = ctrl

;==============================================================================
;------------------------------------------------------------------------------

;------------------------------------------------------------------------------
;-| 特集動作 |-----------------------------------------------------------------
;------------------------------------------------------------------------------
; 緊急回避動作（前）
[State -1]
type = ChangeState
value = 900
triggerall = Command = "c" && !Command = "holdback"
trigger1 = StateType = S && (Ctrl = 1 || StateNo = 100)

;------------------------------------------------------------------------------
; 緊急回避動作（後）
[State -1]
type = ChangeState
value = 910
triggerall = Command = "c" && Command = "holdback"
trigger1 = StateType = S && (Ctrl = 1 || StateNo = 100)

;------------------------------------------------------------------------------
; ガードキャンセル緊急回避動作（前・後）
[State -1]
type = ChangeState
value = 950
triggerall = StateNo = 150 || StateNo = 152
trigger1 = Command = "c" && Power >= 1000
trigger1 = StateType != A

;------------------------------------------------------------------------------
; ガードキャンセルぶっ飛ばし
[State -1]
type = ChangeState
value = 970
triggerall = StateNo = 150 || StateNo = 152
trigger1 = Command = "z" && Power >= 1000
trigger1 = StateType != A
