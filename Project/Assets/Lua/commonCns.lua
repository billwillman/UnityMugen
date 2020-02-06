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

function _On_0(luaPlayer, state)
	--print("_On_0")
	local anim = trigger:Anim(luaPlayer)
	local trigger1 = anim ~= 0 and anim ~= 5
	local trigger2 = anim == 5 and trigger:AnimTime(luaPlayer) == 0
	if trigger1 or trigger2 then
		trigger:PlayAnim(luaPlayer, 0, true)
	end
end

local function _InitStatedef_0(luaPlayer, luaCfg)
	local id = luaCfg:CreateStateDef(standDefName)
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Sprpriority = 0
	def.Animate = 0
	
	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem, Mugen.CnsStateType.none)
	state.OnTriggerEvent = _On_0
	
	return id
end

function _On_100(luaPlayer, state)
	trigger:VelSet(luaPlayer, 18.4, nil)
end

local function _InitStatedef_100(luaPlayer, luaCfg)
	local aiCmd = luaCfg:CreateAICmd("Run Fwd", "")
	aiCmd.type = Mugen.AI_Type.ChangeState
	aiCmd.value = "100"
	aiCmd.AniLoop = true
	aiCmd.OnTriggerEvent = 
		function (cmdName)
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
	state.OnTriggerEvent = _On_100
	return id
end


local function _InitCmds(luaPlayer, luaCfg)
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