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

  self:_initCmds()
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

function Mai_XIII:initCmd_105(luaCfg)

--------------------------- register StateDef 105 ---------------------------
    local id = luaCfg:CreateStateDef("105")

    local def = luaCfg:GetStateDef(id)

    def.Type = Mugen.Cns_Type.S

    def.PhysicsType = Mugen.Cns_PhysicsType.N

    def.Juggle = 0

    def.PowerAdd = 0

    def.Velset_x = 0

    def.Velset_y = 0

    def.Ctrl = 0

    def.Sprpriority = 1

    def.Animate = 105

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

            if trigger1 then

                trigger:PlaySnd(luaPlayer, 105, 0)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:Var(luaPlayer, 29) >= 1) and (trigger:Time(luaPlayer) == 1))

            if trigger1 then

                trigger:StateTypeSet(luaPlayer, Mugen.Cns_Type.A)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:AnimElem(luaPlayer) == 2) and (trigger:Var(luaPlayer, 29) <= 0))

            if trigger1 then

                trigger:StateTypeSet(luaPlayer, Mugen.Cns_Type.A)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

            if trigger1 then

                trigger:VelSet(luaPlayer, -10 * VelSetPer, -5.5 * VelSetPer)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

            if trigger1 then

                trigger:VelAdd(luaPlayer, nil, 0.9)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            trigger:VelMul(luaPlayer, 0.95, nil)

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:VelY(luaPlayer) > 0) and (trigger:PosY(luaPlayer) >= 0))

            if trigger1 then

                trigger:PlayCnsByName(luaPlayer, "106", false)

            end

        end


end

function Mai_XIII:initCmd_106(luaCfg)

--------------------------- register StateDef 106 ---------------------------
    local id = luaCfg:CreateStateDef("106")

    local def = luaCfg:GetStateDef(id)

    def.Type = Mugen.Cns_Type.S

    def.PhysicsType = Mugen.Cns_PhysicsType.N

    def.Juggle = 0

    def.PowerAdd = 0

    def.Velset_x = 0

    def.Velset_y = 0

    def.Ctrl = 0

    def.Sprpriority = 0

    def.Animate = 106

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:RemoveExplod(luaPlayer, 3000)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:PlaySnd(luaPlayer, 41, 0)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:VelSet(luaPlayer, 0 * VelSetPer, 0 * VelSetPer)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:PosSet(luaPlayer, nil, 0)

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

function Mai_XIII:initCmd_170(luaCfg)

--------------------------- register StateDef 170 ---------------------------
    local id = luaCfg:CreateStateDef("170")

    local def = luaCfg:GetStateDef(id)

    def.Type = Mugen.Cns_Type.S

    def.Juggle = 0

    def.PowerAdd = 0

    def.Velset_x = 0

    def.Velset_y = 0

    def.Ctrl = 0

    def.Sprpriority = 0

    def.Animate = 170

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            trigger:CreateNotHit(luaPlayer, 1, 15, 0, 0, false, "")

        end


end

function Mai_XIII:initCmd_100(luaCfg)
--------------------------- register StateDef 100 ---------------------------
    local id = luaCfg:CreateStateDef("100")

    local def = luaCfg:GetStateDef(id)

    def.Type = Mugen.Cns_Type.S

    def.PhysicsType = Mugen.Cns_PhysicsType.S

    def.Juggle = 0

    def.PowerAdd = 0

    def.Ctrl = 0

    def.Sprpriority = 1

    def.Animate = 100

    def.AniLoop = true

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:PlaySnd(luaPlayer, 100, 0, true)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

            if trigger1 then

                trigger:VelSet(luaPlayer, 7 * VelSetPer, nil)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((not trigger:Command(luaPlayer, "holdfwd")) and (trigger:Time(luaPlayer) >= 100) and (trigger:Var(luaPlayer, 59) <= 0))

            if trigger1 then

                trigger:PlayCnsByName(luaPlayer, "101", false)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:Time(luaPlayer) == 0) and (trigger:Var(luaPlayer, 59) < 1))

            if trigger1 then

                trigger:CtrlSet(luaPlayer, 1)

            end

        end
end

function Mai_XIII:initCmd_FF(luaCfg)

--------------------------- register KeyCmd ---------------------------
    local cmd = luaCfg:CreateCmd("FF")
    cmd.time = 1
    cmd:AttachKeyCommands("")

--------------------------- FF ---------------------------
    local aiCmd = luaCfg:CreateAICmd("FF")
    aiCmd.type = Mugen.AI_Type.ChangeState
    aiCmd.value = "100"
    aiCmd.OnTriggerEvent =
        function (luaPlayer, aiName)
            local triggle1 = (trigger:Command(luaPlayer, "FF"))
            return triggle1
        end

end

function Mai_XIII:initCmd_1100(luaCfg)

--------------------------- register StateDef 1100 ---------------------------
    local id = luaCfg:CreateStateDef("1100")

    local def = luaCfg:GetStateDef(id)

    def.Type = Mugen.Cns_Type.S

    def.PhysicsType = Mugen.Cns_PhysicsType.N

    def.MoveType = Mugen.Cns_MoveType.A

    def.Juggle = 0

    def.PowerAdd = 100

    def.Velset_x = 0

    def.Velset_y = 0

    def.Ctrl = 0

    def.Sprpriority = 1

    def.Animate = 1100

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:VarSet(luaPlayer, 5, 0)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 3)

            if trigger1 then

                trigger:PlaySnd(luaPlayer, 1100, 0, false)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:Time(luaPlayer) == 0) and (trigger:Var(luaPlayer, 5) == 1))

            if trigger1 then

                trigger:PlayCnsByName(luaPlayer, "1101", false)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 4)

            if trigger1 then

                trigger:PlaySnd(luaPlayer, 1100, 1, false)

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

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:AnimElem(luaPlayer) == 3) and (trigger:Var(luaPlayer, 5) >= 1))

            if trigger1 then

                trigger:PosAdd(luaPlayer, 32, nil)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:AnimElem(luaPlayer) == 3) and (trigger:Var(luaPlayer, 5) < 1))

            if trigger1 then

                trigger:PosAdd(luaPlayer, 22, nil)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 4)

            if trigger1 then

                trigger:PosAdd(luaPlayer, 15, nil)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 17)

            if trigger1 then

                trigger:PosAdd(luaPlayer, 4, nil)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:AnimElem(luaPlayer) == 5) and (trigger:Anim(luaPlayer) == 1101))

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 1111

                explod.ID = 1110

                explod.postype = ExplodPosType.p1

                explod.bindtime = 1 * bindTimePer

                explod.removetime = -2

                explod.sprpriority = 5

                explod.removeongethit = 1

                explod.ignorehitpause = 0

                explod.isChangeStateRemove = false

                explod.IsUseParentUpdate = false

                explod.scale = Vector2.New(0.475, 0.475)

                explod:Apply()


            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:AnimElem(luaPlayer) == 5) and (trigger:Anim(luaPlayer) ~= 1101))

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 1110

                explod.ID = 1110

                explod.postype = ExplodPosType.p1

                explod.bindtime = 1 * bindTimePer

                explod.removetime = -2

                explod.sprpriority = 5

                explod.removeongethit = 1

                explod.ignorehitpause = 0

                explod.isChangeStateRemove = false

                explod.IsUseParentUpdate = false

                explod.scale = Vector2.New(0.475, 0.475)

                explod:Apply()


            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:AnimElem(luaPlayer) == 10) and (trigger:Anim(luaPlayer) == 1101))

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 1116

                explod.ID = 1115

                explod.pos_x = 80

                explod.postype = ExplodPosType.p1

                explod.bindtime = 1 * bindTimePer

                explod.removetime = -2

                explod.sprpriority = 5

                explod.removeongethit = 0

                explod.ignorehitpause = 1

                explod.isChangeStateRemove = true

                explod.IsUseParentUpdate = true

                explod.scale = Vector2.New(0.8, 0.8)

                explod:Apply()


            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:AnimElem(luaPlayer) == 10) and (trigger:Anim(luaPlayer) ~= 1101))

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 1115

                explod.ID = 1115

                explod.pos_x = 80

                explod.postype = ExplodPosType.p1

                explod.bindtime = 1 * bindTimePer

                explod.removetime = -2

                explod.sprpriority = 5

                explod.removeongethit = 0

                explod.ignorehitpause = 1

                explod.isChangeStateRemove = true

                explod.IsUseParentUpdate = true

                explod.scale = Vector2.New(0.8, 0.8)

                explod:Apply()


            end

        end


end




function Mai_XIII:initCmd_HuoShaoBtn(luaCfg)

--------------------------- register KeyCmd ---------------------------
    local cmd = luaCfg:CreateCmd("火烧")
    cmd.time = 1
    cmd:AttachKeyCommands("")

--------------------------- 火烧 ---------------------------
    local aiCmd = luaCfg:CreateAICmd("火烧")
    aiCmd.type = Mugen.AI_Type.ChangeState
    aiCmd.value = "1100"
    aiCmd.OnTriggerEvent =
        function (luaPlayer, aiName)
            local triggle1 = (trigger:Command(luaPlayer, "火烧"))
                and (trigger:Ctrl(luaPlayer) == 1)
                and (trigger:Statetype(luaPlayer) ~= Mugen.Cns_Type.A)
            return triggle1
        end

end

function Mai_XIII:initCmd_3000(luaCfg)

--------------------------- register StateDef 3000 ---------------------------
    local id = luaCfg:CreateStateDef("3000")

    local def = luaCfg:GetStateDef(id)

    def.Type = Mugen.Cns_Type.S

    def.PhysicsType = Mugen.Cns_PhysicsType.N

    def.MoveType = Mugen.Cns_MoveType.A

    def.Juggle = 0

    def.PowerAdd = 0

    def.Velset_x = 0

    def.Velset_y = 0

    def.Ctrl = 0

    def.Sprpriority = -1

    def.Animate = 3000

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:VarSet(luaPlayer, 5, 0)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:Time(luaPlayer) == 0) and (trigger:Numexplod(luaPlayer, 7910)))

            if trigger1 then

                trigger:RemoveExplod(luaPlayer, 7910)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) <= 32)

            if trigger1 then

                trigger:CreateNotHit(luaPlayer, 1, 15, 0, 0, false, "")

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:RemoveExplod(luaPlayer, 3000)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 3)

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 6600

                explod.pos_y = -35

                explod.postype = ExplodPosType.p1

                explod.bindtime = 1 * bindTimePer

                explod.removetime = -2

                explod.sprpriority = -999

                explod.removeongethit = 0

                explod.ignorehitpause = 1

                explod.isChangeStateRemove = false

                explod.IsUseParentUpdate = false

                explod.scale = Vector2.New(0.475, 0.475)

                explod:Apply()


            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 13)

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 6601

                explod.pos_y = -35

                explod.postype = ExplodPosType.p1

                explod.bindtime = 1 * bindTimePer

                explod.removetime = -2

                explod.sprpriority = 3

                explod.removeongethit = 0

                explod.ignorehitpause = 1

                explod.isChangeStateRemove = false

                explod.IsUseParentUpdate = false

                explod.scale = Vector2.New(0.475, 0.475)

                explod:Apply()


            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 1)

            if trigger1 then

                trigger:PlaySnd(luaPlayer, 2000, 0, false)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 10)

            if trigger1 then

                trigger:PlaySnd(luaPlayer, 2000, 1, false)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 1)

            if trigger1 then

                trigger:PlaySnd(luaPlayer, 3000, 0, false)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 2)

            if trigger1 then

                trigger:VelSet(luaPlayer, 6.5 * VelSetPer, nil)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 10)

            if trigger1 then

                trigger:StateTypeSet(luaPlayer, Mugen.Cns_Type.A)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 10)

            if trigger1 then

                trigger:PhysicsTypeSet(luaPlayer, Mugen.Cns_PhysicsType.N)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 10)

            if trigger1 then

                trigger:VelSet(luaPlayer, 6.5 * VelSetPer, -5 * VelSetPer)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 10)

            if trigger1 then

                trigger:VelAdd(luaPlayer, nil, 0.35)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:RemoveExplod(luaPlayer, 3001)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 7)

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 3001

                explod.ID = 3001

                explod.postype = ExplodPosType.p1

                explod.removetime = -2

                explod.sprpriority = 3

                explod.removeongethit = 1

                explod.ignorehitpause = 0

                explod.isChangeStateRemove = false

                explod.IsUseParentUpdate = false

                explod.scale = Vector2.New(0.475, 0.475)

                explod:Apply()


                trigger:Persistent(luaPlayer, state, true)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:AnimElem(luaPlayer) == 7)

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 3014

                explod.ID = 3014

                explod.postype = ExplodPosType.p1

                explod.removetime = -2

                explod.sprpriority = -1

                explod.removeongethit = 1

                explod.ignorehitpause = 0

                explod.isChangeStateRemove = false

                explod.IsUseParentUpdate = false

                explod.scale = Vector2.New(0.475, 0.475)

                explod:Apply()


                trigger:Persistent(luaPlayer, state, true)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:Numexplod(luaPlayer, 3111)) and (trigger:VelY(luaPlayer) > 0) and (trigger:PosY(luaPlayer) >= -10))

            if trigger1 then

                local explod = trigger:CreateExplod(luaPlayer)

                explod.anim = 3111

                explod.ID = 3111

                explod.pos_x = -50

                explod.postype = ExplodPosType.p1

                explod.bindtime = 1 * bindTimePer

                explod.removetime = -2

                explod.sprpriority = -1

                explod.removeongethit = 1

                explod.ignorehitpause = 0

                explod.isChangeStateRemove = false

                explod.IsUseParentUpdate = false

                explod.scale = Vector2.New(0.475, 0.475)

                explod:Apply()


                trigger:Persistent(luaPlayer, state, true)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:VarSet(luaPlayer, 10, 0)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = ((trigger:VelY(luaPlayer) > 0) and (trigger:PosY(luaPlayer) >= 0))

            if trigger1 then

                trigger:PlayCnsByName(luaPlayer, "3040", false)

            end

        end


end

function Mai_XIII:initCmd_3040(luaCfg)

--------------------------- register StateDef 1210 ---------------------------
    local id = luaCfg:CreateStateDef("3040")

    local def = luaCfg:GetStateDef(id)

    def.Type = Mugen.Cns_Type.S

    def.PhysicsType = Mugen.Cns_PhysicsType.S

    def.MoveType = Mugen.Cns_MoveType.I

    def.Juggle = 0

    def.PowerAdd = 0

    def.Velset_x = 0

    def.Velset_y = 0

    def.Sprpriority = 0

    def.Animate = 1210

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:RemoveExplod(luaPlayer, 3101)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:PosSet(luaPlayer, nil, 0)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local trigger1 = (trigger:Time(luaPlayer) == 0)

            if trigger1 then

                trigger:RemoveExplod(luaPlayer, 3001)

            end

        end

    local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem)

    state.OnTriggerEvent = 

        function (luaPlayer, state)

            local explod = trigger:CreateExplod(luaPlayer)

            explod.anim = 3041

            explod.ID = 3041

            explod.postype = ExplodPosType.p1

            explod.removetime = -2

            explod.sprpriority = 3

            explod.removeongethit = 0

            explod.ignorehitpause = 0

            explod.isChangeStateRemove = false

            explod.IsUseParentUpdate = false

            explod.scale = Vector2.New(0.475, 0.475)

            explod:Apply()


            trigger:Persistent(luaPlayer, state, true)

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

function Mai_XIII:initCmd_ChongFeng(luaCfg)

--------------------------- register KeyCmd ---------------------------
    local cmd = luaCfg:CreateCmd("冲锋")
    cmd.time = 1
    cmd:AttachKeyCommands("")

--------------------------- 冲锋 ---------------------------
    local aiCmd = luaCfg:CreateAICmd("冲锋")
    aiCmd.type = Mugen.AI_Type.ChangeState
    aiCmd.value = "3000"
    aiCmd.OnTriggerEvent =
        function (luaPlayer, aiName)
            local triggle1 = (trigger:Command(luaPlayer, "冲锋"))
                and (trigger:Ctrl(luaPlayer) == 1)
            return triggle1
        end

end



function Mai_XIII:_initCmds()
	local luaCfg = trigger:GetLuaCnsCfg("Mai_XIII")
	if luaCfg == nil then
		return
	end

  self:initCmd_FF(luaCfg)


  self:initCmd_101(luaCfg)
  self:initCmd_105(luaCfg)
  self:initCmd_106(luaCfg)
  self:initCmd_170(luaCfg)
  self:initCmd_100(luaCfg)
  self:initCmd_1100(luaCfg)
  self:initCmd_HuoShaoBtn(luaCfg)
  self:initCmd_3000(luaCfg)
  self:initCmd_3040(luaCfg)
  self:initCmd_ChongFeng(luaCfg)
end

setmetatable(Mai_XIII, {__call = Mai_XIII.new})

return Mai_XIII


