local trigger = require("trigger")
local _InitCommonCns = require("commonCns")

local setmetatable = setmetatable

local Mai_XIII = {}
Mai_XIII.__index = Mai_XIII

function Mai_XIII:new()
	-- 静态数据
   if self._isInit == nil then
		self._isInit = true
		self:_initData()
		self:_initSize()
		self:_initCmds()
    end
   -- 动态数据
   local t = {PlayerDisplay = nil}
   local ret = setmetatable(t, Mai_XIII)
   --print(ret)
   return ret
end

--====================外部调用接口==============================

function Mai_XIII:OnInit(playerDisplay)
	--print(playerDisplay)
	self.PlayerDisplay = playerDisplay;
	--print(self.PlayerDisplay)
	trigger:Help_InitLuaPlayer(self, self)
	-- 初始化默认Cns状态
	_InitCommonCns(self)
end

function Mai_XIII:OnDestroy()
  self.PlayerDisplay = nil
  --print(null)
end

function Mai_XIII:OnGetAICommandName(cmdName)
	return ""
end

--===========================================================

function Mai_XIII:_initData()
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
  
  self.Data.volume = 255
  self.Data.IntPersistIndex = 60
  self.Data.FloatPersistIndex = 40

  	self.velocity = {}
	self.velocity.run = {}
	self.velocity.run.fwd = Vector2.New(7, 0)
	self.velocity.run.back = Vector2.New(-4,-3.5)
end

function Mai_XIII:_initSize()
  if self.Size ~= nil then
	return
  end
  self.Size = {}
  self.Size.xscale = 1
  self.Size.yscale = 1
end

function Mai_XIII:initCmd_101(luaCfg)

--------------------------- register StateDef 101 ---------------------------
    local id = luaCfg:CreateStateDef("101")

    local def = luaCfg:GetStateDef(id)

    def.Type = Mugen.Cns_Type.S

    def.PhysicsType = Mugen.Cns_PhysicsType.S

    def.Juggle = 0

    def.PowerAdd = 0

    def.Velset_x = 0

    def.Velset_y = 0

    def.Ctrl = 1

    def.Sprpriority = 1

    def.Animate = 101

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

function Mai_XIII:_initCmds()
	local luaCfg = trigger:GetLuaCnsCfg("Mai_XIII")
	if luaCfg == nil then
		return
	end

  self:initCmd_101(luaCfg)
end

setmetatable(Mai_XIII, {__call = Mai_XIII.new})

return Mai_XIII


