local trigger = require("trigger")
local _InitCommonCns = require("commonCns")

local setmetatable = setmetatable

local AshXIII = {}
AshXIII.__index = AshXIII

function AshXIII:new()
   -- 静态数据
   if self._isInit == nil then
		self._isInit = true
		self:_initData()
		self:_initSize()
		self:_initCmds()
    end
   -- 动态数据
   local t = {PlayerDisplay = nil}
   local ret = setmetatable(t, AshXIII)
   --print(ret)
   return ret
end

function AshXIII:OnInit(playerDisplay)
	--print(playerDisplay)
	self.PlayerDisplay = playerDisplay;
	--print(self.PlayerDisplay)
	trigger:Help_InitLuaPlayer(self, self)
	-- 初始化默认Cns状态
	_InitCommonCns(self)
end

function AshXIII:OnDestroy()
  self.PlayerDisplay = nil
  --print(null)
end

function AshXIII:OnGetAICommandName(cmdName)
	return ""
end

--------------------------------------------------------------------------------------------------------------------------------------------

function AshXIII:_initData()
	if self.Data ~= nil then
	return
  end
  self.Data = {};
  
  self.Data.life = 1000
  self.Data.Power = 5000
  self.Data.attack = 100
  self.Data.defence = 100
  
  
  self.Data.fall = {}
  self.Data.fall.defence_up = 50
  
  self.Data.liedown = {}
  self.Data.liedown.time = 20
  
  self.Data.airjuggle = 15
  self.Data.sparkno = 2
  
  self.Data.guard = {}
  self.Data.guard.sparkno = 40
  
  self.Data.KO = {}
  self.Data.KO.echo = 0
  
  self.Data.volume = 50
  self.Data.IntPersistIndex = 0
  self.Data.FloatPersistIndex = 0

  	self.velocity = {}
	self.velocity.run = {}
	self.velocity.run.fwd = Vector2.New(5.65625, 0)
	self.velocity.run.back = Vector2.New(-12,-5.7)
end

function AshXIII:_initSize()
	if self.Size ~= nil then
	return
  end
  self.Size = {}
  self.Size.xscale = 1
  self.Size.yscale = 1
end

function AshXIII:_initCmds()
	local luaCfg = trigger:GetLuaCnsCfg("AshXIII")
	if luaCfg == nil then
		return
	end

	local cmd = luaCfg:CreateCmd("FF", "")
	cmd.time = 10
	cmd:AttachKeyCommands("F,F")


end

--------------------------------------------------------------------------------------------------------------------------------------------

setmetatable(AshXIII, {__call = AshXIII.new})

return AshXIII


