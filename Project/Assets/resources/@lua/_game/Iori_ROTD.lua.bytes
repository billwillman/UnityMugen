local trigger = require("trigger")

local setmetatable = setmetatable
local GlobaConfigMgr = MonoSingleton_GlobalConfigMgr.GetInstance()

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
   return setmetatable(t, Iori_ROTD)
end

--====================外部调用接口==============================

function Iori_ROTD:OnInit(playerDisplay)
	self.PlayerDisplay = playerDisplay;
	trigger:Help_InitLuaPlayer(self, self)
end

function Iori_ROTD:OnDestroy()
  self.PlayerDisplay = nil
end

function Iori_ROTD:OnGetAICommandName(cmdName)
	
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
	local luaCfg = GlobaConfigMgr:GetLuaCnsCfg("Iori-ROTD")
	if luaCfg == nil then
		return
	end
	
	-- 创建各种状态
	self:_initStateDef_2210(luaCfg)
end

function Iori_ROTD:_initStateDef_2210(luaCfg)
	
end

--======================================================================================

function Iori_ROTD:_initCmds()
	local luaCfg = GlobaConfigMgr:GetLuaCnsCfg("Iori-ROTD")
	if luaCfg == nil then
		return
	end
	
	self:_initCmd_禁千弐百十壱式・八稚女(luaCfg)
end

function Iori_ROTD:_initCmd_禁千弐百十壱式・八稚女(luaCfg)
	local cmd = luaCfg:CreateCmd("禁千弐百十壱式・八稚女", "禁千弐百十壱式・八稚女")
	cmd.time = 30
	cmd:AttachKeyCommands("~D, DF, F, DF, D, DB, x")
	
	-- 创建状态
end


setmetatable(Iori_ROTD, {__call = Iori_ROTD.new})
return Iori_ROTD