local trigger = require("trigger")

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
    end
   -- 动态数据
   local t = {PlayerDisplay = nil}
   return setmetatable(t, kfm720)
end

function kfm720:OnInit(playerDisplay)
	self.PlayerDisplay = playerDisplay;
	trigger:Help_InitLuaPlayer(self, self)
end

function kfm720:OnDestroy()
  self.PlayerDisplay = nil
end

function kfm720:_initData()
  if self.Data ~= nil then
	return
  end
  self.Data = {};
  
  self.Data.life = 10000
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
end

function kfm720:_initSize()
  if self.Size ~= nil then
	return
  end
  self.Size = {}
  self.Size.xscale = 1
  self.Size.yscale = 1
end

--创建StateDef
function kfm720:_initStateDefs()
	local luaCfg = GlobaConfigMgr:GetLuaCnsCfg("kfm720")
	if luaCfg == nil then
		return
	end
	
	-- 创建各种状态
	self:_initStateDef_200(luaCfg)
end

function kfm720:_initStateDef_200(luaCfg)
	local id = trigger:Help_CreateStateDef(luaCfg, "200")
	local def = trigger:Help_GetStateDef(luaCfg, id)
	--Def注册State
end

setmetatable(kfm720, {__call = kfm720.new})
return kfm720