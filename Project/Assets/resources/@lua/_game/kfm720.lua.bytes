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
end

function kfm720:On_Taunt(cmdName)
	local triggerall = trigger:Command(self, "start")
	local trigger1 = trigger:Statetype(self) ~= Mugen.Cns_Type.A and trigger:CanCtrl(self)
	local ret = triggerall and trigger1
	return ret
end

function kfm720:On_Run_Fwd(cmdName)
	local trigger1 = trigger:Command(self, "FF") and 
						trigger:Statetype(self) == Mugen.Cns_Type.S and 
						trigger:CanCtrl(self)
	
	return trigger1 
end

function kfm720:On_Run_Back(cmdName)
	local trigger1 = trigger:Command(self, "BB") and 
						trigger:Statetype(self) == Mugen.Cns_Type.S and 
						trigger:CanCtrl(self)
	
	return trigger1 
end

function kfm720:_initState_Default(luaCfg)
	local aiCmd = luaCfg:CreateAICmd("Taunt", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "195"
	aiCmd.OnTriggerEvent = self.On_Taunt
	
	aiCmd = luaCfg:CreateAICmd("Run Fwd", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "100"
	aiCmd.AniLoop = true
	aiCmd.OnTriggerEvent = self.On_Run_Fwd
	
	aiCmd = luaCfg:CreateAICmd("Run Back", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "105"
	aiCmd.AniLoop = false
	aiCmd.OnTriggerEvent = self.On_Run_Back
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
----------------------
end

setmetatable(kfm720, {__call = kfm720.new})
return kfm720