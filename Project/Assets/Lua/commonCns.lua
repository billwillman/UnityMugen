local trigger = require("trigger")

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
	local id = luaCfg:CreateStateDef("0")
	local def = luaCfg:GetStateDef(id)
	def.Type = Mugen.Cns_Type.S
	def.PhysicsType = Mugen.Cns_PhysicsType.S
	def.Sprpriority = 0
	def.Animate = 0
	
	local state = def:CreateStateEvent(Mugen.CnsStateTriggerType.AnimElem, Mugen.CnsStateType.none)
	state.OnTriggerEvent = _On_0
	
	return id
end


local function _InitCommonCns(luaPlayer)
	if luaPlayer == nil then
		return
	end
	local luaCfg = _GetLuaCfg(luaPlayer)
	if luaCfg == nil then
		return
	end
	local standId = _InitStatedef_0(luaPlayer, luaCfg)
	
	trigger:PlayCns(luaPlayer, standId, true)
end

return _InitCommonCns