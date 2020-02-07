local trigger = require("trigger")
local standDefName = "0"

local function _GetLuaCfg(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay
	if display == nil then
		return nil
	end
	local ret = display.LuaCfg
	return ret
end

local function _InitStatedef_0(luaPlayer, luaCfg)
	local id = luaCfg:CreateStateDef(standDefName)
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Sprpriority = 0
	def.Animate = 0
	
-- State 0, 1
	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local anim = trigger:Anim(luaPlayer)
			local trigger1 = anim ~= 0 and anim ~= 5
			local trigger2 = anim == 5 and trigger:AnimTime(luaPlayer) == 0
			if trigger1 or trigger2 then
				trigger:PlayAnim(luaPlayer, 0, true)
			end
		end
-- State 0, 2
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 0
			if trigger1 then
				trigger:VelSet(luaPlayer, nil, 0)
			end
		end
-- State 0, 3
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:Abs(trigger:VelX(luaPlayer)) < 2
			local trigger2 = trigger:Time(luaPlayer) == 4
			if trigger1 or trigger2 then
				trigger:VelSet(luaPlayer, 0, nil)
			end
		end
-- State 0, 4
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = not trigger:Alive(luaPlayer)
			if trigger1 then
				trigger:PlayCnsByName(luaPlayer, "5050")
			end
		end
	
	return id
end

local function _InitStatedef_100(luaPlayer, luaCfg)
	local aiCmd = luaCfg:CreateAICmd("Run Fwd", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "100"
	aiCmd.AniLoop = true
	aiCmd.OnTriggerEvent = 
		function (luaPlayer, cmdName)
			local trigger1 = trigger:Command(luaPlayer, "FF") and 
						trigger:Statetype(luaPlayer) == Mugen.Cns_Type.S and 
						trigger:CanCtrl(luaPlayer)
	
			return trigger1 
		end

	local id = luaCfg:CreateStateDef("100")
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Animate = 100
	def.Sprpriority = 1

	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			trigger:VelSet(luaPlayer, luaPlayer.velocity.run.fwd.x, nil)
		end
	return id
end

local function _InitJump(luaPlayer, luaCfg)
	local aiCmd = luaCfg:CreateAICmd("AirJump Start")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "45"
	aiCmd.AniLoop = true
	aiCmd.OnTriggerEvent = 
		function (luaPlayer, cmdName)
			local trigger1 = trigger:Command(luaPlayer, "holdup") and 
								trigger:Statetype(luaPlayer) == Mugen.Cns_Type.S and 
								trigger:CanCtrl(luaPlayer)
			return trigger1
		end

	local id = luaCfg:CreateStateDef("45")
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.A
	def.PhysicsType = Mugen.Cns_PhysicsType.N
	def.Ctrl = 0
	def.Velset_x = 0
	def.Velset_y = 0
	--def.Animate = -1
-- State 45, 1
-- State 45, 2
	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local tt = trigger:Time(luaPlayer)
			local stateNo = trigger:Stateno(luaPlayer)
			local triggerAll = tt == 0 and (stateNo ~= 44) and (stateNo ~= 41)
			local trigger1 = trigger:AnimExist(luaPlayer, 44)
			if triggerAll then
				if trigger1 then
					print("State 45, 1")
					trigger:PlayAnim(luaPlayer, 44)
				else
					print("State 45, 2")
					trigger:PlayAnim(luaPlayer, 41)
				end
				state.persistent = true
			end
		end
-- State 45, 3
-- State 45, 4
-- State 45, 5
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 0
			if trigger1 then
				if trigger:Command(luaPlayer, "holdfwd") then
					print("State 45, 4")
					trigger:VarSet(luaPlayer, 1, 1)
				elseif trigger:Command(luaPlayer, "holdback") then
					print("State 45, 5")
					trigger:VarSet(luaPlayer, 1, -1)
				else
					print("State 45, 3")
					trigger:VarSet(luaPlayer, 1, 0)
				end
			end
		end
-- State 45, 6
-- State 45, 7
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 2
			if trigger1 then
				print("State 45, 6")
				local var1 = trigger:Var(luaPlayer, 1)
				local x
				if var1 == 0 then
					x = luaPlayer.velocity.airjump.neu.x
				elseif var1 == 1 then
					x = luaPlayer.velocity.airjump.fwd.x
				else
					x = luaPlayer.velocity.airjump.back.x
				end

				local y = luaPlayer.velocity.airjump.y
				trigger:VelSet(luaPlayer, x, y)

				print("State 45, 7")
				trigger:CtrlSet(luaPlayer, 1)
				trigger:PlayCnsByName(luaPlayer, "50")
			end
		end
---- 50
	id = luaCfg:CreateStateDef("50")
	def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.A
	def.PhysicsType = Mugen.Cns_PhysicsType.A
	--def.Animate = -1
-- State 50, 1
-- State 50, 2
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local trigger1 = trigger:Time(luaPlayer) == 0
			if trigger1 then
				-- State 50, 1
				print("State 50, 1")
				trigger:VarSet(luaPlayer, 1, 0)
				-- State 50, 2
				print("State 50, 2")
				state.persistent = true
				if (trigger:Abs(trigger:VelX(luaPlayer)) <= 0.000001) then
					trigger:PlayAnim(luaPlayer, 41)
				elseif trigger:VelX(luaPlayer) > 0 then
					trigger:PlayAnim(luaPlayer, 42)
				else
					trigger:PlayAnim(luaPlayer, 43)
				end
			end
		end
-- State 50, 3
	state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimTime)
	state.OnTriggerEvent = 
		function (luaPlayer, state)
			local anim = trigger:Anim(luaPlayer)
			--print(anim + 3)
			
			local trigger1 = trigger:VelY(luaPlayer) > -2 and trigger:AnimExist(luaPlayer, anim + 2)
			if trigger1 then
				print("State 50, 3")
				-- 只执行一次
				state.persistent = true
				trigger:PlayAnim(luaPlayer, anim + 2)
			end
		end
end


local function _InitCmds(luaPlayer, luaCfg)
	_InitJump(luaPlayer, luaCfg)
	_InitStatedef_100(luaPlayer, luaCfg)
end

local function _InitCommonCns(luaPlayer)
	if luaPlayer == nil then
		return
	end
	local meta = getmetatable(luaPlayer)
	if meta == nil then
		return
	end
	if meta._isInitCommonCns then
		trigger:PlayCnsByName(luaPlayer, standDefName, true)
		return
	end
	local luaCfg = _GetLuaCfg(luaPlayer)
	if luaCfg == nil then
		return
	end
	meta._isInitCommonCns = true
	
	local standId = _InitStatedef_0(luaPlayer, luaCfg)
	
	_InitCmds(luaPlayer, luaCfg)
	
	trigger:PlayCns(luaPlayer, standId, true)
end

return _InitCommonCns