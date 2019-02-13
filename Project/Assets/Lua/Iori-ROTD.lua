local trigger = require("trigger")

local setmetatable = setmetatable
local GlobaConfigMgr = MonoSingleton_GlobalConfigMgr.GetInstance()

local Iori-ROTD = {}
Iori-ROTD.__index = Iori-ROTD


function Iori-ROTD:new()
   -- 静态数据
   if self._isInit == nil then
		self._isInit = true
		self:_initData()
		self:_initSize()
		self:_initStateDefs()
    end
   -- 动态数据
   local t = {PlayerDisplay = nil}
   return setmetatable(t, Iori-ROTD)
end

function Iori-ROTD:OnInit(playerDisplay)
	self.PlayerDisplay = playerDisplay;
	trigger:Help_InitLuaPlayer(self, self)
end

function Iori-ROTD:OnDestroy()
  self.PlayerDisplay = nil
end

function Iori-ROTD:_initData()
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

function Iori-ROTD:_initSize()
  if self.Size ~= nil then
	return
  end
  self.Size = {}
  self.Size.xscale = 1
  self.Size.yscale = 1
end


setmetatable(Iori-ROTD, {__call = Iori-ROTD.new})
return Iori-ROTD