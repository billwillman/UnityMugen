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
   local currentFrame = display:ImageCurrentFrame
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
	local attribe = display:Attribe
	if attribe == nil then
		return nil
	end
	return attribe:AILevel
end

function trigger:Alive(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display:Attribe
	if attribe == nil then
		return nil
	end
	local ret = 0
	if attribe:IsAlive then
		ret = 1
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
	local ret = display:AnimationState
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
	local isFlipX = display:IsFlipX
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
	local attribe = display:Attribe
	if attribe == nil then
		return nil
	end
	return attribe:HitCount
end

function trigger:Life(luaPlayer)
	if luaPlayer == nil then
		return nil
	end
	local display = luaPlayer.PlayerDisplay;
	if display == nil then
		return nil 
	end
	local attribe = display:Attribe
	if attribe == nil then
		return nil
	end
	return attribe:Life
end

function trigger:Time(luaPlayer)
end

-- 处理模块

function trigger:Do_PlaySnd()
end

return trigger