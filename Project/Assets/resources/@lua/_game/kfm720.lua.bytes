local trigger = require("trigger")
local _InitCommonCns = require("commonCns")

local setmetatable = setmetatable
local GlobaConfigMgr = MonoSingleton_GlobalConfigMgr.GetInstance()

local kfm720 = {}
kfm720.__index = kfm720

function kfm720:new()
   -- 静态数据
   if self._isInit == nil then
		self._isInit = true
		self:_initData()
		self:_initSize()
		self:_initMovement()
		self:_initStateDefs()
		self:_initCmds()
    end
   -- 动态数据
   local t = {PlayerDisplay = nil}
   return setmetatable(t, kfm720)
end

--====================外部调用接口==============================

function kfm720:OnInit(playerDisplay)
	self.PlayerDisplay = playerDisplay;
	trigger:Help_InitLuaPlayer(self, self)
	_InitCommonCns(self)
end

function kfm720:OnDestroy()
  self.PlayerDisplay = nil
end

function kfm720:OnGetAICommandName(cmdName)
	
end

--===========================================================

function kfm720:_initData()
  if self.Data ~= nil then
	return
  end
  self.Data = {};
  
  self.Data.life = 1000
  self.Data.attack = 100
  self.Data.defence = 100
  
  self.Data.fall = {}
  self.Data.fall.defence_up = 50
  
  self.Data.liedown = {}
  self.Data.liedown.time = 60
  
  self.Data.airjuggle = 15
  self.Data.sparkno = 2
  
  self.Data.guard = {}
  self.Data.guard.sparkno = 40
  
  self.Data.KO = {}
  self.Data.KO.echo = 0
  
  self.Data.volume = 0
  self.Data.IntPersistIndex = 60
  self.Data.FloatPersistIndex = 40
end

function kfm720:_initSize()
  if self.Size ~= nil then
	return
  end
  self.Size = {}
  self.Size.xscale = 1
  self.Size.yscale = 1
end

function kfm720:_initMovement()
	if self.Movement ~= nil then
		return
	end
	
	self.movement = {}
	self.movement.yaccel = 1.76

	self.velocity = {}
	self.velocity.run = {}
	self.velocity.run.fwd = Vector2.New(18.4, 0)
	self.velocity.run.back = Vector2.New(-18,-15.2)
end

--=====================================创建StateDef===================================
function kfm720:_initStateDefs()
	local luaCfg = GlobaConfigMgr:GetLuaCnsCfg("kfm720")
	if luaCfg == nil then
		return
	end
	
	-- 创建各种状态
	self:_initStateDef_200(luaCfg)
	self:_initStateDef_3000(luaCfg)
end

function kfm720:_initStateDef_200(luaCfg)
	local id = trigger:Help_CreateStateDef(luaCfg, "200")
	local def = trigger:Help_GetStateDef(luaCfg, id)
	--Def注册State
end

function kfm720:_initStateDef_3000(luaCfg)
	local id = trigger:Help_CreateStateDef(luaCfg, "3000")
	local def = trigger:Help_GetStateDef(luaCfg, id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Juggle = 4
	def.Animate = 3000
	def.Ctrl = 0
	def.Sprpriority = 2
	def.Velset_x = 0
	def.Velset_y = 0
end
--======================================================================================

function kfm720:_initCommands(luaCfg)
	local cmd = luaCfg:CreateCmd("a")
	cmd.time = 1
	cmd:AttachKeyCommands("a")
	
	cmd = luaCfg:CreateCmd("b")
	cmd.time = 1
	cmd:AttachKeyCommands("b")
	
	cmd = luaCfg:CreateCmd("c")
	cmd.time = 1
	cmd:AttachKeyCommands("c")
	
	cmd = luaCfg:CreateCmd("x")
	cmd.time = 1
	cmd:AttachKeyCommands("x")
	
	cmd = luaCfg:CreateCmd("y")
	cmd.time = 1
	cmd:AttachKeyCommands("y")
	
	cmd = luaCfg:CreateCmd("z")
	cmd.time = 1
	cmd:AttachKeyCommands("z")
	
	cmd = luaCfg:CreateCmd("holdfwd")
	cmd.time = 1
	cmd:AttachKeyCommands("/$F")
	
	cmd = luaCfg:CreateCmd("holdback")
	cmd.time = 1
	cmd:AttachKeyCommands("/$B")
	
	cmd = luaCfg:CreateCmd("holdup")
	cmd.time = 1
	cmd:AttachKeyCommands("/$U")
	
	cmd = luaCfg:CreateCmd("holddown")
	cmd.time = 1
	cmd:AttachKeyCommands("/$D")
	
	cmd = luaCfg:CreateCmd("start")
	cmd.time = 1
	cmd:AttachKeyCommands("s")
	
	cmd = luaCfg:CreateCmd("FF")
	cmd.time = 10
	cmd:AttachKeyCommands("F, F")
	
	cmd = luaCfg:CreateCmd("BB")
	cmd.time = 10
	cmd:AttachKeyCommands("B, B")
	
	cmd = luaCfg:CreateCmd("QCF_y")
	cmd:AttachKeyCommands("~D, DF, F, y")
end

function kfm720:On_Taunt(cmdName)
	local triggerall = trigger:Command(self, "start")
	local trigger1 = trigger:Statetype(self) ~= Mugen.Cns_Type.A and trigger:CanCtrl(self)
	local ret = triggerall and trigger1
	return ret
end

--[[
function kfm720:On_Run_Fwd(cmdName)
	local trigger1 = trigger:Command(self, "FF") and 
						trigger:Statetype(self) == Mugen.Cns_Type.S and 
						trigger:CanCtrl(self)
	
	return trigger1 
end
--]]

function kfm720:On_Run_Back(cmdName)
	local trigger1 = trigger:Command(self, "BB") and 
						trigger:Statetype(self) == Mugen.Cns_Type.S and 
						trigger:CanCtrl(self)
	return trigger1 
end

function kfm720:On_QCF_y(cmdName)
	local triggerall = trigger:Command(self, "QCF_y")
	-- 暂时忽略COMBO条件
	return triggerall
end

function kfm720:_initState_Default(luaCfg)
	local aiCmd = luaCfg:CreateAICmd("Taunt", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "195"
	aiCmd.OnTriggerEvent = self.On_Taunt
	--[[
	aiCmd = luaCfg:CreateAICmd("Run Fwd", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "100"
	aiCmd.AniLoop = true
	aiCmd.OnTriggerEvent = self.On_Run_Fwd
	--]]
	
	aiCmd = luaCfg:CreateAICmd("Run Back", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "105"
	aiCmd.AniLoop = false
	aiCmd.OnTriggerEvent = self.On_Run_Back
	
	aiCmd = luaCfg:CreateAICmd("Strong Kung Fu Palm", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "1010"
	aiCmd.AniLoop = false
	aiCmd.OnTriggerEvent = self.On_QCF_y
end

function kfm720:_initCmds()
	local luaCfg = trigger:GetLuaCnsCfg("kfm720")
	if luaCfg == nil then
		return
	end
	
	self:_initCommands(luaCfg)
	self:_initState_Default(luaCfg)
	self:_initStateDef(luaCfg)
end

function kfm720:_initStateDef(luaCfg)
--------------------- start -------------------
	local id = luaCfg:CreateStateDef("195")
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.Ctrl = 0
	def.Animate = 195
	def.Velset_x = 0
	def.Velset_y = 0
	def.MoveType = Mugen.Cns_MoveType.I
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Sprpriority = 2
	
	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime, Mugen.CnsStateType.none)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local aniTime = trigger:AnimTime(luaPlayer)
			if aniTime == 0 then
				trigger:PlayCnsByName(luaPlayer, "0", true)
			end
		end
---------------------- Strong Kung Fu Palm ------------------
	id = luaCfg:CreateStateDef("1010")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.MoveType = Mugen.Cns_MoveType.A
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Juggle = 4
	def.PowerAdd = 60
	def.Velset_x = 0
	def.Velset_y = 0
	def.Animate = 1010
	def.Ctrl = 0
	def.Sprpriority = 2
	-- [State 1010, 1]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
		  local t = trigger:Time(luaPlayer)
		  if t == 9 then
			trigger:PlaySnd(luaPlayer, 0, 3)
		  end
		end
	-- [State 1010, 2]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local frame = trigger:AnimElem(luaPlayer)
			if frame == 2 then
				trigger:PosAdd(luaPlayer, 80, 0)
			end
		end
	-- [State 1010, 3]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local frame = trigger:AnimElem(luaPlayer)
			if frame == 3 or frame == 13 then
				trigger:PosAdd(luaPlayer, 40, 0)
			end
		end
	-- [State 1010, 4]
	-- [State 1010, 5]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local frame = trigger:AnimElem(luaPlayer)
			if frame == 5 then
				trigger:PosAdd(luaPlayer, 20, 0)
				trigger:VelSet(luaPlayer, 16, 0)
			end
		end
	-- [State 1010, 8]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local aniTime = trigger:AnimTime(luaPlayer)
			if aniTime == 0 then
				trigger:SetCtrl(luaPlayer, 1)
				trigger:PlayCnsByName(luaPlayer, "0", true)
			end
		end
----------------- RUN_BACK --------------
	id = luaCfg:CreateStateDef("105")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.A
	def.MoveType = Mugen.Cns_MoveType.A
	def.Ctrl = 0
	def.Animate = 105
	def.Sprpriority = 1
-- [State 105, 1]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			--print(tt)
			if tt == 0 then
				print("State 105, 1")
				trigger:VelSet(luaPlayer, -18, -15.2)
			end
		end
-- [State 105, 2]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			if tt == 2 then
				print("State 105, 2")
				trigger:CtrlSet(luaPlayer, 1)
			end
		end
-- [State 105, 3]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:VelY(luaPlayer) > 0 and trigger:PosY(luaPlayer) >= 0
			if trigger1 then
				print("State 105, 3")
				trigger:PlayCnsByName(luaPlayer, "106")
			end
		end

	id = luaCfg:CreateStateDef("106")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Ctrl = 0
	def.Animate = 47
-- [State 106, 1]
-- [State 106, 2]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			print("106 Check")
			local trigger1 = trigger:Time(luaPlayer) == 0
			if trigger1 then
				trigger:VelSet(luaPlayer, nil, 0)
				trigger:PosSet(luaPlayer, nil, 0)
			end
		end
-- [State 106, 4]
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 7
			if trigger1 then
				trigger:SetCtrl(luaPlayer, 1)
				trigger:PlayCnsByName(luaPlayer, "0", true)
			end
		end
------------------------- Run Fwd -----------------
--[[
	id = luaCfg:CreateStateDef("100")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Animate = 100
	def.Sprpriority = 1
-- State 100, 1
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent =
		function (luaPlayer, state)
			trigger:VelSet(luaPlayer, 18.4, nil)
		end
--]]
end

setmetatable(kfm720, {__call = kfm720.new})
return kfm720