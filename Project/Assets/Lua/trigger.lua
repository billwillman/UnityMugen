local setmetatable = setmetatable
local mugen = mugen or {}
local trigger = mugen.trigger
if trigger ~= nil then
  return trigger
end

trigger = {}
mugen.trigger = trigger

-- 条件模块

function trigger:AnimElem(luaPlayer)
   if luaPlayer == nil then
	  return nil
   end
   local display = luaPlayer.PlayerDisplay;
   if display == nil then
	  return nil
   end
   local currentFrame = display:ImageCurrentFrame()
   if currentFrame == nil then
	  return nil
   end
   return currentFrame + 1
end

function trigger:AILevel(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.AILevel
end

function trigger:Alive(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	local ret = 0
	if attribe.IsAlive then
		ret = 1
	end
	return ret
end

function trigger:Anim(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display.Stateno
	return ret
end

function trigger.AnimExist(luaPlayer, state)
	if luaPlayer == nil or state == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display:HasAniGroup(state)
	return ret
end

function trigger:Facing(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local isFlipX = display.IsFlipX
	if not isFlipX then
		return 1
	else
		return 0
	end
end

function trigger:HitCount(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.HitCount
end

function trigger:Life(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.Life
end

function trigger:Power(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.Power
end

function trigger:Statetype(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.StandType
end

function trigger:CanCtrl(luaPlayer)
	local ctrl = self:Ctrl(luaPlayer)
	if ctrl == nil then
		return false
	end
	return ctrl ~= 0
end

function trigger:Ctrl(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display.Attribe
	if attribe == nil then
		return nil
	end
	return attribe.Ctrl
end

function trigger:Stateno(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local ret = display.Stateno
	return ret
end

function trigger:Time(luaPlayer)
end

-- 处理模块

function trigger:Do_PlaySnd()
end

-- 帮助模块
function trigger:Help_CreateStateDef(luaCfg, name)
	if luaCfg == nil or name == nil then
		return nil
	end
	local id = luaCfg:CreateStateDef(name)
	return id
end

function trigger:Help_GetStateDef(luaCfg, id)
	if luaCfg == nil or id == nil then
		return nil
	end
	local def = luaCfg:GetStateDef(id)
	return def;
end

function trigger:Help_InitLuaPlayer(newLuaPlayer, basePlayer)
	if newLuaPlayer == nil or basePlayer == nil then
		return
	end
	
	local org = basePlayer.Data
	if org == nil then
		return
	end
	
	local display = newLuaPlayer.PlayerDisplay;
	if display == nil then
		return 
	end
	
	local dst = display.Attribe
	if dst == nil then
		return
	end
	
	if org.life ~= nil then
		dst.life = org.life
	end
	
	if org.attack ~= nil then
		dst.attack = org.attack
	end
	
	if org.defence ~= nil then
		dst.defence = org.defence
	end
	
	if org.fall ~= nil and org.fall.defence_up ~= nil then
		dst.fail__defence_up = org.fall.defence_up
	end
	
	if org.liedown ~= nil and org.liedown.time ~= nil then
		dst.liedown__time = org.liedown.time
	end
	
	if org.airjuggle ~= nil then
		dst.airjuggle = org.airjuggle
	end
	
	if org.sparkno ~= nil then
		dst.sparkno = org.sparkno
	end
	
	if org.guard ~= nil and org.guard.sparkno ~= nil then
		dst.guard__sparkno = org.guard.sparkno
	end
	
	if org.KO ~= nil and org.KO.echo ~= nil then
		dst.ko__echo = org.KO.echo
	end
	
	if org.Power ~= nil then
		dst.Power = org.Power
	end
	
	if org.volume ~= nil then
		dst.volume = org.volume
	end
	
	if org.IntPersistIndex ~= nil then
		dst.IntPersistIndex = org.IntPersistIndex
	end
	
	if org.FloatPersistIndex ~= nil then
		dst.FloatPersistIndex = org.FloatPersistIndex
	end
	-- 初始化變量
	dst:InitVars()
	dst:ResetDatas()
end

return trigger
