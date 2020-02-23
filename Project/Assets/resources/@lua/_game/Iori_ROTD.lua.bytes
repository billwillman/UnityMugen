local trigger = require("trigger")
local _InitCommonCns = require("commonCns")

local setmetatable = setmetatable

local Iori_ROTD = {}
Iori_ROTD.__index = Iori_ROTD


function Iori_ROTD:new()
   -- 静态数据
   if self._isInit == nil then
		self._isInit = true
		self:_initData()
		self:_initSize()
		self:_initStateDefs()
		self:_initCmds()
    end
   -- 动态数据
   local t = {PlayerDisplay = nil}
   local ret = setmetatable(t, Iori_ROTD)
   --print(ret)
   return ret
end

--====================外部调用接口==============================

function Iori_ROTD:OnInit(playerDisplay)
	--print(playerDisplay)
	self.PlayerDisplay = playerDisplay;
	--print(self.PlayerDisplay)
	trigger:Help_InitLuaPlayer(self, self)
	-- 初始化默认Cns状态
	_InitCommonCns(self)
end

function Iori_ROTD:OnDestroy()
  self.PlayerDisplay = nil
  --print(null)
end

function Iori_ROTD:OnGetAICommandName(cmdName)
	return ""
end

--===========================================================

function Iori_ROTD:_initData()
  if self.Data ~= nil then
	return
  end
  self.Data = {};
  
  self.Data.life = 1000
  self.Data.Power = 3000
  self.Data.attack = 100
  self.Data.defence = 100
  
  
  self.Data.fall = {}
  self.Data.fall.defence_up = 50
  
  self.Data.liedown = {}
  self.Data.liedown.time = 60
  
  self.Data.airjuggle = 15
  self.Data.sparkno = 200
  
  self.Data.guard = {}
  self.Data.guard.sparkno = 40
  
  self.Data.KO = {}
  self.Data.KO.echo = 0
  
  self.Data.volume = 0
  self.Data.IntPersistIndex = 60
  self.Data.FloatPersistIndex = 40

  	self.velocity = {}
	self.velocity.run = {}
	self.velocity.run.fwd = Vector2.New(6.5, 0)
	self.velocity.run.back = Vector2.New(-6.5,-2.9)
end

function Iori_ROTD:_initSize()
  if self.Size ~= nil then
	return
  end
  self.Size = {}
  self.Size.xscale = 1
  self.Size.yscale = 1
end

--=====================================创建StateDef===================================
debugStartTime = 0
--创建StateDef
function Iori_ROTD:_initStateDefs()
	local luaCfg = trigger:GetLuaCnsCfg("Iori-ROTD")
	if luaCfg == nil then
		return
	end
	
	-- 创建各种状态
	--self:_initStateDef_2000(luaCfg)
end

function Iori_ROTD:initCmd_QiangZhuang(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("强撞")
		cmd.time = 10
		cmd:AttachKeyCommands("")

--------------------------- 强撞 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("强撞")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "3000"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "强撞"))
						return triggle1
				end

--------------------------- register StateDef 3000 ---------------------------
		local id = luaCfg:CreateStateDef("3000")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.N

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 14

		def.PowerAdd = 0

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 1

		def.Animate = 3000

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 50, 2)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 3, 4)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:AnimElem(luaPlayer) == 2) or (trigger:AnimElem(luaPlayer) == 3))

						if trigger1 then

								trigger:PosAdd(luaPlayer, 8, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:AnimElem(luaPlayer) == 6) or (trigger:AnimElem(luaPlayer) == 7))

						if trigger1 then

								trigger:PosAdd(luaPlayer, -8, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

						if trigger1 then

								trigger:VelSet(luaPlayer, 3.5 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 4)

						if trigger1 then

								trigger:VelSet(luaPlayer, 0 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

						if trigger1 then

								trigger:VelMul(luaPlayer, 0.9, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end


end

function Iori_ROTD:initCmd_ZhuaRen(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("抓人")
		cmd.time = 10
		cmd:AttachKeyCommands("")

--------------------------- 抓人 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("抓人")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "810"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "抓人"))
						return triggle1
				end

--------------------------- register StateDef 810 ---------------------------
		local id = luaCfg:CreateStateDef("810")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.N

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 0

		def.PowerAdd = 0

		def.Ctrl = 0

		def.Sprpriority = 2

		def.Animate = 810

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 1)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 7, 0)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 5)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 56, 0)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 5)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 56, 1)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:AnimElem(luaPlayer) == 1) or (trigger:AnimElem(luaPlayer) == 2))

						if trigger1 then

								local explod = trigger:CreateExplod(luaPlayer)

								explod.anim = 6020

								explod.ID = 6020

								explod.pos_x = 30

								explod.pos_y = -55

								explod.postype = ExplodPosType.p1

								explod.removetime = -2

								explod.sprpriority = 4

								explod.removeongethit = 1

								explod.ignorehitpause = 1

								explod.isChangeStateRemove = false

								explod.IsUseParentUpdate = false

								explod:Apply()


						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 5)

						if trigger1 then

								local explod = trigger:CreateExplod(luaPlayer)

								explod.anim = 18100

								explod.ID = 18100

								explod.pos_x = 60

								explod.pos_y = -80

								explod.postype = ExplodPosType.p1

								explod.removetime = -2

								explod.sprpriority = 4

								explod.removeongethit = 1

								explod.ignorehitpause = 1

								explod.isChangeStateRemove = false

								explod.IsUseParentUpdate = false

								explod:Apply()


						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 5)

						if trigger1 then

								local explod = trigger:CreateExplod(luaPlayer)

								explod.anim = 18101

								explod.ID = 18100

								explod.pos_x = 60

								explod.pos_y = -80

								explod.postype = ExplodPosType.p1

								explod.removetime = -2

								explod.sprpriority = 4

								explod.removeongethit = 1

								explod.ignorehitpause = 1

								explod.isChangeStateRemove = false

								explod.IsUseParentUpdate = false

								explod:Apply()


						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:AnimElem(luaPlayer) == 5) or (trigger:AnimElem(luaPlayer) == 2))

						if trigger1 then

								local explod = trigger:CreateExplod(luaPlayer)

								explod.anim = 18102

								explod.ID = 18100

								explod.postype = ExplodPosType.p1

								explod.removetime = -2

								explod.sprpriority = 4

								explod.removeongethit = 1

								explod.ignorehitpause = 1

								explod.isChangeStateRemove = false

								explod.IsUseParentUpdate = false

								explod:Apply()


						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:AnimElem(luaPlayer) == 5) or (trigger:AnimElem(luaPlayer) == 2))

						if trigger1 then

								local explod = trigger:CreateExplod(luaPlayer)

								explod.anim = 18102

								explod.ID = 18100

								explod.postype = ExplodPosType.p1

								explod.removetime = -2

								explod.sprpriority = 4

								explod.removeongethit = 1

								explod.ignorehitpause = 1

								explod.isChangeStateRemove = false

								explod.IsUseParentUpdate = false

								explod:Apply()


						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:AnimElem(luaPlayer) == 5) or (trigger:AnimElem(luaPlayer) == 4))

						if trigger1 then

								local explod = trigger:CreateExplod(luaPlayer)

								explod.anim = 18102

								explod.ID = 18100

								explod.pos_x = -25

								explod.pos_y = -15

								explod.postype = ExplodPosType.p1

								explod.removetime = -2

								explod.sprpriority = 4

								explod.removeongethit = 1

								explod.ignorehitpause = 1

								explod.isChangeStateRemove = false

								explod.IsUseParentUpdate = false

								explod:Apply()


						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = ((trigger:AnimElem(luaPlayer) == 5) or (trigger:AnimElem(luaPlayer) == 8))

						if trigger1 then

								local explod = trigger:CreateExplod(luaPlayer)

								explod.anim = 18102

								explod.ID = 18100

								explod.pos_x = -85

								explod.pos_y = -10

								explod.postype = ExplodPosType.p1

								explod.removetime = -2

								explod.sprpriority = 4

								explod.removeongethit = 1

								explod.ignorehitpause = 1

								explod.isChangeStateRemove = false

								explod.IsUseParentUpdate = false

								explod:Apply()


						end

				end


		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end


end

function Iori_ROTD:initCmd_QianShan(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("前闪")
		cmd.time = 1
		cmd:AttachKeyCommands("")

--------------------------- 前闪 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("前闪")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "900"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "前闪"))
								and (trigger:Ctrl(luaPlayer) == 1)
						return triggle1
				end

--------------------------- register StateDef 900 ---------------------------
		local id = luaCfg:CreateStateDef("900")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.N

		def.MoveType = Mugen.Cns_MoveType.I

		def.Juggle = 0

		def.PowerAdd = 0

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 0

		def.Animate = 900

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

						if trigger1 then

								trigger:VelSet(luaPlayer, 5 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 8)

						if trigger1 then

								trigger:VelSet(luaPlayer, 0 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:Turn(luaPlayer)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end


end

function Iori_ROTD:initCmd_HouShan(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("后闪")
		cmd.time = 1
		cmd:AttachKeyCommands("")

--------------------------- 后闪 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("后闪")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "910"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "后闪"))
								and (trigger:Ctrl(luaPlayer) == 1)
						return triggle1
				end

--------------------------- register StateDef 910 ---------------------------
		local id = luaCfg:CreateStateDef("910")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.N

		def.MoveType = Mugen.Cns_MoveType.I

		def.Juggle = 0

		def.PowerAdd = 0

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 0

		def.Animate = 910

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 4)

						if trigger1 then

								trigger:VelSet(luaPlayer, -5.5 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 10)

						if trigger1 then

								trigger:VelSet(luaPlayer, 0 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end


end

function Iori_ROTD:initCmd_TiaoXing(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("挑衅")
		cmd.time = 1
		cmd:AttachKeyCommands("")

--------------------------- 挑衅 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("挑衅")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "160"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "挑衅"))
								and (trigger:Ctrl(luaPlayer) == 1)
						return triggle1
				end

--------------------------- register StateDef 160 ---------------------------
		local id = luaCfg:CreateStateDef("160")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.S

		def.MoveType = Mugen.Cns_MoveType.I

		def.Juggle = 0

		def.PowerAdd = 0

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 2

		def.Animate = 160

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 20, 0)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						trigger:CtrlSet(luaPlayer, 1)

				end



end

function Iori_ROTD:initCmd_ZhongShou(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("重手")
		cmd.time = 1
		cmd:AttachKeyCommands("")

--------------------------- 重手 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("重手")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "210"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "重手"))
								and (trigger:Ctrl(luaPlayer) == 1)
						return triggle1
				end

--------------------------- register StateDef 210 ---------------------------
		local id = luaCfg:CreateStateDef("210")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.S

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 4

		def.PowerAdd = 65

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 2

		def.Animate = 210

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 3)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 50, 1)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 3)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 3, 1)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end


end

function Iori_ROTD:initCmd_QinTui(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("轻腿")
		cmd.time = 1
		cmd:AttachKeyCommands("")

--------------------------- 轻腿 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("轻腿")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "220"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "轻腿"))
						return triggle1
				end

--------------------------- register StateDef 220 ---------------------------
		local id = luaCfg:CreateStateDef("220")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.S

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 4

		def.PowerAdd = 22

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 2

		def.Animate = 220

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 50, 0)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 3, 2)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimTime(luaPlayer) == 0)

						if trigger1 then

								trigger:PlayStandCns(luaPlayer)

								trigger:CtrlSet(luaPlayer, 1)


						end

				end


end

function Iori_ROTD:initCmd_KuiHua(luaCfg)

--------------------------- register KeyCmd ---------------------------
		local cmd = luaCfg:CreateCmd("葵花")
		cmd.time = 1
		cmd:AttachKeyCommands("")

--------------------------- 葵花 ---------------------------
		local aiCmd = luaCfg:CreateAICmd("葵花")
		aiCmd.type = Mugen.AI_Type.ChangeState
		aiCmd.value = "1200"
		aiCmd.OnTriggerEvent =
				function (luaPlayer, aiName)
						local triggle1 = (trigger:Command(luaPlayer, "葵花"))
								and (trigger:Ctrl(luaPlayer) == 1)
						return triggle1
				end

--------------------------- register StateDef 1200 ---------------------------
		local id = luaCfg:CreateStateDef("1200")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.N

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 2

		def.PowerAdd = 65

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 3

		def.Animate = 1200

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 3)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 62, 0)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 3)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 3, 3)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 3)

						if trigger1 then

								trigger:PosAdd(luaPlayer, 16, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 1)

						if trigger1 then

								trigger:VelSet(luaPlayer, 4 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 5)

						if trigger1 then

								trigger:VelSet(luaPlayer, 0 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) >= 1)

						if trigger1 then

								trigger:VelMul(luaPlayer, nil, 0.95)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 6)

						if trigger1 then

								trigger:PlayCnsByName(luaPlayer, 1210, false)

						end

				end


--------------------------- register StateDef 1210 ---------------------------
		local id = luaCfg:CreateStateDef("1210")

		local def = luaCfg:GetStateDef(id)

		def.Type = Mugen.Cns_Type.S

		def.PhysicsType = Mugen.Cns_PhysicsType.N

		def.MoveType = Mugen.Cns_MoveType.A

		def.Juggle = 2

		def.PowerAdd = 65

		def.Velset_x = 0

		def.Velset_y = 0

		def.Ctrl = 0

		def.Sprpriority = 3

		def.Animate = 1210

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 4)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 62, 1)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 4)

						if trigger1 then

								trigger:PlaySnd(luaPlayer, 3, 3)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 1)

						if trigger1 then

								trigger:VelSet(luaPlayer, 5 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 5)

						if trigger1 then

								trigger:VelSet(luaPlayer, 0 * VelSetPer, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) >= 1)

						if trigger1 then

								trigger:VelMul(luaPlayer, 0.95, nil)

						end

				end

		local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

		state.OnTriggerEvent = 

				function (luaPlayer, state)

						local trigger1 = (trigger:AnimElem(luaPlayer) == 6)

						if trigger1 then

								trigger:PlayCnsByName(luaPlayer, 1220, false)

						end

				end


end

function Iori_ROTD:_initCmds()
	local luaCfg = trigger:GetLuaCnsCfg("Iori-ROTD")
	if luaCfg == nil then
		return
	end
	
	-- 普攻
	--self:_initCmd_a(luaCfg)
	-- 奔跑
	self:_initCmd_FF(luaCfg)
	--禁千弐百十壱式・八稚女
	--self:_initCmd_SuperCode1(luaCfg)

	self:initCmd_QiangZhuang(luaCfg)
	self:initCmd_ZhuaRen(luaCfg)
	self:initCmd_QianShan(luaCfg)
	self:initCmd_HouShan(luaCfg)
	self:initCmd_TiaoXing(luaCfg)
	self:initCmd_ZhongShou(luaCfg)
	self:initCmd_QinTui(luaCfg)
	self:initCmd_KuiHua(luaCfg)
	
---------------------Run Back ------------------------------
	local cmd = luaCfg:CreateCmd("BB")
	cmd.time = 10
	cmd:AttachKeyCommands("B, B")
	
	local aiCmd = luaCfg:CreateAICmd("Back Step")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "105"
	aiCmd.OnTriggerEvent = 
		function (luaPlayer, aiName)
			local trigger1 = trigger:Command(luaPlayer, "BB") and 
								trigger:Statetype(luaPlayer) == Mugen.Cns_Type.S and
								trigger:CanCtrl(luaPlayer)
			return trigger1
		end
-- Build By Iori-ROTD-N.cns.txt
----- 105
	local id = luaCfg:CreateStateDef("105")
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.Ctrl = 0
	def.Animate = 105
	def.Velset_x = 0
	def.Velset_y = 0
	
	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
	  function (luaPlayer, state)
	    local trigger1 = trigger:AnimTime(luaPlayer) == 0
		if trigger1 then
			debugStartTime = os.time()
		  	trigger:PlayCnsByName(luaPlayer, "106")
		end
	  end
----- 106 
	id = luaCfg:CreateStateDef("106")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.A
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.Ctrl = 0
	def.Animate = 106
	
-- [State 106,PlaySnd]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
	  function (luaPlayer, state)
		local tt = trigger:Time(luaPlayer)
	    local trigger1 = tt == 0
		if trigger1 then
			print(os.time())
			print(string.format("State 106,PlaySnd: %d[%d]%d - %d", tt, os.time() - debugStartTime, os.time(), debugStartTime))
			debugStartTime = os.time()
			trigger:PlaySnd(luaPlayer, 1, 1)
		end
	  end
-- [State 106, VelSet]
	 state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	 state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			local trigger1 = tt == 3
			if trigger1 then
				print(os.time())
				print(string.format("State 106, VelSet: %d[%d]%d - %d", tt, os.time() - debugStartTime, os.time(), debugStartTime))
				debugStartTime = os.time()
				trigger:VelSet(luaPlayer, -8, -4)
			end
		end
-- [State 106, VelAdd]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			local posX = trigger:PosY(luaPlayer)
			local trigger1 = tt >= 5 and posX < -3
			if trigger1 then
				print(string.format("State 106, VelAdd: %d, %d[%d]", tt, posX, os.time() - debugStartTime))
				debugStartTime = os.time()
				trigger:VelAdd(luaPlayer, 0.43, 0.55)
			end
		end
-- [State 106, VelSet]
-- [State 106, PosSet]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			local posY = trigger:PosY(luaPlayer)
			local trigger1 = tt > 10 and posY >= -5
			if trigger1 then
				print(string.format("State 106, VelSet: %d, %d[%d]", tt, posY, os.time() - debugStartTime))
				print(string.format("State 106, PosSet: %d, %d[%d]", tt, posY, os.time() - debugStartTime))
				debugStartTime = os.time()
				trigger:VelSet(luaPlayer, 0, 0)
				trigger:PosSet(luaPlayer, nil , 0)
			end
		end
-- [State 106, ChangeState]
   state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
   state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			local posY = trigger:PosY(luaPlayer)
			local trigger1 = tt > 10 and posY  == 0
			if trigger1 then
				print(string.format("State 106, ChangeState: %d, %d[%d]", tt, posY, os.time() - debugStartTime))
				debugStartTime = os.time()
				trigger:PlayCnsByName(luaPlayer, "107")
			end
		end
----- 107
	id = luaCfg:CreateStateDef("107")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.Ctrl = 0
	def.Animate = 107
-- [State 107, PlaySnd]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local frame = trigger:Time(luaPlayer)
			local trigger1 = frame == 1
			if trigger1 then
				print(string.format("State 107, PlaySnd: %d[%d]", frame, os.time() - debugStartTime))
				debugStartTime = os.time()
				trigger:PlaySnd(luaPlayer, 0, 1)
			end
		end
-- [State 107, VelSet]
-- [State 107, PosSet]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local frame = trigger:Time(luaPlayer)
			if frame == 0 then
				print(string.format("State 107, VelSet && State 107, PosSet: %d[%d]", frame, os.time() - debugStartTime))
				debugStartTime = os.time()
				trigger:VelSet(luaPlayer, 0, 0)
				trigger:PosSet(luaPlayer, nil, 0)
			end
		end
-- [State 107, ChangeState]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local frame = trigger:AnimTime(luaPlayer)
			if frame == 0 then
				print(string.format("State 107, ChangeState: %d[%d]", frame, os.time() - debugStartTime))
				debugStartTime = os.time()
				trigger:PlayCnsByName(luaPlayer, "0", true)
				trigger:SetCtrl(luaPlayer, 1)
			end
		end
-------------------------------- 气功波 -----------------------
	cmd = luaCfg:CreateCmd("气功波")
	cmd.time = 1
	cmd:AttachKeyCommands("F,F,F,F")

	aiCmd = luaCfg:CreateAICmd("气功波")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "1000"
	aiCmd.OnTriggerEvent =
		function (luaPlayer, aiName)
			local trigger1 = trigger:Command(luaPlayer, "气功波") and trigger:Statetype(luaPlayer) ~= Mugen.Cns_Type.A and trigger:CanCtrl(luaPlayer)
			return trigger1
		end
-- Statedef 1000
	id = luaCfg:CreateStateDef("1000")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.Juggle = 0
	def.PowerAdd = 65
	def.Ctrl = 0
	def.Velset_x = 0
	def.Velset_y = 0
	def.Animate = 1000
	def.Sprpriority = 3
-- State 1000, PlaySnd
-- State 1000, PlaySnd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 4 then
				trigger:PlaySnd(luaPlayer, 60, 0)
				trigger:PlaySnd(luaPlayer, 60, 1)	
			end
		end
--State 1000, Explod
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 3 then
				local explod = trigger:CreateExplod(luaPlayer)
				explod.anim = 20000
				explod.ID = 20000
				explod.pos_x = 0
				explod.pos_y = 0
				explod.postype = ExplodPosType.p1
				explod.bindtime = -1
				explod.removetime = -2
				explod.sprpriority = 4
				explod.removeongethit = 1
				explod.ignorehitpause = 1

				explod:Apply()
			end
		end
-- State 1000, Projectile]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:AnimElem(luaPlayer) == 4
			if trigger1 then
				local proj = trigger:CreateProj(luaPlayer)
				proj.projanim = 20005
				proj.projhitanim = 90000
				proj.projremanim = 90000
				proj.projcancelanim = 90000
				proj.projshadow = -1
				proj.offset_x = 30
				proj.offset_y = 0
				proj.projpriority = 1
				proj.projsprpriority = 4
				proj.velocity_x = 4.5

				-- 自己写间隔
				proj.projremovetime = 100

				proj:Apply()
			end
		end
-- State 1000, ChangeState
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:AnimTime(luaPlayer) == 0
			if trigger1 then
				trigger:SetCtrl(luaPlayer, 1)
				trigger:PlayStandCns(luaPlayer)
			end
		end
------------------------ 升龙 ---------------------
	cmd = luaCfg:CreateCmd("升龙")
	cmd.time = 10
	cmd:AttachKeyCommands("")

	aiCmd = luaCfg:CreateAICmd("升龙")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "1100"
	aiCmd.OnTriggerEvent = 
		function (luaPlayer, aiName)
			local trigger1 = trigger:Command(luaPlayer, "升龙") and trigger:Statetype(luaPlayer) ~= Mugen.Cns_Type.A and trigger:CanCtrl(luaPlayer)
			return trigger1
		end

	id = luaCfg:CreateStateDef("1100")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.Juggle = 4
	def.PowerAdd = 30
	def.Velset_x = 0
	def.Velset_y = 0
	def.Animate = 1100
	def.Ctrl = 0
	def.Sprpriority = 3
-- State 1100, PlaySnd
-- State 1100, PlaySnd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 4 then
				trigger:PlaySnd(luaPlayer, 61, 0)
			elseif animElem == 6 then
				trigger:PlaySnd(luaPlayer, 61, 1)
			end
		end
-- State 1100, Explod
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 6 then
				local explod = trigger:CreateExplod(luaPlayer)
				explod.anim = 21000
				explod.ID = 21000
				explod.pos_x = 0
				explod.pos_y = 0
				explod.postype = ExplodPosType.p1
				explod.bindtime = 1 * bindTimePer
				explod.removetime = -2
				explod.sprpriority = 4
				explod.removeongethit = 0
				explod.ignorehitpause = 1
				explod.isChangeStateRemove = false
				explod.IsUseParentUpdate = false

				explod:Apply()

				trigger:Persistent(luaPlayer, state, true)
			end
		end
-- State 1100, Explod
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 2 then
				local explod = trigger:CreateExplod(luaPlayer)
				explod.anim = 21001
				explod.ID = 21001
				explod.pos_x = 0
				explod.pos_y = 0
				explod.postype = ExplodPosType.p1
				explod.bindtime = 1 * bindTimePer
				explod.removetime = -2
				explod.sprpriority = 4
				explod.removeongethit = 0
				explod.ignorehitpause = 1
				explod.IsUseParentUpdate = false

				explod:Apply()

				trigger:Persistent(luaPlayer, state, true)
			end
		end

	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 10 then
				local explod = trigger:CreateExplod(luaPlayer)
				explod.anim = 21001
				explod.ID = 21001
				explod.pos_x = 0
				explod.pos_y = 0
				explod.postype = ExplodPosType.p1
				explod.bindtime = 1 * bindTimePer
				explod.removetime = -2
				explod.sprpriority = 4
				explod.removeongethit = 0
				explod.ignorehitpause = 1
				explod.IsUseParentUpdate = false

				explod:Apply()

				trigger:Persistent(luaPlayer, state, true)
			end
		end
-- State 1100, PosAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 5 then
				trigger:PosAdd(luaPlayer, 16, nil)
			end
		end
-- State 1100, PosAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 6 then
				trigger:PosAdd(luaPlayer, 24, nil)
			end
		end
-- State 1100, VelSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 6 then
				trigger:VelSet(luaPlayer, nil, -5 * VelSetPer)
			end
		end
-- State 1100, VelAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			if trigger:AnimElem(luaPlayer) >= 6 then
				trigger:VelAdd(luaPlayer, nil, 0.42)
			end
		end
-- State 1100, StateTypeSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 6 then
				trigger:StateTypeSet(luaPlayer, Mugen.Cns_Type.A)
				trigger:PhysicsTypeSet(luaPlayer, Mugen.Cns_PhysicsType.N)
			end
		end
-- State 1100, VelSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:VelY(luaPlayer) > 0 and trigger:PosY(luaPlayer) >= 0
			if trigger1 then
				trigger:PlayCnsByName(luaPlayer, "1110")
			end
		end
-- StateDef 1110
	id = luaCfg:CreateStateDef("1110")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.I
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Animate = 1110
	def.Ctrl = 0
	def.Velset_x = 0
	def.Velset_y = 0
	def.Sprpriority = 2
-- State 1110, PlaySnd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 1 then
				trigger:PlaySnd(luaPlayer, 0, 1)
			end
		end
-- State 1110, PosSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			if tt == 0 then
				trigger:PosSet(luaPlayer, nil, 0)
			end
		end
-- State 1110, Changestate
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:AnimTime(luaPlayer)
			if tt == 0 then
				trigger:CtrlSet(luaPlayer, 1)
				trigger:PlayStandCns(luaPlayer)
			end
		end
------------------------ 风车轮 -------------------------
	cmd = luaCfg:CreateCmd("风车轮")
	cmd.time = 10
	cmd:AttachKeyCommands("")

	aiCmd = luaCfg:CreateAICmd("风车轮")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "1500"
	aiCmd.OnTriggerEvent = 
		function (luaPlayer, aiName)
			local trigger1 = trigger:Command(luaPlayer, "风车轮") and trigger:Statetype(luaPlayer) ~= Mugen.Cns_Type.A and trigger:CanCtrl(luaPlayer)
			return trigger1
		end

	id = luaCfg:CreateStateDef("1500")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.PowerAdd = 30
	def.Velset_x = 0
	def.Velset_y = 0
	def.Animate = 1500
	def.Ctrl = 0
	def.Sprpriority = 3
-- State 1700, PlaySnd
-- State 1700, PlaySnd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 3 then
				trigger:PlaySnd(luaPlayer, 65, 0)
			elseif animElem == 5 then
				trigger:PlaySnd(luaPlayer, 65, 1)
			end
		end
-- State 1700, PosAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 2 or animElem == 3 or animElem == 4 then
				trigger:PosAdd(luaPlayer, 16, nil)
			end
		end
-- State 1700, PosAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 5 or animElem == 6 or animElem == 7 or animElem == 8 or animElem == 9 then
				trigger:PosAdd(luaPlayer, 8, nil)
			end
		end
-- State 1700, VelSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 6 then
				trigger:VelSet(luaPlayer, 4 * VelSetPer, -3.5 * VelSetPer)
			end
		end
-- State 1700, VelAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem >= 6 then
				trigger:VelAdd(luaPlayer, nil, 0.56)
			end
		end
-- State 1700, StateTypeSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 6 then
				trigger:StateTypeSet(luaPlayer, Mugen.Cns_Type.A)
				trigger:PhysicsTypeSet(luaPlayer, Mugen.Cns_PhysicsType.N)
			end
		end
-- State 1700, ChangeState
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:VelY(luaPlayer) > 0 and trigger:PosY(luaPlayer) >= 0
			if trigger1 then
				trigger:PlayCnsByName(luaPlayer, "1710")
			end
		end
--- StateDef 1710
	id = luaCfg:CreateStateDef("1710")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Juggle = 4
	def.Animate = 1510
	def.Ctrl = 0
	def.Velset_x = 0
	def.Velset_y = 0
	def.Sprpriority = 2
-- State 1710, PlaySnd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 1 then
				trigger:PlaySnd(luaPlayer, 61, 1)
			end
		end
-- State 1710, Explod
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 1 then
				local explod = trigger:CreateExplod(luaPlayer)
				explod.anim = 25100
				explod.ID = 25100
				explod.pos_x = 0
				explod.pos_y = 0
				explod.postype = ExplodPosType.p1
				explod.bindtime = 1 * bindTimePer
				explod.removetime = -2
				explod.sprpriority = 4
				explod.removeongethit = 0
				explod.ignorehitpause = 1

				-- 切换状态是否删除对象，默认为TRUE
				explod.isChangeStateRemove = false
				explod.IsUseParentUpdate = false

				explod:Apply()

				trigger:Persistent(luaPlayer, state, true)
			end
		end
-- State 1710, PosSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			if tt == 0 then
				trigger:PosSet(luaPlayer, nil, 0)
			end
		end
-- State 1710, PosAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 2 or animElem == 3 then
				trigger:PosAdd(luaPlayer, 8, nil)
			end
		end
-- State 1710, Changestate
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:AnimTime(luaPlayer)
			if tt == 0 then
				trigger:CtrlSet(luaPlayer, 1)
				trigger:PlayStandCns(luaPlayer)
			end
		end

-------------------- 真・大风车 ----------------
	cmd = luaCfg:CreateCmd("真・大风车")
	cmd.time = 10
	cmd:AttachKeyCommands("")

	aiCmd = luaCfg:CreateAICmd("真・大风车")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "2100"
	aiCmd.OnTriggerEvent = 
		function (luaPlayer, aiName)
			local trigger1 = trigger:Command(luaPlayer, "真・大风车") and trigger:Statetype(luaPlayer) ~= Mugen.Cns_Type.A and trigger:CanCtrl(luaPlayer)
			if trigger1 then
				print(">>>>>>>>>>>真・大风车")
			end
			return trigger1
		end

	id = luaCfg:CreateStateDef("2100")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.Juggle = 4
	def.PowerAdd = 0
	def.Ctrl = 0
	def.Velset_x = 0
	def.Velset_y = 0
	def.Animate = 2100
	def.Sprpriority = 3

-- State 2300, PlaySnd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 1 then
				print("State 2300, PlaySnd: 5, 1")
				trigger:PlaySnd(luaPlayer, 5, 1)
			end
		end

-- State 2300, Explod
--[[
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			if tt == 2 then
				print("State 2300, Explod: 6060")
				local explod = trigger:CreateExplod(luaPlayer)
				explod.anim = 6060
				explod.pos_x = 10
				explod.pos_y = -65
				explod.postype = ExplodPosType.p1
				explod.sprpriority = 0
				explod.bindtime = 1 * bindTimePer
				explod.removetime = -2

				-- 切换状态是否删除对象，默认为TRUE
				explod.isChangeStateRemove = false
				explod.IsUseParentUpdate = false

				explod:Apply()
			end
		end	


-- State 2300, Explod
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			if tt == 2 then
				print("State 2300, Explod: 6061")
				local explod = trigger:CreateExplod(luaPlayer)
				explod.anim = 6061
				explod.pos_x = 10
				explod.pos_y = -65
				explod.postype = ExplodPosType.p1
				explod.sprpriority = 3
				explod.bindtime = 1 * bindTimePer
				explod.removetime = -2

				-- 切换状态是否删除对象，默认为TRUE
				explod.isChangeStateRemove = false
				explod.IsUseParentUpdate = false

				explod:Apply()
			end
		end
	--]]
-- State 2300, PlaySnd
-- State 2300, PlaySnd
-- State 2300, PlaySnd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 1 then
				print("State 2300, PlaySnd: 67, 0")
				trigger:PlaySnd(luaPlayer, 67, 0)
			elseif animElem == 5 then
				print("State 2300, PlaySnd: 65, 1")
				trigger:PlaySnd(luaPlayer, 65, 1)
			elseif animElem == 9 then
				print("State 2300, PlaySnd: 61, 1")
				trigger:PlaySnd(luaPlayer, 61, 1)
			end
		end
-- State 2300, PosAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 2 or animElem == 3 or animElem == 4 then
				print("State 2300, PosAdd: 16, nil")
				trigger:PosAdd(luaPlayer, 16, nil)
			end
		end
-- State 2300, PosAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 5 or animElem == 6 or animElem == 7 or animElem == 8 or animElem == 9 then
				print("State 2300, PosAdd: 8, nil")
				trigger:PosAdd(luaPlayer, 8, nil)
			end
		end
-- State 2300, VelSet
-- State 2300, VelAdd
-- State 2300, StateTypeSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 6 then
				print("State 2300, VelSet: 5.5, -4")
				trigger:VelSet(luaPlayer, 5.5 * VelSetPer, -4 * VelSetPer)
				print("State 2300, StateTypeSet: Mugen.Cns_Type.A, Mugen.Cns_PhysicsType.N")
				trigger:StateTypeSet(luaPlayer, Mugen.Cns_Type.A)
				trigger:PhysicsTypeSet(luaPlayer, Mugen.Cns_PhysicsType.N)
			elseif animElem >= 6 then
				print("State 2300, VelAdd: nil, 0.56")
				trigger:VelAdd(luaPlayer, nil, 0.56)
			end
		end
-- State 2300, VelSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local trigger1 = trigger:VelY(luaPlayer) > 0 and trigger:PosY(luaPlayer) >= 0
			if trigger1 then
				print("State 2300, VelSet: 2110")
				trigger:PlayCnsByName(luaPlayer, "2110")
			end
		end
--- Statedef 2110
	id = luaCfg:CreateStateDef("2110")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Animate = 2110
	def.Ctrl = 0
	def.Velset_x = 0
	def.Velset_y = 0
	def.Sprpriority = 2
-- State 2110, PlaySnd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 1 then
				print("State 2110, PlaySnd: 61, 1")
				trigger:PlaySnd(luaPlayer, 61, 1)
			end
		end
-- State 2110, Explod
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 1 then
				print("State 2110, Explod: 25100")
				local explod = trigger:CreateExplod(luaPlayer)
				explod.anim = 25100
				explod.ID = 25100
				explod.pos_x = 0
				explod.pos_y = 0
				explod.postype = ExplodPosType.p1
				explod.sprpriority = 4
				explod.bindtime = 1 * bindTimePer
				explod.removetime = -2
				explod.removeongethit = 0
				explod.ignorehitpause = 1

				-- 切换状态是否删除对象，默认为TRUE
				explod.isChangeStateRemove = false
				explod.IsUseParentUpdate = false

				trigger:Persistent(luaPlayer, state, true)

				explod:Apply()
			end
		end
-- State 2110, PosSet
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			if tt == 0 then
				print("State 2110, PosSet: nil, 0")
				trigger:PosSet(luaPlayer, nil, 0)
			end
		end
-- State 2110, PosAdd
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local animElem = trigger:AnimElem(luaPlayer)
			if animElem == 2 or animElem == 3 then
				print("State 2110, PosAdd: 8")
				trigger:PosAdd(luaPlayer, 8)
			end
		end
-- State 2110, Changestate
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			local tt = trigger:AnimTime(luaPlayer)
			if tt == 0 then
				print("State 2110, Changestate: 0")
				trigger:CtrlSet(luaPlayer, 1)
				trigger:PlayStandCns(luaPlayer)
			end
		end

end

--======================================================================================

--== 普攻 a

function Iori_ROTD:OnAICmd_a_or_b(aiName)
	local triggerall = trigger:Command(self, "a") or trigger:Command(self, "b")
	if not triggerall then
		return false
	end
	local trigger1 = trigger:Stateno(self) == 100
	local ret = triggerall and trigger1
	return ret 
end

function Iori_ROTD:OnAICmd_a_and_holdfwd(cmdName)
	local triggerall = trigger:Command(self, "a") and trigger:Command(self, "holdfwd")
	local trigger1 = trigger:Statetype(self) ~= Mugen.Cns_Type.A and trigger:CanCtrl(self)
	local ret = triggerall and trigger1
	return ret
end

function Iori_ROTD:_initCmd_a(luaCfg)
	local cmd = luaCfg:CreateCmd("a", "")
	cmd.time = 1
	cmd:AttachKeyCommands("a")

	local aiCmd = luaCfg:CreateAICmd("a or b", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "260"
	aiCmd.OnTriggerEvent = self.OnAICmd_a_or_b

	aiCmd = luaCfg:CreateAICmd("a or holdfwd", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "320"
	aiCmd.OnTriggerEvent = self.OnAICmd_a_and_holdfwd
end

function Iori_ROTD:_initCmd_FF(luaCfg)
	local cmd = luaCfg:CreateCmd("FF", "")
	cmd.time = 10
	cmd:AttachKeyCommands("F,F")
end

--==禁千弐百十壱式・八稚女

function Iori_ROTD:_initStateDef_2000(luaCfg)
	local id = luaCfg:CreateStateDef("2000")
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.Juggle = 4
	def.PowerAdd = 0
	def.Animate = 2000
	def.Ctrl = 0
	def.Sprpriority = 3
	def.Velset_x = 0
	def.Velset_y = 0
	
	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem, Mugen.CnsStateType.none)
	state.OnTriggerEvent = self._On_2000_PlaySnd

	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem, Mugen.CnsStateType.none)
	state.OnTriggerEvent = self._On_2000_Vel

	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime, Mugen.CnsStateType.none)
	state.OnTriggerEvent = self._On_2000_AnimTime

	trigger:RegisterAnimationState(2010)
end

function Iori_ROTD:_On_2000_AnimTime(state)

	local animTime = trigger:AnimTime(self)
	--print(animTime)
	if animTime == 0 then
		trigger:SetCtrl(self, 1)
		trigger:ChangeState(self, 0, false)
		--trigger:ChangeState(self, 2010, false)
	end
end

function Iori_ROTD:_On_2000_PlaySnd(state)
	local curFrame = trigger:AnimElem(self)
	--print(curFrame)
	if curFrame == 1 then
		trigger:Do_PlaySnd(self, 5, 0)
	elseif curFrame == 2 then
		trigger:Do_PlaySnd(self, 66, 0)
	elseif curFrame == 6 then
		trigger:Do_PlaySnd(self, 66, 2)
	end
end

function Iori_ROTD:_On_2000_Vel(state)
	local curFrame = trigger:AnimElem(self)
	if curFrame == 6 then
		trigger:VelSet(self, 8.6, 0)
	elseif curFrame == 12 then
		trigger:VelSet(self, 0, 0)
	elseif curFrame == 10 then
		trigger:VelMul(self, 0.9, 0)
	end
end

function Iori_ROTD:OnAICmd_SuperCode1(cmdName)
	--print(self.PlayerDisplay)
	local triggerAll = (cmdName == "禁千弐百十壱式・八稚女") and (trigger:Statetype(self) ~= Mugen.Cns_Type.A)
	if not triggerAll then
		return false
	end
	triggerAll = triggerAll and (trigger:Power(self) >= 1000)
	if not triggerAll then
		return false
	end
	local trigger1 = trigger:CanCtrl(self)
	local ret = triggerAll and trigger1
	--print(triggerAll)
	--print(trigger1)
	return ret
end

function Iori_ROTD:_initCmd_SuperCode1(luaCfg)
	local cmd = luaCfg:CreateCmd("禁千弐百十壱式・八稚女", "禁千弐百十壱式・八稚女")
	cmd.time = 30
	cmd:AttachKeyCommands("~D, DF, F, DF, D, DB, x")
	
	-- 创建状态
	local aiCmd = luaCfg:CreateAICmd("禁千弐百十壱式・八稚女")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "2000"
	aiCmd.OnTriggerEvent = self.OnAICmd_SuperCode1
end

--==

setmetatable(Iori_ROTD, {__call = Iori_ROTD.new})

return Iori_ROTD