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

--创建StateDef
function Iori_ROTD:_initStateDefs()
	local luaCfg = trigger:GetLuaCnsCfg("Iori-ROTD")
	if luaCfg == nil then
		return
	end
	
	-- 创建各种状态
	self:_initStateDef_2000(luaCfg)
end

function Iori_ROTD:_initCmds()
	local luaCfg = trigger:GetLuaCnsCfg("Iori-ROTD")
	if luaCfg == nil then
		return
	end
	
	-- 普攻
	self:_initCmd_a(luaCfg)
	-- 奔跑
	self:_initCmd_FF(luaCfg)
	--禁千弐百十壱式・八稚女
	self:_initCmd_SuperCode1(luaCfg)
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

function Iori_ROTD:OnAICmd_Dash(cmdName)
	local isCmdOk = trigger:Command(self, "FF")
	local isStateType = trigger:Statetype(self) == Mugen.Cns_Type.S
	local canCtrl = trigger:CanCtrl(self)
	--print(canCtrl)
    local trigger1 = isCmdOk and isStateType and canCtrl
    return trigger1
end

function Iori_ROTD:_initCmd_FF(luaCfg)
	local cmd = luaCfg:CreateCmd("FF", "")
	cmd.time = 10
	cmd:AttachKeyCommands("F,F")
	
	local aiCmd = luaCfg:CreateAICmd("Dash", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "100"
	aiCmd.OnTriggerEvent = self.OnAICmd_Dash
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